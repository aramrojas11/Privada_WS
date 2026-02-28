using AccessControl.App.ViewModels;

namespace AccessControl.App.Views;


public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();

        BindingContext = new LoginViewModel();
    }
}