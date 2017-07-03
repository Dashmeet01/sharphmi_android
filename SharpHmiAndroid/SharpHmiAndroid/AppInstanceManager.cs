﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;
using HmiApiLib.Interfaces;
using HmiApiLib.Proxy;
using HmiApiLib;
using System.Collections.Generic;
using HmiApiLib.Manager;
using HmiApiLib.Base;
using Android.Graphics;
using Java.IO;
using System.IO;

namespace SharpHmiAndroid
{
	public class AppInstanceManager : ProxyHelper, IConnectionListener, IDispatchingHelper<LogMessage>
	{
		private static volatile AppInstanceManager instance;
		private static object syncRoot = new Object();
		public MessageAdapter _msgAdapter = null;
		public static Boolean bRecycled = false;
		public Boolean isConnected = false;

		public static Boolean appResumed = false;

		private AppSetting appSetting = null;
		public static List<AppItem> appList = new List<AppItem>();
		AppUiCallback appUiCallback;
        public static Dictionary<int, List<RpcRequest>> menuOptionListUi = new Dictionary<int, List<RpcRequest>>();
		public static Dictionary<int, List<Dictionary<string, Bitmap>>> appIdPutfileList = new Dictionary<int, List<Dictionary<string, Bitmap>>>();
		public static Dictionary<int, string> appIdPolicyIdDictionary = new Dictionary<int, string>();

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

