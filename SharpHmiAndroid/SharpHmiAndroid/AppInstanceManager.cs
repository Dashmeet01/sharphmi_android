using System;
using System.Runtime.CompilerServices;
using HmiApiLib.Interfaces;
using HmiApiLib.Proxy;

namespace SharpHmiAndroid
{
	public class AppInstanceManager : ProxyHelper, IConnectionListener
	{
		private static volatile AppInstanceManager instance;
		private static object syncRoot = new Object();
		public static Boolean bRecycled = false;

		public static Boolean appResumed = false;

		private AppSetting appSetting = null;

		public static AppInstanceManager Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new AppInstanceManager();
					}
				}
				else
				{
					bRecycled = true;
				}

				return instance;
			}
		}

		private AppInstanceManager()
		{

		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void setAppSetting(AppSetting appSetting)
		{
			this.appSetting = appSetting;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public AppSetting getAppSetting()
		{
			return this.appSetting;
		}

		public void setupConnection(String ipAddr, int portNum)
		{
			initConnectionManager(ipAddr, portNum, this, this);
		}

		//UI interface callbacks
		public override void onUiSetAppIconRequest(HmiApiLib.Controllers.UI.IncomingRequests.SetAppIcon msg)
		{
			base.onUiSetAppIconRequest(msg);
		}

		public override void onUiShowRequest(HmiApiLib.Controllers.UI.IncomingRequests.Show msg)
		{
			base.onUiShowRequest(msg);
		}

		public override void onUiAddCommandRequest(HmiApiLib.Controllers.UI.IncomingRequests.AddCommand msg)
		{
			base.onUiAddCommandRequest(msg);
		}

		public override void onUiAlertRequest(HmiApiLib.Controllers.UI.IncomingRequests.Alert msg)
		{
			base.onUiAlertRequest(msg);
		}

		public override void onUiPerformInteractionRequest(HmiApiLib.Controllers.UI.IncomingRequests.PerformInteraction msg)
		{
			base.onUiPerformInteractionRequest(msg);
		}

		public override void onUiGetLanguageRequest(HmiApiLib.Controllers.UI.IncomingRequests.GetLanguage msg)
		{
			base.onUiGetLanguageRequest(msg);
		}

		public override void onUiDeleteCommandRequest(HmiApiLib.Controllers.UI.IncomingRequests.DeleteCommand msg)
		{
			base.onUiDeleteCommandRequest(msg);
		}

		public override void onUiIsReadyRequest(HmiApiLib.Controllers.UI.IncomingRequests.IsReady msg)
		{
			base.onUiIsReadyRequest(msg);
		}

		//TTS interface callbacks
		public override void onTtsSpeakRequest(HmiApiLib.Controllers.TTS.IncomingRequests.Speak msg)
		{
			base.onTtsSpeakRequest(msg);
		}

		public override void onTtsStopSpeakingRequest(HmiApiLib.Controllers.TTS.IncomingRequests.StopSpeaking msg)
		{
			base.onTtsStopSpeakingRequest(msg);
		}

		public override void onTtsGetLanguageRequest(HmiApiLib.Controllers.TTS.IncomingRequests.GetLanguage msg)
		{
			base.onTtsGetLanguageRequest(msg);
		}

		public override void onTtsIsReadyRequest(HmiApiLib.Controllers.TTS.IncomingRequests.IsReady msg)
		{
			base.onTtsIsReadyRequest(msg);
		}

		//VR interface callbacks
		public override void onVrAddCommandRequest(HmiApiLib.Controllers.VR.IncomingRequests.AddCommand msg)
		{
			base.onVrAddCommandRequest(msg);
		}

		public override void onVrGetLanguageRequest(HmiApiLib.Controllers.VR.IncomingRequests.GetLanguage msg)
		{
			base.onVrGetLanguageRequest(msg);
		}

		public override void onVrDeleteCommandRequest(HmiApiLib.Controllers.VR.IncomingRequests.DeleteCommand msg)
		{
			base.onVrDeleteCommandRequest(msg);
		}

		public override void onVrIsReadyRequest(HmiApiLib.Controllers.VR.IncomingRequests.IsReady msg)
		{
			base.onVrIsReadyRequest(msg);
		}

		public override void onVrPerformInteractionRequest(HmiApiLib.Controllers.VR.IncomingRequests.PerformInteraction msg)
		{
			base.onVrPerformInteractionRequest(msg);
		}

		//Navigation interface callbacks
		public override void onNavIsReadyRequest(HmiApiLib.Controllers.Navigation.IncomingRequests.IsReady msg)
		{
			base.onNavIsReadyRequest(msg);
		}

		//VehicleInfo interface callbacks
		public override void onVehicleInfoIsReadyRequest(HmiApiLib.Controllers.VehicleInfo.IncomingRequests.IsReady msg)
		{
			base.onVehicleInfoIsReadyRequest(msg);
		}

		//Bc interface callbacks
		public override void onBcAppRegisteredNotification(HmiApiLib.Controllers.BasicCommunication.IncomingNotifications.OnAppRegistered msg)
		{
			base.onBcAppRegisteredNotification(msg);
		}

		public override void onBcAppUnRegisteredNotification(HmiApiLib.Controllers.BasicCommunication.IncomingNotifications.OnAppUnregistered msg)
		{
			base.onBcAppUnRegisteredNotification(msg);
		}

		public override void onBcMixingAudioSupportedRequest(HmiApiLib.Controllers.BasicCommunication.IncomingRequests.MixingAudioSupported msg)
		{
			base.onBcMixingAudioSupportedRequest(msg);
		}

		public override void onBcActivateAppRequest(HmiApiLib.Controllers.BasicCommunication.IncomingRequests.ActivateApp msg)
		{
			base.onBcActivateAppRequest(msg);
		}

		//Buttons interface callbacks 
		public override void onButtonsGetCapabilitiesRequest(HmiApiLib.Controllers.Buttons.IncomingRequests.GetCapabilities msg)
		{
			base.onButtonsGetCapabilitiesRequest(msg);
		}

		public void onOpen()
		{
			// Handle logic for Callback triggered when Socket is Opened.
			Console.WriteLine("Debug: onOpen()");
		}

		public void onClose()
		{
			// Handle logic for Callback triggered when Socket is Opened.
			Console.WriteLine("Debug: onClose()");
		}

		public void onError()
		{
			// Handle logic for Callback triggered when Socket is Opened.
			Console.WriteLine("Debug: onError()");
		}
	}
}
