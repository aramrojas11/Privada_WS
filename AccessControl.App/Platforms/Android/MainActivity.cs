using Android.App;
using Android.Content; // <--- ESTE ES EL QUE FALTA PARA EL INTENT
using Android.Content.PM;
using Android.OS;

namespace AccessControl;

[Activity(Theme = "@style/Maui.MainTheme.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Intentamos arrancar el servicio de forma segura
        try
        {
            // Usamos la ruta completa para evitar confusiones de namespace
            var intent = new Intent(this, typeof(Platforms.Android.MqttForegroundService));

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                StartForegroundService(intent);
            }
            else
            {
                StartService(intent);
            }
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al iniciar el servicio: {ex.Message}");
        }
    }
}