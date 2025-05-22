
using PracticalWork2Calculator.Views;

namespace PracticalWork2Calculator;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        Routing.RegisterRoute(nameof(PasswordRecoveryPage), typeof(PasswordRecoveryPage));
        Routing.RegisterRoute(nameof(ConverterPage), typeof(ConverterPage));
        Routing.RegisterRoute(nameof(UserInfoPage), typeof(UserInfoPage));
    }
}