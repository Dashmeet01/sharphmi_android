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
		AppSetting appSetting;

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

			appSetting = theInstance.getAppSetting();

			if (!theInstance.isConnected)
			{
				if (appSetting != null)
				{
					new System.Threading.Thread(new System.Threading.ThreadStart(() =>
					{
						theInstance.setupConnection(appSetting.getIPAddress(), int.Parse(appSetting.getTcpPort()));
					})).Start();
				}
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
				.SetContentText("SharpHmi")
				.SetSmallIcon(Resource.Drawable.sharp)
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
