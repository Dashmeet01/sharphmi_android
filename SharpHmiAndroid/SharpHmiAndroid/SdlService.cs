using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;

namespace SharpHmiAndroid
{
	[Service]
	public class SdlService : Service
	{
		// variable to contain the current state of the service
		public static SdlService instance = null;
		NotificationManager mNotificationManager;

		public override void OnCreate()
		{
			base.OnCreate();
			instance = this;
			mNotificationManager = (NotificationManager)GetSystemService(NotificationService);
		}

		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			instance = this;

			AppInstanceManager theInstance = AppInstanceManager.Instance;

			if (!theInstance.isConnected)
			{
				theInstance.setupConnection("192.168.1.213", 8087);				
			}

            setupService(false);
			return StartCommandResult.Sticky;		}

		public void setupService(Boolean updateForeground)
		{
			Intent intent = null;

			try
			{
				intent = new Intent(this, typeof(MainActivity));
				intent.AddFlags(ActivityFlags.ClearTop);
			}
			catch (Exception ex)
			{

			}

			if (null != intent)
			{
				PendingIntent pi = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.UpdateCurrent);
				var builder = new NotificationCompat.Builder(this)
				.SetContentTitle("SharpHmi")
				.SetContentText("SharpHmi Android")
				.SetSmallIcon(Resource.Drawable.ic_launcher)
				.SetContentIntent(pi)
				.SetOngoing(true);

				Notification note = builder.Build();

				if (updateForeground)
				{
					mNotificationManager.Notify((int)NotificationFlags.ForegroundService, note);
				}
				else
				{
                    StartForeground((int)NotificationFlags.ForegroundService, note);
				}

			}
		}

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
			instance = null;
		}
	}
}
