
using PracticalWork2Calculator.Models;
using PracticalWork2Calculator.Services;

namespace PracticalWork2Calculator.Views;

public partial class UserInfoPage : ContentPage
{
    private readonly UserManager _userManager;
    private string _currentUsername;

	public UserInfoPage(UserManager userManager)
	{
		InitializeComponent();
        _userManager = userManager;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _currentUsername = LoginPage.CurrentUser;
        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        if (!string.IsNullOrEmpty(_currentUsername))
        {
            User currentUser = _userManager.GetUser(_currentUsername);
            if (currentUser != null)
            {
                NameLabel.Text = $"Nombre: {currentUser.Name}";
                UsernameLabel.Text = $"Nombre de Usuario: {currentUser.Username}";
                EmailLabel.Text = $"Correo: {currentUser.Email}";
                PasswordLabel.Text = $"Contraseña: {currentUser.Password}"; // Mostrando contraseña [cite: 11]
                OperationsLabel.Text = $"Operaciones Ejecutadas: {currentUser.OperationsExecuted}"; // [cite: 11]
            }
            else
            {
                
                DisplayAlert("Error", "No se pudo cargar la información del usuario.", "OK");
                Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
        else
        {
             
            DisplayAlert("Error", "No hay un usuario activo.", "OK");
            Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
     private async void OnBackToConverterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(ConverterPage)}");
    }
}