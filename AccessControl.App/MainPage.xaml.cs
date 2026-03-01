using AccessControl.App.Views;

namespace AccessControl.App
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnDashboardClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));

        }
        //Enviar señal para abrir Porton Vehicular
        private async void OnPortonVehicularClicked(object sender, EventArgs e)
        {
            await  DisplayAlert("Próximamente", "La lógica se implementará eventualmente.", "OK");

        }

        //Enviar notificacion el ESP32 para abrir la puerta peatonal
        private async void OnPuertaPeatonalClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Próximamente", "La lógica se implementará eventualmente.", "OK");
        }

        //Metodo para gestion de Camaras de Vigilancia
        private async void OnCamarasVigilanciaClicked(object sender, EventArgs e)
        {
            // Cuando tengamos la vista de cámaras, aquí pondremos la navegación
            // await Shell.Current.GoToAsync(nameof(CamarasPage));
            await DisplayAlert("Próximamente", "La vista de cámaras está en construcción.", "OK");
        }
        private async void OnAlarmaClicked(object sender, EventArgs e) {

            bool confirmar = await DisplayAlert(
                "Activar Alarma",//Titilo de la alarma
                "¿Seguro que desea activar la Alarma Vecinal? Solo podrá desactivarla el administrador.",//Advetencia
                "Si, Activar", //Devuelve TRUE
                "Cancelar"// Devuelve FALSE
                );
            if (confirmar)
            {
                await DisplayAlert("Alarma", "¡Alarma activada con exito!", "Entendido");
            }
        }
    }
}

