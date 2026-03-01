using AccessControl.App.ViewModels;

namespace AccessControl.App.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Si la llave existe y es "true", nos brincamos esta pantalla sin animaciones
        bool isLoggedIn = Preferences.Default.Get("IsLoggedIn", false);

        if (isLoggedIn)
        {
            await Shell.Current.GoToAsync(nameof(MainPage), false); // El 'false' quita la animación
        }
    }

    private void OnEyeClicked(object sender, EventArgs e)
    {
        //Invertir el estado de IsPassword
        PasswordEnrty.IsPassword = !PasswordEnrty.IsPassword;

        //Se cambia la imagen del boton segun el estado
        if (PasswordEnrty.IsPassword)
        {
            EyeBtn.Source = "eye_open_icon.svg";
        }
        else
        {
            EyeBtn.Source = "eye_closed_icon.svg";
        }
    }
}