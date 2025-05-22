
using PracticalWork2Calculator.Services;

namespace PracticalWork2Calculator.Views;

public partial class RegisterPage : ContentPage
{
    private readonly UserManager _userManager;
    private const string PolicyTerms = 
        "Política de Protección de Datos:\n\n" +
        "1. Recopilación de datos: Recopilamos nombre, nombre de usuario, correo electrónico y contraseña para el funcionamiento de la aplicación.\n" +
        "2. Uso de datos: Los datos se utilizan para la autenticación y el seguimiento del número de operaciones.\n" +
        "3. Almacenamiento: La información del usuario se almacena localmente en un archivo JSON en el directorio de datos de la aplicación.\n" +
        "4. Seguridad: Las contraseñas se almacenan tal como se ingresan (para fines de este ejercicio práctico). En aplicaciones reales, deben usarse hashes seguros.\n" +
        "5. Consentimiento: Al registrarse, usted acepta esta política.";


    public RegisterPage(UserManager userManager)
    {
        InitializeComponent();
        _userManager = userManager;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        string name = NameEntry.Text;
        string username = UsernameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(username) || 
            string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || 
            string.IsNullOrWhiteSpace(confirmPassword))
        {
            await DisplayAlert("Error", "Todos los campos son obligatorios.", "OK");
            return;
        }

        if (username.Equals(name, StringComparison.OrdinalIgnoreCase)) 
        {
             await DisplayAlert("Error de Registro", "El nombre y el nombre de usuario deben ser diferentes.", "OK");
             return;
        }
        if (password != confirmPassword) 
        {
            await DisplayAlert("Error de Registro", "Las contraseñas no coinciden.", "OK");
            return;
        }

        if (!PasswordValidator.IsPasswordStrong(password)) 
        {
            await DisplayAlert("Error de Registro", "La contraseña no cumple los requisitos: al menos 8 caracteres, una mayúscula, una minúscula, un número y un símbolo especial.", "OK");
            return;
        }
        
        if (!PolicyCheckBox.IsChecked) 
        {
            await DisplayAlert("Error de Registro", "Debes aceptar la política de protección de datos.", "OK");
            return;
        }

        var (success, message) = await _userManager.RegisterUserAsync(name, username, email, password);
        if (success)
        {
            await DisplayAlert("Registro Exitoso", message, "OK");
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
        else
        {
            await DisplayAlert("Error de Registro", message, "OK");
        }
    }
    private async void OnPolicyTapped(object sender, TappedEventArgs e) 
    {
        await DisplayAlert("Política de Protección de Datos", PolicyTerms, "Entendido");
    }
    private async void OnBackToLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}