		internal void setAppUiCallback(AppUiCallback callback)
		{
			appUiCallback = callback;
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
		public void setMsgAdapter(MessageAdapter messageAdapter)
		{
			this._msgAdapter = messageAdapter;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public MessageAdapter getMsgAdapter()
		{
			return this._msgAdapter;
		}

		public void setupConnection(String ipAddr, int portNum)
		{
			initConnectionManager(ipAddr, portNum, this, this, this);
		}

		//UI interface callbacks
		public override void onUiSetAppIconRequest(HmiApiLib.Controllers.UI.IncomingRequests.SetAppIcon msg)
		{
			if (null != appUiCallback)
			{
				int appId = -1;

				if (appIdPutfileList.ContainsKey(msg.getAppId()))
				{
					appId = msg.getAppId();
				}
				else
				{
					appId = getCorrectAppId(msg.getAppId());
				}

				if (appId != -1)
				{
					for (int i = 0; i < appIdPutfileList[appId].Count; i++)
					{
						if (appIdPutfileList[appId][i].ContainsKey(msg.getAppIcon().getValue()))
						{
							for (int j = 0; j < appList.Count; j++)
							{
								if ((appList[j].getAppID() == appId) || (appList[j].getAppID() == msg.getAppId()))
								{ 
									appList[j].setAppIcon(appIdPutfileList[appId][i][msg.getAppIcon().getValue()]);
									break;
								}
							}

							appUiCallback.setDownloadedAppIcon();
							break;
						}
					}
				}
			}

			base.onUiSetAppIconRequest(msg);
		}

		public int getCorrectAppId(int? matchValue)
		{
			int appId = -1;

			if (matchValue == null) return appId;

			if (appIdPolicyIdDictionary.ContainsKey(matchValue.Value))
			{
				appId = int.Parse(appIdPolicyIdDictionary[matchValue.Value]);
			}

			if (appIdPolicyIdDictionary.ContainsValue(matchValue.Value.ToString()))
			{
				appId = appIdPolicyIdDictionary.FirstOrDefault(x => x.Value == matchValue.Value.ToString()).Key;
			}

			return appId;
		}

		public override void onUiShowRequest(HmiApiLib.Controllers.UI.IncomingRequests.Show msg)
		{
			base.onUiShowRequest(msg);
		}

		public override void onUiAddCommandRequest(HmiApiLib.Controllers.UI.IncomingRequests.AddCommand msg)
		{
			base.onUiAddCommandRequest(msg);
            List<RpcRequest> data;
            if(menuOptionListUi.ContainsKey(msg.getAppId()))
            {
                data = menuOptionListUi[msg.getAppId()];
                data.Add(msg);
                menuOptionListUi.Remove(msg.getAppId());
            }
            else
            {
                data = new List<RpcRequest>();
                data.Add(msg);
            }
            menuOptionListUi.Add(msg.getAppId(), data);
            appUiCallback.refreshOptionsMenu();
		}

        public override void onUiAddSubMenuRequest(HmiApiLib.Controllers.UI.IncomingRequests.AddSubMenu msg)
        {
            base.onUiAddSubMenuRequest(msg);
			List<RpcRequest> data;
			if (menuOptionListUi.ContainsKey(msg.getAppId()))
			{
				data = menuOptionListUi[msg.getAppId()];
				data.Add(msg);
				menuOptionListUi.Remove(msg.getAppId());
			}
			else
			{
				data = new List<RpcRequest>();
				data.Add(msg);
			}
			menuOptionListUi.Add(msg.getAppId(), data);
			appUiCallback.refreshOptionsMenu();
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
            appList.Add(new AppItem(msg.getApplication().appName, msg.getApplication().appID));
			appIdPolicyIdDictionary.Add(msg.getApplication().getAppID(), msg.getApplication().getPolicyAppID());
			if (null != appUiCallback)
				appUiCallback.onBcAppRegisteredNotificationCallback(true);
		}

		public override void onBcAppUnRegisteredNotification(HmiApiLib.Controllers.BasicCommunication.IncomingNotifications.OnAppUnregistered msg)
		{
			base.onBcAppUnRegisteredNotification(msg);
            int appID = msg.getAppId();
            for (int i = 0; i < appList.Count; i++)
            {
                if (appList[i].getAppID() == appID)
				{
                    appList.RemoveAt(i);
					appIdPolicyIdDictionary.Remove(appID);
                    i--;
				}
            }

			if (appIdPutfileList.ContainsKey(appID))
			{
				appIdPutfileList[appID].Clear();
				appIdPutfileList.Remove(appID);
			}

			if (null != appUiCallback)
				appUiCallback.onBcAppRegisteredNotificationCallback(false);
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

		//Bc interface callbacks
		public override void onBcPutfileNotification(HmiApiLib.Controllers.BasicCommunication.IncomingNotifications.OnPutFile msg)
		{
			Dictionary<string, Bitmap> tmpMapping = new Dictionary<string, Bitmap>();
			string storedFileName = HttpUtility.getStoredFileName(msg.getSyncFileName());
			int appId = HttpUtility.getAppId(storedFileName);

			Stream inputStream = HttpUtility.downloadImage(storedFileName);
			Bitmap image = null;

			try
			{
				image = BitmapFactory.DecodeStream(inputStream);
			}
			catch (Exception ex)
			{				
			}

			if (image != null)
			{
				if (appIdPutfileList.ContainsKey(appId))
				{
					bool elementFound = false;
					for (int i = 0; i < appIdPutfileList[appId].Count; i++)
					{
						if (appIdPutfileList[appId][i].ContainsKey(msg.getSyncFileName()))
						{
							tmpMapping.Add(msg.getSyncFileName(), image);
							appIdPutfileList[appId][i] = tmpMapping;

							elementFound = true;
							break;
						}
					}

					if (!elementFound)
					{
						tmpMapping.Add(msg.getSyncFileName(), image);
						appIdPutfileList[appId].Add(tmpMapping);
					}
				}
				else
				{
					List<Dictionary<string, Bitmap>> tmpPutFileList = new List<Dictionary<string, Bitmap>>();
					tmpMapping.Add(msg.getSyncFileName(), image);
					tmpPutFileList.Add(tmpMapping);
					appIdPutfileList.Add(appId, tmpPutFileList);
				}

			}

			base.onBcPutfileNotification(msg);
		}

		public void onOpen()
		{
			// Handle logic for Callback triggered when Socket is Opened.
			//Console.WriteLine("Debug: onOpen()");
			isConnected = true;
		}

		public void onClose()
		{
			// Handle logic for Callback triggered when Socket is Opened.
			//Console.WriteLine("Debug: onClose()");
			isConnected = false;
		}

		public void onError()
		{
			// Handle logic for Callback triggered when Socket is Opened.
			//Console.WriteLine("Debug: onError()");
			isConnected = false;
		}

		private void addMessageToUI(LogMessage message)
		{
			if (_msgAdapter == null) return;

			_msgAdapter.addMessage(message);
		}

		public void dispatch(LogMessage message)
		{
			addMessageToUI(message);
		}

		public void handleDispatchingError(string info, Exception ex)
		{
			LogMessage logMessage = new StringLogMessage(info);
            addMessageToUI(logMessage);
		}

		public void handleQueueingError(string info, Exception ex)
		{
			LogMessage logMessage = new StringLogMessage(info);
			addMessageToUI(logMessage);
		}
	}
}
