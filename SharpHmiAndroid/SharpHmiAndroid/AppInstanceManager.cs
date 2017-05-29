using System;
using System.Runtime.CompilerServices;
using HmiApiLib.Interfaces;
using HmiApiLib.Proxy;
using System.Collections.Generic;
using HmiApiLib;
using Android.App;

namespace SharpHmiAndroid
{
	public class AppInstanceManager : ProxyHelper, IConnectionListener, IDispatchingHelper<LogMessage>
	{
		private static volatile AppInstanceManager instance;
		private static object syncRoot = new Object();
		private List<LogMessage> _logMessages = new List<LogMessage>();
		public MessageAdapter _msgAdapter = null;
		public static Boolean bRecycled = false;
		public Boolean isConnected = false;
		private Activity curActivity = null;

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

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void setActivity(Activity activity)
		{
			this.curActivity = activity;

			curActivity.RunOnUiThread(() => initializeMsgAdapter());
		}

		public void initializeMsgAdapter()
		{
			if (_msgAdapter == null)
			{
				_msgAdapter = new MessageAdapter(this.curActivity, Resource.Layout.row, _logMessages);
			}

		}

		public void setupConnection(String ipAddr, int portNum)
		{
			initConnectionManager(ipAddr, portNum, this, this, this);
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
			isConnected = true;
		}

		public void onClose()
		{
			// Handle logic for Callback triggered when Socket is Opened.
			Console.WriteLine("Debug: onClose()");
			isConnected = false;
		}

		public void onError()
		{
			// Handle logic for Callback triggered when Socket is Opened.
			Console.WriteLine("Debug: onError()");
			isConnected = false;
		}

		private void addMessageToUI(LogMessage message)
		{
			if (curActivity == null) return;

			curActivity.RunOnUiThread(() => _msgAdapter.addMessage(message));
		}

		public void dispatch(LogMessage message)
		{
			addMessageToUI(message);
		}

		public void handleDispatchingError(string info, Exception ex)
		{
			LogMessage logMessage = new LogMessage(info);
            addMessageToUI(logMessage);
		}

		public void handleQueueingError(string info, Exception ex)
		{
			LogMessage logMessage = new LogMessage(info);
			addMessageToUI(logMessage);
		}
	}
}
