using System;
using Android.Graphics;

namespace SharpHmiAndroid
{
	public interface AppUiCallback
	{
        void onBcAppRegisteredNotificationCallback(Boolean isNewAppRegistered);
        void refreshOptionsMenu();
		void setDownloadedAppIcon();
	}
}
