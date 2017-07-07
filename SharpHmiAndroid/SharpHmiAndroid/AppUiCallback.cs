using System;
using Android.Graphics;
using HmiApiLib.Controllers.UI.IncomingRequests;

namespace SharpHmiAndroid
{
	public interface AppUiCallback
	{
        void onBcAppRegisteredNotificationCallback(Boolean isNewAppRegistered);
        void refreshOptionsMenu();
		void setDownloadedAppIcon();
        void onUiShowRequestCallback(Show msg);
        void onUiAlertRequestCallback(Alert msg);
        void onUiScrollableMessageRequestCallback(ScrollableMessage msg);
        void onUiPerformInteractionRequestCallback(PerformInteraction msg);
        void onUiSetMediaClockTimerRequestCallback(SetMediaClockTimer msg);
	}
}
