using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Text;
using System.Text.Json;

namespace AccessControl.Services
{
    public class MqttService
    {
        private IMqttClient _client;
        private bool _continuarVibrando = false;

        private string GetUniqueClientId()
        {
            string deviceId = Preferences.Get("DeviceMqttId", string.Empty);
            if (string.IsNullOrEmpty(deviceId))
            {
                deviceId = $"{DeviceInfo.Name.Replace(" ", "_")}_{Guid.NewGuid().ToString().Substring(0, 5)}";
                Preferences.Set("DeviceMqttId", deviceId);
            }
            return deviceId;
        }

        public async Task ConnectAsync()
        {
            if (_client != null && _client.IsConnected) return;

            var factory = new MqttFactory();
            _client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithClientId(GetUniqueClientId())
                .WithWebSocketServer("ca773208669c41d8b2fdd366323c74d6.s1.eu.hivemq.cloud:8884/mqtt")
                .WithCredentials("Raz0911", "ROMR031109h")
                .WithTls()
                .WithCleanSession(false)
                .Build();

            _client.UseDisconnectedHandler(async e =>
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                try { await _client.ConnectAsync(options); } catch { }
            });

            _client.UseConnectedHandler(async e =>
            {
                await _client.SubscribeAsync("Privada/AlertaGeneral");
            });

            _client.UseApplicationMessageReceivedHandler(e =>
            {
                if (e.ApplicationMessage.Topic == "Privada/AlertaGeneral")
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        _continuarVibrando = true;
                        _ = CicloDeVibracion();

                        await Application.Current.MainPage.DisplayAlert("🚨 ALERTA", "¡Alarma activada en la privada!", "OK");

                        _continuarVibrando = false;
                        Vibration.Default.Cancel();
                    });
                }
            });

            try { await _client.ConnectAsync(options); } catch { }
        }

        private async Task CicloDeVibracion()
        {
            while (_continuarVibrando)
            {
                Vibration.Default.Vibrate(TimeSpan.FromSeconds(1));
                await Task.Delay(1100);
            }
        }

        // Métodos de control (Puertas/Alarma)
        private async Task PublicarComandoAsync(string topico, int id, bool activar)
        {
            if (_client == null || !_client.IsConnected) await ConnectAsync();
            var payload = new { ID = id, Estatus = activar ? 1 : 0, Usuario = DeviceInfo.Name };
            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topico)
                .WithPayload(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(payload)))
                .Build();
            await _client.PublishAsync(message);
        }

        public async Task AbrirPeatonalAsync(bool activar) => await PublicarComandoAsync("Privada/AbrirPuerta", 0, activar);
        public async Task AbrirPortonAsync(bool activar) => await PublicarComandoAsync("Privada/AbrirPorton", 1, activar);
        public async Task AlarmaVecinalAsync(bool activar) => await PublicarComandoAsync("Privada/AlarmaVecinal", 2, activar);
    }
}