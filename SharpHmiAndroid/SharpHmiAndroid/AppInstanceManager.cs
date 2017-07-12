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
			addMessageToUI(logMessage);		}

		//UI interface callbacks
		public override void onUiSetAppIconRequest(SetAppIcon msg)
		{
			if (null != appUiCallback)
			{
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
									}
									catch (Exception ex)
									{
									}
									break;
								}
							}
							break;
						}
					}
				}
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

		public override void onUiShowRequest(HmiApiLib.Controllers.UI.IncomingRequests.Show msg)
		{
			appUiCallback.onUiShowRequestCallback(msg);
		}

		public override void onUiAddCommandRequest(HmiApiLib.Controllers.UI.IncomingRequests.AddCommand msg)
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
		}

		public override void onUiAlertRequest(Alert msg)
		{
			appUiCallback.onUiAlertRequestCallback(msg);
		}

		public override void onUiPerformInteractionRequest(HmiApiLib.Controllers.UI.IncomingRequests.PerformInteraction msg)
		{
			appUiCallback.onUiPerformInteractionRequestCallback(msg);
		}

		public override void onUiGetLanguageRequest(HmiApiLib.Controllers.UI.IncomingRequests.GetLanguage msg)
		{

		}

		public override void onUiDeleteCommandRequest(HmiApiLib.Controllers.UI.IncomingRequests.DeleteCommand msg)
		{
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
		}

		public override void onUiIsReadyRequest(HmiApiLib.Controllers.UI.IncomingRequests.IsReady msg)
		{

		}

		//TTS interface callbacks
		public override void onTtsSpeakRequest(Speak msg)
		{
			appUiCallback.onTtsSpeakRequestCallback(msg);
		}

		public override void onTtsStopSpeakingRequest(StopSpeaking msg)
		{

		}

		public override void onTtsGetLanguageRequest(HmiApiLib.Controllers.TTS.IncomingRequests.GetLanguage msg)
		{

		}

		public override void onTtsIsReadyRequest(HmiApiLib.Controllers.TTS.IncomingRequests.IsReady msg)
		{

		}

		//VR interface callbacks
		public override void onVrAddCommandRequest(HmiApiLib.Controllers.VR.IncomingRequests.AddCommand msg)
		{

		}

		public override void onVrGetLanguageRequest(HmiApiLib.Controllers.VR.IncomingRequests.GetLanguage msg)
		{

		}

		public override void onVrDeleteCommandRequest(HmiApiLib.Controllers.VR.IncomingRequests.DeleteCommand msg)
		{

		}

		public override void onVrIsReadyRequest(HmiApiLib.Controllers.VR.IncomingRequests.IsReady msg)
		{

		}

		public override void onVrPerformInteractionRequest(HmiApiLib.Controllers.VR.IncomingRequests.PerformInteraction msg)
		{

		}

		//Navigation interface callbacks
		public override void onNavIsReadyRequest(HmiApiLib.Controllers.Navigation.IncomingRequests.IsReady msg)
		{

		}

		//VehicleInfo interface callbacks
		public override void onVehicleInfoIsReadyRequest(HmiApiLib.Controllers.VehicleInfo.IncomingRequests.IsReady msg)
		{

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

		}

		public override void OnButtonSubscriptionNotification(OnButtonSubscription msg)
		{
			appUiCallback.OnButtonSubscriptionNotificationCallback(msg);
		}

		public override void onUiAddSubMenuRequest(AddSubMenu msg)
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
		}

		public override void onUiChangeRegistrationRequest(HmiApiLib.Controllers.UI.IncomingRequests.ChangeRegistration msg)
		{

		}

		public override void onUiClosePopUpRequest(ClosePopUp msg)
		{

		}

		public override void onUiDeleteSubMenuRequest(DeleteSubMenu msg)
		{
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
		}

		public override void onUiEndAudioPassThruRequest(EndAudioPassThru msg)
		{

		}

		public override void onUiGetCapabilitiesRequest(HmiApiLib.Controllers.UI.IncomingRequests.GetCapabilities msg)
		{

		}

		public override void onUiGetSupportedLanguagesRequest(HmiApiLib.Controllers.UI.IncomingRequests.GetSupportedLanguages msg)
		{

		}

		public override void onUiPerformAudioPassThruRequest(PerformAudioPassThru msg)
		{

		}

		public override void onUiScrollableMessageRequest(ScrollableMessage msg)
		{
			appUiCallback.onUiScrollableMessageRequestCallback(msg);
		}

		public override void onUiSetDisplayLayoutRequest(SetDisplayLayout msg)
		{

		}

		public override void onUiSetGlobalPropertiesRequest(HmiApiLib.Controllers.UI.IncomingRequests.SetGlobalProperties msg)
		{

		}

		public override void onUiSetMediaClockTimerRequest(SetMediaClockTimer msg)
		{
			appUiCallback.onUiSetMediaClockTimerRequestCallback(msg);
		}

		public override void onUiShowCustomFormRequest(ShowCustomForm msg)
		{

		}

		public override void onUiSliderRequest(Slider msg)
		{
			appUiCallback.onUiSliderRequestCallback(msg);
		}

		public override void onTtsChangeRegistrationRequest(HmiApiLib.Controllers.TTS.IncomingRequests.ChangeRegistration msg)
		{

		}

		public override void onTtsGetCapabilitiesRequest(HmiApiLib.Controllers.TTS.IncomingRequests.GetCapabilities msg)
		{

		}

		public override void onTtsGetSupportedLanguagesRequest(HmiApiLib.Controllers.TTS.IncomingRequests.GetSupportedLanguages msg)
		{

		}

		public override void onTtsSetGlobalPropertiesRequest(HmiApiLib.Controllers.TTS.IncomingRequests.SetGlobalProperties msg)
		{

		}

		public override void onVrChangeRegistrationRequest(HmiApiLib.Controllers.VR.IncomingRequests.ChangeRegistration msg)
		{

		}

		public override void onVrGetCapabilitiesRequest(HmiApiLib.Controllers.VR.IncomingRequests.GetCapabilities msg)
		{

		}

		public override void onVrGetSupportedLanguagesRequest(HmiApiLib.Controllers.VR.IncomingRequests.GetSupportedLanguages msg)
		{

		}

		public override void onNavAlertManeuverRequest(AlertManeuver msg)
		{

		}

		public override void onNavGetWayPointsRequest(GetWayPoints msg)
		{

		}

		public override void onNavSendLocationRequest(SendLocation msg)
		{

		}

		public override void onNavShowConstantTBTRequest(ShowConstantTBT msg)
		{

		}

		public override void onNavStartAudioStreamRequest(StartAudioStream msg)
		{

		}

		public override void onNavStartStreamRequest(StartStream msg)
		{

		}

		public override void onNavStopAudioStreamRequest(StopAudioStream msg)
		{

		}

		public override void onNavStopStreamRequest(StopStream msg)
		{

		}

		public override void onNavSubscribeWayPointsRequest(SubscribeWayPoints msg)
		{

		}

		public override void onNavUnsubscribeWayPointsRequest(UnsubscribeWayPoints msg)
		{

		}

		public override void onNavUpdateTurnListRequest(UpdateTurnList msg)
		{

		}

		public override void onVehicleInfoDiagnosticMessageRequest(DiagnosticMessage msg)
		{

		}

		public override void onVehicleInfoGetDTCsRequest(GetDTCs msg)
		{

		}

		public override void onVehicleInfoGetVehicleDataRequest(GetVehicleData msg)
		{

		}

		public override void onVehicleInfoGetVehicleTypeRequest(GetVehicleType msg)
		{

		}

		public override void onVehicleInfoReadDIDRequest(ReadDID msg)
		{

		}

		public override void onVehicleInfoSubscribeVehicleDataRequest(SubscribeVehicleData msg)
		{

		}

		public override void onVehicleInfoUnsubscribeVehicleDataRequest(UnsubscribeVehicleData msg)
		{

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

		}

		public override void onBcDialNumberRequest(DialNumber msg)
		{

		}

		public override void onBcGetSystemInfoRequest(GetSystemInfo msg)
		{

		}

		public override void onBcPolicyUpdateRequest(PolicyUpdate msg)
		{

		}

		public override void onBcSystemRequestRequest(SystemRequest msg)
		{

		}

		public override void onBcUpdateAppListRequest(UpdateAppList msg)
		{

		}

		public override void onBcUpdateDeviceListRequest(UpdateDeviceList msg)
		{

		}

		public override void OnUiCommandNotification(OnCommand msg)
		{

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
