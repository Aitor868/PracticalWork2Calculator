
namespace PracticalWork2Calculator.Views;

public partial class PasswordRecoveryPage : ContentPage
{
	public PasswordRecoveryPage()
	{
		InitializeComponent();
	}

    private async void OnBackToLoginClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}