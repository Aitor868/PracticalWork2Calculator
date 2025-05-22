
using PracticalWork2Calculator.Services;

namespace PracticalWork2Calculator.Views;

public partial class ConverterPage : ContentPage
{
    private readonly UserManager _userManager;
    private string _currentUsername;

    public ConverterPage(UserManager userManager)
    {
        InitializeComponent();
        _userManager = userManager;
        ConversionTypePicker.SelectedIndexChanged += ConversionTypePicker_SelectedIndexChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUsername = LoginPage.CurrentUser; 
        if (!string.IsNullOrEmpty(_currentUsername))
        {
            CurrentUserLabel.Text = $"Usuario: {_currentUsername}";
        }
        else
        {
            
            Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        ResultLabel.Text = "Resultado:"; 
        InputEntry.Text = string.Empty; 
        ConversionTypePicker.SelectedIndex = -1; 
        BitsEntry.IsVisible = false; 
    }

    private void ConversionTypePicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedConversion = ConversionTypePicker.SelectedItem as string;
        if (selectedConversion == "Decimal a Complemento a Dos" || selectedConversion == "Complemento a Dos a Decimal")
        {
            BitsEntry.IsVisible = true;
        }
        else
        {
            BitsEntry.IsVisible = false;
        }
    }

    private void OnNumericButtonClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        InputEntry.Text += button?.Text;
    }

    private void OnClearClicked(object sender, EventArgs e) // [cite: 10]
    {
        InputEntry.Text = string.Empty;
        ResultLabel.Text = "Resultado:";
        BitsEntry.Text = string.Empty;
    }

    private async void OnConvertClicked(object sender, EventArgs e)
    {
        string inputValue = InputEntry.Text;
        string selectedConversion = ConversionTypePicker.SelectedItem as string;
        int? bits = null;

        if (string.IsNullOrEmpty(inputValue))
        {
            await DisplayAlert("Error", "Por favor, ingresa un valor.", "OK");
            return;
        }
        if (selectedConversion == null)
        {
            await DisplayAlert("Error", "Por favor, selecciona un tipo de conversión.", "OK");
            return;
        }
        
        if (BitsEntry.IsVisible)
        {
            if (string.IsNullOrWhiteSpace(BitsEntry.Text) || !int.TryParse(BitsEntry.Text, out int b) || b <= 0)
            {
                await DisplayAlert("Error", "Por favor, ingresa un número de bits válido para C2.", "OK"); // [cite: 13]
                return;
            }
            bits = b;
        }

        string fromBase = "";
        string result = "Error";

        try
        {
            switch (selectedConversion)
            {
                case "Binario a Decimal":
                    fromBase = "Binary";
                    var (isValidBin, errMsgBin) = ConverterLogic.ValidateInput(inputValue, fromBase);
                    if (!isValidBin) { await DisplayAlert("Entrada Inválida", errMsgBin, "OK"); InputEntry.Text = ""; return; } // [cite: 14]
                    result = ConverterLogic.ConvertBinaryToDecimal(inputValue);
                    break;
                case "Decimal a Binario":
                    fromBase = "Decimal";
                     var (isValidDecBin, errMsgDecBin) = ConverterLogic.ValidateInput(inputValue, fromBase);
                    if (!isValidDecBin) { await DisplayAlert("Entrada Inválida", errMsgDecBin, "OK"); InputEntry.Text = ""; return; }
                    result = ConverterLogic.ConvertDecimalToBinary(inputValue);
                    break;
                case "Decimal a Octal":
                    fromBase = "Decimal";
                    var (isValidDecOct, errMsgDecOct) = ConverterLogic.ValidateInput(inputValue, fromBase);
                    if (!isValidDecOct) { await DisplayAlert("Entrada Inválida", errMsgDecOct, "OK"); InputEntry.Text = ""; return; }
                    result = ConverterLogic.ConvertDecimalToOctal(inputValue);
                    break;
                case "Decimal a Hexadecimal":
                    fromBase = "Decimal";
                    var (isValidDecHex, errMsgDecHex) = ConverterLogic.ValidateInput(inputValue, fromBase);
                    if (!isValidDecHex) { await DisplayAlert("Entrada Inválida", errMsgDecHex, "OK"); InputEntry.Text = ""; return; }
                    result = ConverterLogic.ConvertDecimalToHexadecimal(inputValue);
                    break;
                case "Octal a Decimal":
                    fromBase = "Octal";
                    var (isValidOct, errMsgOct) = ConverterLogic.ValidateInput(inputValue, fromBase);
                    if (!isValidOct) { await DisplayAlert("Entrada Inválida", errMsgOct, "OK"); InputEntry.Text = ""; return; }
                    result = ConverterLogic.ConvertOctalToDecimal(inputValue);
                    break;
                case "Hexadecimal a Decimal":
                    fromBase = "Hexadecimal";
                    var (isValidHex, errMsgHex) = ConverterLogic.ValidateInput(inputValue, fromBase);
                    if (!isValidHex) { await DisplayAlert("Entrada Inválida", errMsgHex, "OK"); InputEntry.Text = ""; return; }
                    result = ConverterLogic.ConvertHexadecimalToDecimal(inputValue.ToUpper());
                    break;
                case "Decimal a Complemento a Dos":
                    fromBase = "Decimal";
                    var (isValidDecC2, errMsgDecC2) = ConverterLogic.ValidateInput(inputValue, fromBase, bits);
                    if (!isValidDecC2) { await DisplayAlert("Entrada Inválida", errMsgDecC2, "OK"); InputEntry.Text = ""; return; }
                    result = ConverterLogic.ConvertDecimalToTwoComplement(inputValue, bits.Value);
                    break;
                case "Complemento a Dos a Decimal":
                    fromBase = "TwoComplement";
                     var (isValidC2, errMsgC2) = ConverterLogic.ValidateInput(inputValue, fromBase, bits);
                    if (!isValidC2) { await DisplayAlert("Entrada Inválida", errMsgC2, "OK"); InputEntry.Text = ""; return; }
                    result = ConverterLogic.ConvertTwoComplementToDecimal(inputValue, bits.Value);
                    break;
                default:
                    await DisplayAlert("Error", "Tipo de conversión no implementado.", "OK");
                    return;
            }
            ResultLabel.Text = $"Resultado: {result}";
            _userManager.IncrementOperations(_currentUsername); 
        }
        catch (ArgumentOutOfRangeException ex) 
        {
            await DisplayAlert("Error de Conversión", ex.Message, "OK"); 
            ResultLabel.Text = "Resultado: Error de Rango";
            InputEntry.Text = ""; 
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error de Conversión", $"Ocurrió un error: {ex.Message}", "OK"); 
            ResultLabel.Text = "Resultado: Error";
            InputEntry.Text = ""; 
        }
    }

    private async void OnViewInfoClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(UserInfoPage));
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        LoginPage.CurrentUser = null; // Limpiar usuario actual
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}