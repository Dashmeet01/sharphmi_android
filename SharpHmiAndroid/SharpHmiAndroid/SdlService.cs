using System;
using Android.App;
using Android.Content;
using Android.OS;

namespace SharpHmiAndroid
{
	public class SdlService : Service
	{
		// variable to contain the current state of the service
		private static SdlService instance = null;

		public override void OnCreate()
		{
			base.OnCreate();
			instance = this;
		}


		public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
		{
			instance = this;
			return StartCommandResult.Sticky;		}

		public override IBinder OnBind(Intent intent)
		{
			return null;
		}

		public override void OnDestroy()
		{
			base.OnDestroy();
		}
	}
}
