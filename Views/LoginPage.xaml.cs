
using PracticalWork2Calculator.Services;

namespace PracticalWork2Calculator.Views;

public partial class LoginPage : ContentPage
{
    private readonly UserManager _userManager;
    public static string CurrentUser { get; set; }


    public LoginPage(UserManager userManager)
    {
        InitializeComponent();
        _userManager = userManager;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Error", "Por favor, ingresa usuario y contraseña.", "OK");
            return;
        }

        var (success, user, message) = await _userManager.LoginUserAsync(username, password);
        if (success)
        {
            CurrentUser = user.Username; 
            await Shell.Current.GoToAsync($"//{nameof(ConverterPage)}");
            UsernameEntry.Text = string.Empty; 
            PasswordEntry.Text = string.Empty;
        }
        else
        {
            await DisplayAlert("Error de Inicio de Sesión", message, "OK");
            PasswordEntry.Text = string.Empty;
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }

    private async void OnPasswordRecoveryClicked(object sender, EventArgs e) 
    {
        await Shell.Current.GoToAsync(nameof(PasswordRecoveryPage));
    }
}