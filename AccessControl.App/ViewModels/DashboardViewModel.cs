using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccessControl.Shared.DTOs; // Necesario para AccionRequest
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using System.Net.Http.Json; // Necesario para llamadas HTTP

namespace AccessControl.App.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        [ObservableProperty]
        private string nombreResidente;

        // NUEVO: El cliente HTTP para hacer las peticiones
        private readonly HttpClient _httpClient;

        public DashboardViewModel()
        {
            NombreResidente = Preferences.Default.Get("NombreUsuario", "Residente");
            _httpClient = new HttpClient(); // En Azure ya no necesitamos evadir los certificados locales
        }

        #region GESTION ACCESO

        [RelayCommand]
        public async Task AbrirPortonVehicularAsync()
        {
            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "🚘 Acceso Vehicular",
                "¿Deseas abrir el portón principal?",
                "Sí, Abrir", "Cancelar");

            if (confirmar)
            {
                // Disparamos la petición a la API en Azure
                await EjecutarAccionEnApi("vehicular");
            }
        }

        [RelayCommand]
        public async Task AbrirPuertaPeatonalAsync()
        {
            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "🚶 Acceso Peatonal",
                "¿Deseas abrir la puerta peatonal?",
                "Sí, Abrir", "Cancelar");

            if (confirmar)
            {
                // Disparamos la petición a la API en Azure
                await EjecutarAccionEnApi("peatonal");
            }
        }

        #endregion

        #region SEGURIDAD - CAMARAS Y ALARMAS
        [RelayCommand]
        public async Task VerCamarasAsync()
        {
            await Application.Current.MainPage.DisplayAlert("Próximamente", "La vista de cámaras está en construcción.", "OK");
        }

        [RelayCommand]
        public async Task ActivarAlarmaAsync()
        {
            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "🚨 Activar Alarma",
                "¿Seguro que desea activar la alarma vecinal? Solo la podrá apagar el administrador.",
                "Sí, Activar", "Cancelar");

            if (confirmar)
            {
                // Disparamos la petición a la API en Azure
                await EjecutarAccionEnApi("alarma");
            }
        }

        #endregion

        #region CERRAR SESION

        [RelayCommand]
        public async Task CerrarSesionAsync()
        {
            bool confirmar = await Application.Current.MainPage.DisplayAlert(
                "Cerrar Sesión",
                "¿Estás seguro de que deseas salir de tu cuenta?",
                "Sí, salir",
                "Cancelar");

            if (confirmar)
            {
                Preferences.Default.Remove("IsLoggedIn");
                Preferences.Default.Remove("UsuarioId"); // Agregamos limpiar el ID
                Preferences.Default.Remove("NombreUsuario");
                Preferences.Default.Remove("EsAdmin");

                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
        #endregion

        #region MÉTODO MAESTRO API
        // Este método centraliza todas las llamadas hacia tu API de Azure
        private async Task EjecutarAccionEnApi(string endpoint)
        {
            int usuarioId = Preferences.Default.Get("UsuarioId", 0);

            if (usuarioId == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Sesión inválida. Por favor cierra sesión y vuelve a entrar.", "OK");
                return;
            }

            try
            {
                var request = new AccionRequest { UsuarioId = usuarioId };

                // Tu URL mágica de Azure apuntando a AccionesController
                string apiUrl = $"https://api-catalanes-g2egcwbqhjecd5f3.canadacentral-01.azurewebsites.net/api/Acciones/{endpoint}";

                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Operación realizada correctamente en la Privada.", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Denegado", "El servidor rechazó la acción.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error de Conexión", $"No se pudo alcanzar el servidor en la nube: {ex.Message}", "OK");
            }
        }
        #endregion
    }
}