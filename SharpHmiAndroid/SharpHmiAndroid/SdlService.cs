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
			proxyListener = AppInstanceManager.getInstance().getProxyListener();

			if (proxyListener != null)
			{
				if (!proxyListener.doesServiceMatch(this))
				{
					proxyListener.setService(this);
				}
				_msgAdapter = proxyListener.getMsgAdapter();
				appSetting = proxyListener.getAppSetting();

				if (appSetting != null)
				{
					isVerboseEnabled = appSetting.getVerboseLoggingEnabled();
				}
				setupVideoStreamWriter();

			}
			else
			{
				return StartCommandResult.Sticky;
			}

			instance = this;

			setupService(false);

			if (_msgAdapter != null)
			{
				logMessage = new StringLogMessage("onStartCommand");
				_msgAdapter.logMessage(logMessage, Log.ERROR, true);
			}

			if (intent != null)
			{
				startProxy();
			}

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
