using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccessControl.Shared.DTOs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Net.Http.Json;

namespace AccessControl.App.ViewModels
{
    // Heredar de ObservableObject es la magia del CommunityToolkit
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private bool recordarme;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private bool isBusy;

        private readonly HttpClient _httpClient;

        public LoginViewModel()
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
        }

        [RelayCommand]
        public async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                MostrarError("Por favor ingresa tu correo y contraseña.");
                return;
            }

            IsBusy = true;
            HasError = false;

            try
            {
                var request = new LoginRequest { Email = this.Email, Password = this.Password };

                // Usamos http:// (sin la S) y la ruta correcta a tu PC
                string apiUrl = "http://192.168.1.156:7101/api/Auth/login";

                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    if (loginResponse != null && loginResponse.Exito)
                    {
                        // ----- LÓGICA DE GUARDAR SESIÓN -----
                        if (Recordarme)
                        {
                            Preferences.Default.Set("IsLoggedIn", true);
                            Preferences.Default.Set("NombreUsuario", loginResponse.NombreCompleto);
                            Preferences.Default.Set("EsAdmin", loginResponse.EsAdministrador);
                        }
                        else
                        {
                            // Si desmarcó la casilla, limpiamos todo por seguridad
                            Preferences.Default.Remove("IsLoggedIn");
                            Preferences.Default.Remove("NombreUsuario");
                            Preferences.Default.Remove("EsAdmin");
                        }
                        // -------------------------------------

                        // Navegamos al Dashboard
                        await Shell.Current.GoToAsync(nameof(MainPage));
                    }
                }
                else
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    MostrarError(errorResponse?.Mensaje ?? "Credenciales incorrectas.");
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void MostrarError(string mensaje)
        {
            ErrorMessage = mensaje;
            HasError = true;
        }
    }
}