using System;
using Android.Graphics;
using HmiApiLib.Controllers.Buttons.IncomingNotifications;
using HmiApiLib.Controllers.TTS.IncomingRequests;
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
        void OnButtonSubscriptionNotificationCallback(OnButtonSubscription msg);
        void onTtsSpeakRequestCallback(Speak msg);
        void onUiSliderRequestCallback(Slider msg);
    }
}
