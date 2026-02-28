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
        // El atributo [ObservableProperty] genera automáticamente la propiedad pública 
        // y el código que le avisa a la interfaz gráfica cuando el valor cambia.
        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool hasError;

        [ObservableProperty]
        private bool isBusy; 

        private readonly HttpClient _httpClient;

        public LoginViewModel()
        {
            // Nota de desarrollo local: Evitamos validaciones SSL estrictas 
            // que suelen dar problemas en emuladores de Android con certificados locales.
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
        }

        // [RelayCommand] convierte este método en un ICommand que el botón en XAML puede ejecutar
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

                // Solo usamos http:// (sin la S) y la ruta correcta
                string apiUrl = "http://192.168.100.159:5168/api/Auth/login";

                //Quiza se usen eventualmente
                //    ? "https://192.168.100.159:5168/api/Auth/login"
                //    : "https://192.168.100.159:5168/api:Auth/login";

                // Hacemos la petición POST a la API
                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    if (loginResponse != null && loginResponse.Exito)
                    {
                        // 1. Mostrar un Pop-up con el nombre real que viene de la Base de Datos
                        await Application.Current.MainPage.DisplayAlert(
                            "¡Conexión Exitosa!",
                            $"Bienvenido a la Privada, {loginResponse.NombreCompleto}.",
                            "¡Genial!");

                        // 2. Comentamos temporalmente la navegación para que la app no crashee
                        // await Shell.Current.GoToAsync("//MainPage"); 
                    }
                }
                else
                {
                    // Si la API nos rebotó (Código 400 o 401), leemos el mensaje de tu backend
                    var errorResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    MostrarError(errorResponse?.Mensaje ?? "Credenciales incorrectas.");
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al conectar con el servidor.");
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
