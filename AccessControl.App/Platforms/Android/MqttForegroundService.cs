using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using AccessControl.Services;
using Android.Content.PM; // Para ForegroundService.Type

namespace AccessControl.Platforms.Android
{
    // Cambiamos el atributo para que sea más compatible
    [Service(Enabled = true, Exported = false, ForegroundServiceType = ForegroundService.TypeDataSync)]
    public class MqttForegroundService : Service
    {
        private MqttService _mqttService;

        public override IBinder OnBind(Intent intent) => null;

        // CORRECCIÓN 1: La firma exacta que espera Android en MAUI
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            string channelId = "mqtt_service_channel";
            var notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(channelId, "Servicio de Alarma", NotificationImportance.High);
                notificationManager.CreateNotificationChannel(channel);
            }

            var notification = new NotificationCompat.Builder(this, channelId)
                .SetContentTitle("Protección Vecinal Activa")
                .SetContentText("Escuchando alertas...")
                .SetSmallIcon(global::Android.Resource.Drawable.IcDialogAlert)
                .SetOngoing(true)
                .Build();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
            {
                StartForeground(1001, notification, ForegroundService.TypeDataSync);
            }
            else
            {
                StartForeground(1001, notification);
            }

            Task.Run(async () =>
            {
                if (_mqttService == null)
                    _mqttService = new MqttService();

                await _mqttService.ConnectAsync();
            });

            return StartCommandResult.Sticky;
        }
    }
}