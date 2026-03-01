using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage; 

namespace AccessControl.App.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        // Esta propiedad se enlazará a la etiqueta de bienvenida
        [ObservableProperty]
        private string nombreResidente;

        public DashboardViewModel()
        {
            // Buscamos el nombre del usuario y sino se encuentra se pondrá Residente
            NombreResidente = Preferences.Default.Get("NombreUsuario", "Residente");
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
                // TODO: Aquí llamaremos al PortonesController de la API
                await Application.Current.MainPage.DisplayAlert("Éxito", "Abriendo portón vehicular...", "OK");
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
                // TODO: Aquí llamaremos al PortonesController de la API
                await Application.Current.MainPage.DisplayAlert("Éxito", "Abriendo puerta peatonal...", "OK");
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
                // TODO: Aquí llamaremos a la API para detonar la alarma
                await Application.Current.MainPage.DisplayAlert("Alarma", "¡Alarma activada con éxito!", "Entendido");
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
                //Limpiamos se borra la sesion
                Preferences.Default.Remove("IsLoggedIn");
                Preferences.Default.Remove("NombreUsuario");
                Preferences.Default.Remove("EsAdmin");

                //Regresamos al Login
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
        #endregion
    }
}
