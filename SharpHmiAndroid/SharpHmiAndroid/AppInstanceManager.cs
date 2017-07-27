using System;
using System.Linq;
using System.Runtime.CompilerServices;
using HmiApiLib.Interfaces;
using HmiApiLib.Proxy;
using HmiApiLib;
using System.Collections.Generic;
using HmiApiLib.Base;
using Android.Graphics;
using Java.IO;
using System.IO;
using HmiApiLib.Controllers.Buttons.IncomingNotifications;
using HmiApiLib.Controllers.BasicCommunication.IncomingNotifications;
using HmiApiLib.Controllers.BasicCommunication.IncomingRequests;
using HmiApiLib.Controllers.Buttons.OutGoingNotifications;
using HmiApiLib.Controllers.Navigation.IncomingRequests;
using HmiApiLib.Controllers.TTS.IncomingRequests;
using HmiApiLib.Controllers.UI.IncomingRequests;
using HmiApiLib.Controllers.VehicleInfo.IncomingRequests;
using HmiApiLib.Controllers.UI.OutGoingNotifications;
using HmiApiLib.Builder;
using HmiApiLib.Common.Enums;
using HmiApiLib.Common.Structs;
using Android.App;

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
		public static Dictionary<int, List<string>> appIdPutfileList = new Dictionary<int, List<string>>();
		public static Dictionary<int, string> appIdPolicyIdDictionary = new Dictionary<int, string>();
        public static Dictionary<int, List<int?>> commandIdList = new Dictionary<int, List<int?>>();

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

		//UI interface callbacks
		public override void onUiSetAppIconRequest(SetAppIcon msg)
		{
			if (null != appUiCallback)
			{
				int corrId = msg.getId();
				int appId = -1;

				if (appIdPutfileList.ContainsKey((int)msg.getAppId()))
				{
					appId = (int)msg.getAppId();
				}
				else
				{
					appId = getCorrectAppId(msg.getAppId());
				}

				if (appId != -1)
				{
					for (int i = 0; i < appIdPutfileList[appId].Count; i++)
					{
						if (appIdPutfileList[appId].Contains(msg.getAppIcon().getValue()))
						{
							for (int j = 0; j < appList.Count; j++)
							{
								if ((appList[j].getAppID() == appId) || (appList[j].getAppID() == msg.getAppId()))
								{
									Bitmap image = null;

									try
									{
										image = BitmapFactory.DecodeStream(getPutfile(msg.getAppIcon().getValue()));
										appList[j].setAppIcon(image);
										appUiCallback.setDownloadedAppIcon();
										sendRpc(BuildRpc.buildUiSetAppIconResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
									}
									catch (Exception ex)
									{
										sendRpc(BuildRpc.buildUiSetAppIconResponse(corrId, HmiApiLib.Common.Enums.Result.INVALID_DATA));
									}
									return;
								}

							}
							break;
						}
					}
				}
				else
				{
					sendRpc(BuildRpc.buildUiSetAppIconResponse(corrId, HmiApiLib.Common.Enums.Result.INVALID_ID));
					return;
				}

				sendRpc(BuildRpc.buildUiSetAppIconResponse(corrId, HmiApiLib.Common.Enums.Result.DATA_NOT_AVAILABLE));
			}

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

		public override void onUiShowRequest(Show msg)
		{
            int appID = (int)msg.getAppId();
			int corrId = msg.getId();
            HmiApiLib.Controllers.UI.OutgoingResponses.Show tmpObj = new HmiApiLib.Controllers.UI.OutgoingResponses.Show();
            tmpObj = (HmiApiLib.Controllers.UI.OutgoingResponses.Show)AppUtils.getSavedPreferenceValueForRpc<HmiApiLib.Controllers.UI.OutgoingResponses.Show>(((MainActivity)appUiCallback), tmpObj.getMethod(), appID);
            if (null == tmpObj)
            {
				sendRpc(BuildRpc.buildUiShowResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
				appUiCallback.onUiShowRequestCallback(msg);
            }
            else
            {
                tmpObj.setId(corrId);
                sendRpc(tmpObj);
            }
		}

		public override void onUiAddCommandRequest(AddCommand msg)
		{
            int appID = (int)msg.getAppId();
            int corrId = msg.getId();

            HmiApiLib.Controllers.UI.OutgoingResponses.AddCommand tmpObj = new HmiApiLib.Controllers.UI.OutgoingResponses.AddCommand();
            tmpObj = (HmiApiLib.Controllers.UI.OutgoingResponses.AddCommand)AppUtils.getSavedPreferenceValueForRpc<HmiApiLib.Controllers.UI.OutgoingResponses.AddCommand>(((MainActivity)appUiCallback), tmpObj.getMethod(), appID);
            if (null == tmpObj)
            {
				List<RpcRequest> data;
				List<int?> cmdIdList;
				if (menuOptionListUi.ContainsKey(appID))
				{
					data = menuOptionListUi[appID];
					data.Add(msg);
					menuOptionListUi.Remove(appID);
				}
				else
				{
					data = new List<RpcRequest>();
					data.Add(msg);
				}
				menuOptionListUi.Add(appID, data);

				if (commandIdList.ContainsKey(appID))
				{
					cmdIdList = commandIdList[appID];
					cmdIdList.Add(msg.getCmdId());
					commandIdList.Remove(appID);
				}
				else
				{
					cmdIdList = new List<int?>();
					cmdIdList.Add(msg.getCmdId());
				}
				commandIdList.Add(appID, cmdIdList);

				appUiCallback.refreshOptionsMenu();
				sendRpc(BuildRpc.buildUiAddCommandResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
            }
            else
            {
				tmpObj.setId(corrId);
				sendRpc(tmpObj);
            }
		}

		public override void onUiAlertRequest(Alert msg)
		{
			int corrId = msg.getId();
			int? appId = msg.getAppId();

			sendRpc(BuildRpc.buildUiOnSystemContext(SystemContext.ALERT, appId));

            HmiApiLib.Controllers.UI.OutgoingResponses.Alert tmpObj = new HmiApiLib.Controllers.UI.OutgoingResponses.Alert();
            tmpObj = (HmiApiLib.Controllers.UI.OutgoingResponses.Alert)AppUtils.getSavedPreferenceValueForRpc<HmiApiLib.Controllers.UI.OutgoingResponses.Alert>(((MainActivity)appUiCallback), tmpObj.getMethod(), (int)appId);
            if (null == tmpObj)
            {
                appUiCallback.onUiAlertRequestCallback(msg);
                sendRpc(BuildRpc.buildUiAlertResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));
            }
            else
            {
                tmpObj.setId(corrId);
                sendRpc(tmpObj);
            }
            sendRpc(BuildRpc.buildUiOnSystemContext(SystemContext.MAIN, appId));
		}

		public override void onUiPerformInteractionRequest(PerformInteraction msg)
		{
            appUiCallback.onUiPerformInteractionRequestCallback(msg);
		}

		public override void onUiGetLanguageRequest(HmiApiLib.Controllers.UI.IncomingRequests.GetLanguage msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiGetLanguageResponse(corrId, Language.EN_US, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onUiDeleteCommandRequest(DeleteCommand msg)
		{
			int corrId = msg.getId();
			List<RpcRequest> data = new List<RpcRequest>();
			if (menuOptionListUi.ContainsKey((int)msg.getAppId()))
			{
				data = menuOptionListUi[(int)msg.getAppId()];
				foreach (RpcRequest req in data)
				{
					if (req is AddCommand)
					{
						if (((AddCommand)req).getCmdId() == msg.getCmdId())
						{
							data.Remove(req);
							break;
						}
					}
				}
				menuOptionListUi.Remove((int)msg.getAppId());
			}
			menuOptionListUi.Add((int)msg.getAppId(), data);
			appUiCallback.refreshOptionsMenu();

			sendRpc(BuildRpc.buildUiDeleteCommandResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
		}

		public override void onUiIsReadyRequest(HmiApiLib.Controllers.UI.IncomingRequests.IsReady msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildIsReadyResponse(corrId, HmiApiLib.Types.InterfaceType.UI, true, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		//TTS interface callbacks
		public override void onTtsSpeakRequest(Speak msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildTtsSpeakResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
            appUiCallback.onTtsSpeakRequestCallback(msg);

		}

		public override void onTtsStopSpeakingRequest(StopSpeaking msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildTtsStopSpeakingResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onTtsGetLanguageRequest(HmiApiLib.Controllers.TTS.IncomingRequests.GetLanguage msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildTtsGetLanguageResponse(corrId, Language.EN_US, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onTtsIsReadyRequest(HmiApiLib.Controllers.TTS.IncomingRequests.IsReady msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildIsReadyResponse(corrId, HmiApiLib.Types.InterfaceType.TTS, true, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		//VR interface callbacks
		public override void onVrAddCommandRequest(HmiApiLib.Controllers.VR.IncomingRequests.AddCommand msg)
		{
			int corrId = msg.getId();
			int? cmdId = msg.getCmdId();
			int? grammerId = msg.getGrammarId();

			if ((vrGrammerAddCommandDictionary != null) && (grammerId != null)
				&& (cmdId != null) && (grammerId != -1))
			{
				vrGrammerAddCommandDictionary.Add((int)grammerId, (int)cmdId);
			}

			sendRpc(BuildRpc.buildVrAddCommandResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onVrGetLanguageRequest(HmiApiLib.Controllers.VR.IncomingRequests.GetLanguage msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVrGetLanguageResponse(corrId, Language.EN_US, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onVrDeleteCommandRequest(HmiApiLib.Controllers.VR.IncomingRequests.DeleteCommand msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVrDeleteCommandResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onVrIsReadyRequest(HmiApiLib.Controllers.VR.IncomingRequests.IsReady msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildIsReadyResponse(corrId, HmiApiLib.Types.InterfaceType.VR, true, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onVrPerformInteractionRequest(HmiApiLib.Controllers.VR.IncomingRequests.PerformInteraction msg)
		{
			int corrId = msg.getId();
			int returnVal = getVrAddCommandId(msg.getGrammarID());

			if (returnVal != -1)
			{
				sendRpc(BuildRpc.buildVrPerformInteractionResponse(corrId, returnVal, HmiApiLib.Common.Enums.Result.SUCCESS));
			}
			else
			{
				sendRpc(BuildRpc.buildVrPerformInteractionResponse(corrId, null, HmiApiLib.Common.Enums.Result.GENERIC_ERROR));
			}
		}

		//Navigation interface callbacks
		public override void onNavIsReadyRequest(HmiApiLib.Controllers.Navigation.IncomingRequests.IsReady msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildIsReadyResponse(corrId, HmiApiLib.Types.InterfaceType.Navigation, true, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		//VehicleInfo interface callbacks
		public override void onVehicleInfoIsReadyRequest(HmiApiLib.Controllers.VehicleInfo.IncomingRequests.IsReady msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildIsReadyResponse(corrId, HmiApiLib.Types.InterfaceType.VehicleInfo, true, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		//Bc interface callbacks
		public override void onBcAppRegisteredNotification(OnAppRegistered msg)
		{
			appList.Add(new AppItem(msg.getApplication().appName, msg.getApplication().appID));
			appIdPolicyIdDictionary.Add(msg.getApplication().getAppID(), msg.getApplication().getPolicyAppID());
			if (null != appUiCallback)
				appUiCallback.onBcAppRegisteredNotificationCallback(true);
		}

		public override void onBcAppUnRegisteredNotification(OnAppUnregistered msg)
		{
			int appID = (int)msg.getAppId();
			for (int i = 0; i < appList.Count; i++)
			{
				if ((appList[i].getAppID() == appID) || (appList[i].getAppID() == getCorrectAppId(appID)))
				{
					int tmpAppId = appID;

					if (appList[i].getAppID() == getCorrectAppId(tmpAppId))
					{
						tmpAppId = getCorrectAppId(tmpAppId);
					}

					appList.RemoveAt(i);
					i--;
				}
			}

			if (appIdPutfileList.ContainsKey(appID) || appIdPutfileList.ContainsKey(getCorrectAppId(appID)))
			{
				int tmpAppId = appID;
				if (appIdPutfileList.ContainsKey(getCorrectAppId(tmpAppId)))
				{
					tmpAppId = getCorrectAppId(tmpAppId);
				}

				deletePutfileDirectory(appIdPutfileList[tmpAppId][0]);
				appIdPutfileList[tmpAppId].Clear();
				appIdPutfileList.Remove(tmpAppId);
				appIdPolicyIdDictionary.Remove(tmpAppId);
			}

			if (null != appUiCallback)
				appUiCallback.onBcAppRegisteredNotificationCallback(false);
		}

		public override void onBcMixingAudioSupportedRequest(MixingAudioSupported msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildBasicCommunicationMixingAudioSupportedResponse(corrId, null, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void OnButtonSubscriptionNotification(OnButtonSubscription msg)
		{
            appUiCallback.OnButtonSubscriptionNotificationCallback(msg);
		}

		public override void onUiAddSubMenuRequest(AddSubMenu msg)
		{
			int corrId = msg.getId();
            int appID = (int)msg.getAppId();
            HmiApiLib.Controllers.UI.OutgoingResponses.AddSubMenu tmpObj = new HmiApiLib.Controllers.UI.OutgoingResponses.AddSubMenu();
            tmpObj = (HmiApiLib.Controllers.UI.OutgoingResponses.AddSubMenu)AppUtils.getSavedPreferenceValueForRpc<HmiApiLib.Controllers.UI.OutgoingResponses.AddSubMenu>(((MainActivity)appUiCallback), tmpObj.getMethod(), appID);
            if (null == tmpObj)
            {
				List<RpcRequest> data;
				if (menuOptionListUi.ContainsKey((int)msg.getAppId()))
				{
					data = menuOptionListUi[(int)msg.getAppId()];
					data.Add(msg);
					menuOptionListUi.Remove((int)msg.getAppId());
				}
				else
				{
					data = new List<RpcRequest>();
					data.Add(msg);
				}
				menuOptionListUi.Add((int)msg.getAppId(), data);
				appUiCallback.refreshOptionsMenu();

				sendRpc(BuildRpc.buildUiAddSubMenuResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
            }
            else
            {
                tmpObj.setId(corrId);
                sendRpc(tmpObj);
            }
		}

		public override void onUiChangeRegistrationRequest(HmiApiLib.Controllers.UI.IncomingRequests.ChangeRegistration msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiChangeRegistrationResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onUiClosePopUpRequest(ClosePopUp msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiClosePopUpResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onUiDeleteSubMenuRequest(DeleteSubMenu msg)
		{
			int corrId = msg.getId();
			List<RpcRequest> data = new List<RpcRequest>();
			if (menuOptionListUi.ContainsKey((int)msg.getAppId()))
			{
				data = menuOptionListUi[(int)msg.getAppId()];
				foreach (RpcRequest req in data)
				{
					if (req is AddSubMenu)
					{
						if (((AddSubMenu)req).getMenuID() == msg.getMenuID())
						{
							data.Remove(req);
							break;
						}
					}
				}
				menuOptionListUi.Remove((int)msg.getAppId());
			}
			menuOptionListUi.Add((int)msg.getAppId(), data);
			appUiCallback.refreshOptionsMenu();

			sendRpc(BuildRpc.buildUiDeleteSubMenuResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
		}

		public override void onUiEndAudioPassThruRequest(EndAudioPassThru msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiEndAudioPassThruResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
		}

		public override void onUiGetCapabilitiesRequest(HmiApiLib.Controllers.UI.IncomingRequests.GetCapabilities msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiGetCapabilitiesResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null, null, HmiZoneCapabilities.BACK, null, null));
		}

		public override void onUiGetSupportedLanguagesRequest(HmiApiLib.Controllers.UI.IncomingRequests.GetSupportedLanguages msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiGetSupportedLanguagesResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));
		}

		public override void onUiPerformAudioPassThruRequest(PerformAudioPassThru msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiPerformAudioPassThruResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onUiScrollableMessageRequest(ScrollableMessage msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiScrollableMessageResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
            appUiCallback.onUiScrollableMessageRequestCallback(msg);
		}

		public override void onUiSetDisplayLayoutRequest(SetDisplayLayout msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiSetDisplayLayoutResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null, null, null, null));
		}

		public override void onUiSetGlobalPropertiesRequest(HmiApiLib.Controllers.UI.IncomingRequests.SetGlobalProperties msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiSetGlobalPropertiesResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onUiSetMediaClockTimerRequest(SetMediaClockTimer msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiSetMediaClockTimerResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
            appUiCallback.onUiSetMediaClockTimerRequestCallback(msg);
		}

		public override void onUiShowCustomFormRequest(ShowCustomForm msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildUiShowCustomFormResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));
		}

		public override void onUiSliderRequest(Slider msg)
		{
            appUiCallback.onUiSliderRequestCallback(msg);
		}

		public override void onTtsChangeRegistrationRequest(HmiApiLib.Controllers.TTS.IncomingRequests.ChangeRegistration msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildTTSChangeRegistrationResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onTtsGetCapabilitiesRequest(HmiApiLib.Controllers.TTS.IncomingRequests.GetCapabilities msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildTTSGetCapabilitiesResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null, null));

		}

		public override void onTtsGetSupportedLanguagesRequest(HmiApiLib.Controllers.TTS.IncomingRequests.GetSupportedLanguages msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildTTSGetSupportedLanguagesResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));

		}

		public override void onTtsSetGlobalPropertiesRequest(HmiApiLib.Controllers.TTS.IncomingRequests.SetGlobalProperties msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildTTSSetGlobalPropertiesResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
		}

		public override void onVrChangeRegistrationRequest(HmiApiLib.Controllers.VR.IncomingRequests.ChangeRegistration msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVrChangeRegistrationResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
		}

		public override void onVrGetCapabilitiesRequest(HmiApiLib.Controllers.VR.IncomingRequests.GetCapabilities msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVrGetCapabilitiesResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));

		}

		public override void onVrGetSupportedLanguagesRequest(HmiApiLib.Controllers.VR.IncomingRequests.GetSupportedLanguages msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVrGetSupportedLanguagesResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));

		}

		public override void onNavAlertManeuverRequest(AlertManeuver msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavAlertManeuverResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
		}

		public override void onNavGetWayPointsRequest(GetWayPoints msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavGetWayPointsResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null, null));

		}

		public override void onNavSendLocationRequest(SendLocation msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavSendLocationResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onNavShowConstantTBTRequest(ShowConstantTBT msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavShowConstantTBTResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onNavStartAudioStreamRequest(StartAudioStream msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavStartAudioStreamResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onNavStartStreamRequest(StartStream msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavStartStreamResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onNavStopAudioStreamRequest(StopAudioStream msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavStopAudioStreamResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onNavStopStreamRequest(StopStream msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavStopStreamResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onNavSubscribeWayPointsRequest(SubscribeWayPoints msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavSubscribeWayPointsResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));
		}

		public override void onNavUnsubscribeWayPointsRequest(UnsubscribeWayPoints msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavUnsubscribeWayPointsResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onNavUpdateTurnListRequest(UpdateTurnList msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildNavUpdateTurnListResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onVehicleInfoDiagnosticMessageRequest(DiagnosticMessage msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVehicleInfoDiagnosticMessageResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));

		}

		public override void onVehicleInfoGetDTCsRequest(GetDTCs msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVehicleInfoGetDTCsResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null, null));

		}

		public override void onVehicleInfoGetVehicleDataRequest(GetVehicleData msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVehicleInfoGetVehicleDataResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onVehicleInfoGetVehicleTypeRequest(GetVehicleType msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVehicleInfoGetVehicleTypeResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onVehicleInfoReadDIDRequest(ReadDID msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVehicleInfoReadDIDResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));

		}

		public override void onVehicleInfoSubscribeVehicleDataRequest(SubscribeVehicleData msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVehicleInfoSubscribeVehicleDataResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onVehicleInfoUnsubscribeVehicleDataRequest(UnsubscribeVehicleData msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildVehicleInfoUnsubscribeVehicleDataResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onBcPutfileNotification(OnPutFile msg)
		{
			Dictionary<string, Bitmap> tmpMapping = new Dictionary<string, Bitmap>();
			string storedFileName = HttpUtility.getStoredFileName(msg.getSyncFileName());
			int appId = HttpUtility.getAppId(storedFileName);
			string appStorageDirectoryName = HttpUtility.getAppStorageDirectory(storedFileName);
			string fileName = HttpUtility.getFileName(storedFileName);

			Stream inputStream = HttpUtility.downloadFile(storedFileName);

			String state = Android.OS.Environment.ExternalStorageState;

			if (Android.OS.Environment.MediaMounted == state)
			{
				string appRootDirPath = Android.OS.Environment.ExternalStorageDirectory.Path
											+ "/Sharp Hmi Android/";
				Java.IO.File sharpHmiAndroidDir = new Java.IO.File(appRootDirPath);
				if (!sharpHmiAndroidDir.Exists())
				{
					sharpHmiAndroidDir.Mkdir();
				}

				Java.IO.File appStorageDir = new Java.IO.File(appRootDirPath + appStorageDirectoryName + "/");
				if (!appStorageDir.Exists())
				{
					appStorageDir.Mkdir();
				}

				Java.IO.File myFile = new Java.IO.File(appStorageDir, fileName);

				if (!myFile.Exists())
				{
					try
					{
						myFile.CreateNewFile();
						FileOutputStream fileOutStream = new FileOutputStream(myFile);

						byte[] buffer = new byte[1024];
						int len = 0;
						while ((len = inputStream.Read(buffer, 0, buffer.Length)) > 0)
						{
							fileOutStream.Write(buffer, 0, len);
						}
						fileOutStream.Close();
					}
					catch (Exception ex)
					{
					}
				}
			}

			if (appIdPutfileList.ContainsKey(appId))
			{
				if (!appIdPutfileList[appId].Contains(msg.getSyncFileName()))
				{
					appIdPutfileList[appId].Add(msg.getSyncFileName());
				}
			}
			else
			{
				List<string> tmpPutFileList = new List<string>();
				tmpPutFileList.Add(msg.getSyncFileName());
				appIdPutfileList.Add(appId, tmpPutFileList);
			}

		}

		public override void onBcAllowDeviceToConnectRequest(AllowDeviceToConnect msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildBasicCommunicationAllowDeviceToConnectResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null));

		}

		public override void onBcDialNumberRequest(DialNumber msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildBasicCommunicationDialNumberResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onBcGetSystemInfoRequest(GetSystemInfo msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildBasicCommunicationGetSystemInfoResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS, null, Language.EN_US, null));

		}

		public override void onBcPolicyUpdateRequest(PolicyUpdate msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildBasicCommunicationPolicyUpdateResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onBcSystemRequestRequest(SystemRequest msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildBasicCommunicationSystemRequestResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onBcUpdateAppListRequest(UpdateAppList msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildBasicCommunicationUpdateAppListResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void onBcUpdateDeviceListRequest(UpdateDeviceList msg)
		{
			int corrId = msg.getId();
			sendRpc(BuildRpc.buildBasicCommunicationUpdateDeviceListResponse(corrId, HmiApiLib.Common.Enums.Result.SUCCESS));

		}

		public override void OnUiCommandNotification(OnCommand msg)
		{
            sendRpc(msg);
		}

		public override void OnVrCommandNotification(HmiApiLib.Controllers.VR.OutGoingNotifications.OnCommand msg)
		{

		}

		public override void onButtonPressNotification(OnButtonPress msg)
		{

		}

		public override void onButtonEventNotification(OnButtonEvent msg)
		{

		}

		public override void onUiSystemContextNotification(OnSystemContext msg)
		{
		}

		public Stream getPutfile(string syncFileName)
		{
			Stream inputStreamObj = null;
			if (Android.OS.Environment.MediaMounted == Android.OS.Environment.ExternalStorageState)
			{
				string appRootDirPath = Android.OS.Environment.ExternalStorageDirectory.Path
											+ "/Sharp Hmi Android/";

				Java.IO.File sharpHmiAndroidDir = new Java.IO.File(appRootDirPath);
				if (sharpHmiAndroidDir.Exists())
				{
					string storedFileName = HttpUtility.getStoredFileName(syncFileName);
					string appStorageDirectoryName = HttpUtility.getAppStorageDirectory(storedFileName);

					Java.IO.File appStorageDir = new Java.IO.File(appRootDirPath + appStorageDirectoryName + "/");
					if (appStorageDir.Exists())
					{
						string fileName = HttpUtility.getFileName(storedFileName);

						Java.IO.File myFile = new Java.IO.File(appStorageDir, fileName);
						if (myFile.Exists())
						{
							StreamReader inputStream = new StreamReader(appRootDirPath + appStorageDirectoryName + "/" + fileName);
							inputStreamObj = inputStream.BaseStream;
						}
					}
				}
			}
			return inputStreamObj;
		}

		public void deletePutfileDirectory(string syncFileName)
		{
			if (Android.OS.Environment.MediaMounted == Android.OS.Environment.ExternalStorageState)
			{
				string appRootDirPath = Android.OS.Environment.ExternalStorageDirectory.Path
											+ "/Sharp Hmi Android/";

				Java.IO.File sharpHmiAndroidDir = new Java.IO.File(appRootDirPath);
				if (sharpHmiAndroidDir.Exists())
				{
					string storedFileName = HttpUtility.getStoredFileName(syncFileName);
					string appStorageDirectoryName = HttpUtility.getAppStorageDirectory(storedFileName);

					Java.IO.File appStorageDir = new Java.IO.File(appRootDirPath + appStorageDirectoryName + "/");
					if (appStorageDir.Exists() && appStorageDir.IsDirectory)
					{
						Java.IO.File[] filesInFolder = appStorageDir.ListFiles();

						bool filedeleted = false;
						for (int i = 0; i < filesInFolder.Length; i++)
						{
							Java.IO.File file = filesInFolder[i];
							filedeleted = file.Delete();
						}
						filedeleted = appStorageDir.Delete();
					}
				}
			}
		}
	}
}