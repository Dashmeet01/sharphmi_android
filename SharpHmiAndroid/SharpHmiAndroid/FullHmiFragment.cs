using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using HmiApiLib.Base;
using HmiApiLib.Builder;
using HmiApiLib.Common.Enums;
using HmiApiLib.Common.Structs;
using HmiApiLib.Controllers.Buttons.IncomingNotifications;
using HmiApiLib.Controllers.UI.IncomingRequests;

namespace SharpHmiAndroid
{
    public class FullHmiFragment : Fragment, SeekBar.IOnSeekBarChangeListener
    {
        // Basic Communication Outgoing Response
        private string BCResponseActivateApp = "BCResponseActivateApp";
        private string BCResponseAllowDeviceToConnect = "BCResponseAllowDeviceToConnect";
        private string BCResponseDialNumber = "BCResponseDialNumber";
        private string BCResponseGetSystemInfo = "BCResponseGetSystemInfo";
        private string BCResponseMixingAudioSupported = "BCResponseMixingAudioSupported";
        private string BCResponsePolicyUpdate = "BCResponsePolicyUpdate";
        private string BCResponseSystemRequest = "BCResponseSystemRequest";
        private string BCResponseUpdateAppList = "BCResponseUpdateAppList";
        private string BCResponseUpdateDeviceList = "BCResponseUpdateDeviceList";

		// Basic Communication Outgoing Notification
		private string BCNotificationOnAppActivated = "BCNotificationOnAppActivated";
        private string BCNotificationOnAppDeactivated = "BCNotificationOnAppDeactivated";
        private string BCNotificationOnAwakeSDL = "BCNotificationOnAwakeSDL";
        private string BCNotificationOnDeactivateHMI = "BCNotificationOnDeactivateHMI";
        private string BCNotificationOnDeviceChosen = "BCNotificationOnDeviceChosen";
        private string BCNotificationOnEmergencyEvent = "BCNotificationOnEmergencyEvent";
        private string BCNotificationOnEventChanged = "BCNotificationOnEventChanged";
        private string BCNotificationOnExitAllApplications = "BCNotificationOnExitAllApplications";
        private string BCNotificationOnExitApplication = "BCNotificationOnExitApplication";
        private string BCNotificationOnFindApplications = "BCNotificationOnFindApplications";
        private string BCNotificationOnIgnitionCycleOver = "BCNotificationOnIgnitionCycleOver";
        private string BCNotificationOnPhoneCall = "BCNotificationOnPhoneCall";
        private string BCNotificationOnReady = "BCNotificationOnReady";
        private string BCNotificationOnStartDeviceDiscovery = "BCNotificationOnStartDeviceDiscovery";
        private string BCNotificationOnSystemInfoChanged = "BCNotificationOnSystemInfoChanged";
        private string BCNotificationOnSystemRequest = "BCNotificationOnSystemRequest";
        private string BCNotificationOnUpdateDeviceList = "BCNotificationOnUpdateDeviceList";

		// Button Outgoing Response
        private string ButtonsResponseGetCapabilities = "ButtonsResponseGetCapabilities";

		// Button Outgoing Notifications
        private string ButtonsNotificationOnButtonEvent = "ButtonsNotificationOnButtonEvent";
        private string ButtonsNotificationOnButtonPress = "BCNotificationOnButtonPress";

		// Navigation Outgoing Response
		private string NavigationResponseAlertManeuver = "NavigationResponseAlertManeuver";
        private string NavigationResponseGetWayPoints = "NavigationResponseGetWayPoints";
        private string NavigationResponseIsReady = "NavigationResponseIsReady";
        private string NavigationResponseSendLocation = "NavigationResponseSendLocation";
        private string NavigationResponseShowConstantTBT = "NavigationResponseShowConstantTBT";
        private string NavigationResponseStartAudioStream = "NavigationResponseStartAudioStream";
        private string NavigationResponseStartStream = "NavigationResponseStartStream";
        private string NavigationResponseStopAudioStream = "NavigationResponseStopAudioStream";
        private string NavigationResponseStopStream = "NavigationResponseStopStream";
        private string NavigationResponseSubscribeWayPoints = "NavigationResponseSubscribeWayPoints";
        private string NavigationResponseUnsubscribeWayPoints = "NavigationResponseUnsubscribeWayPoints";
        private string NavigationResponseUpdateTurnList = "NavigationResponseUpdateTurnList";

		// Navigation Outgoing Notifications
		private string NavigationNotificationOnTBTClientState = "NavigationNotificationOnTBTClientState";

		// SDL Outgoing Request
		private string SDLRequestActivateApp = "SDLRequestActivateApp";
        private string SDLRequestGetListOfPermissions = "SDLRequestGetListOfPermissions";
        private string SDLRequestGetStatusUpdate = "SDLRequestGetStatusUpdate";
        private string SDLRequestGetURLS = "SDLRequestGetURLS";
        private string SDLRequestGetUserFriendlyMessage = "SDLRequestGetUserFriendlyMessage";
        private string SDLRequestUpdateSDL = "SDLRequestUpdateSDL";

		// SDL Outgoing Notifications
		private string SDLNotificationOnAllowSDLFunctionality = "SDLNotificationOnAllowSDLFunctionality";
        private string SDLNotificationOnAppPermissionConsent = "SDLNotificationOnAppPermissionConsent";
        private string SDLNotificationOnPolicyUpdate = "SDLNotificationOnPolicyUpdate";
        private string SDLNotificationOnReceivedPolicyUpdate = "SDLNotificationOnReceivedPolicyUpdate";

		// TTS Outgoing Response
		private string TTSResponseChangeRegistration = "TTSResponseChangeRegistration";
		private string TTSResponseGetCapabilities = "TTSResponseGetCapabilities";
		private string TTSResponseGetLanguage = "TTSResponseGetLanguage";
		private string TTSResponseGetSupportedLanguages = "TTSResponseGetSupportedLanguages";
		private string TTSResponseIsReady = "TTSResponseIsReady";
		private string TTSResponseSetGlobalProperties = "TTSResponseSetGlobalProperties";
		private string TTSResponseSpeak = "TTSResponseSpeak";
		private string TTSResponseStopSpeaking = "TTSResponseStopSpeaking";

		// TTS Outgoing Notifications
		private string TTSNotificationOnLanguageChange = "TTSNotificationOnLanguageChange";
		private string TTSNotificationOnResetTimeout = "TTSNotificationOnResetTimeout";
		private string TTSNotificationStarted = "TTSNotificationStarted";
		private string TTSNotificationStopped = "TTSNotificationStopped";

		// UI Outgoing Response
		private string UIResponseAddCommand = "UIResponseAddCommand";
        private string UIResponseAddSubMenu = "UIResponseAddSubMenu";
        private string UIResponseAlert = "UIResponseAlert";
        private string UIResponseChangeRegistration = "UIResponseChangeRegistration";
        private string UIResponseClosePopUp = "UIResponseClosePopUp";
        private string UIResponseDeleteCommand = "UIResponseDeleteCommand";
        private string UIResponseDeleteSubMenu = "UIResponseDeleteSubMenu";
        private string UIResponseEndAudioPassThru = "UIResponseEndAudioPassThru";
        private string UIResponseGetCapabilities = "UIResponseGetCapabilities";
        private string UIResponseGetLanguage = "UIResponseGetLanguage";
        private string UIResponseGetSupportedLanguages = "UIResponseGetSupportedLanguages";
        private string UIResponseIsReady = "UIResponseIsReady";
        private string UIResponsePerformAudioPassThru = "UIResponsePerformAudioPassThru";
        private string UIResponsePerformInteraction = "UIResponsePerformInteraction";
        private string UIResponseScrollableMessage = "UIResponseScrollableMessage";
        private string UIResponseSetAppIcon = "UIResponseSetAppIcon";
        private string UIResponseSetDisplayLayout = "UIResponseSetDisplayLayout";
        private string UIResponseSetGlobalProperties = "UIResponseSetGlobalProperties";
        private string UIResponseSetMediaClockTimer = "UIResponseSetMediaClockTimer";
        private string UIResponseShow = "UIResponseShow";
        private string UIResponseShowCustomForm = "UIResponseShowCustomForm";
        private string UIResponseSlider = "UIResponseSlider";

		// UI Outgoing Notifications
		private string UINotificationOnCommand = "UINotificationOnCommand";
        private string UINotificationOnDriverDistraction = "UINotificationOnDriverDistraction";
        private string UINotificationOnKeyboardInput = "UINotificationOnKeyboardInput";
        private string UINotificationOnLanguageChange = "UINotificationOnLanguageChange";
        private string UINotificationOnRecordStart = "UINotificationOnRecordStart";
        private string UINotificationOnResetTimeout = "UINotificationOnResetTimeout";
        private string UINotificationOnSystemContext = "UINotificationOnSystemContext";
        private string UINotificationOnTouchEvent = "UINotificationOnTouchEvent";

		// VehicleInfo Outgoing Response
		private string VIResponseDiagnosticMessage = "VIResponseDiagnosticMessage";
        private string VIResponseGetDTCs = "VIResponseGetDTCs";
        private string VIResponseGetVehicleData = "VIResponseGetVehicleData";
        private string VIResponseGetVehicleType = "VIResponseGetVehicleType";
        private string VIResponseIsReady = "VIResponseIsReady";
        private string VIResponseReadDID = "VIResponseReadDID";
        private string VIResponseSubscribeVehicleData = "VIResponseSubscribeVehicleData";
        private string VIResponseUnsubscribeVehicleData = "VIResponseUnsubscribeVehicleData";

		int appID;
        public static readonly String sClickedAppID = "APP_ID";

        TextView mainField1;
        TextView mainField2;
        TextView mainField3;
        TextView mainField4;

        TextView mediaClockTimer;
        TextView mediaTrackText;

        Button softButton1;
        Button softButton2;
        Button softButton3;
        Button softButton4;
        Button softButton5;
        Button softButton6;
        Button softButton7;
        Button softButton8;

        ImageView mainGraphic;
        ImageView previousButton;
        ImageView playPauseButton;
        ImageView nextButton;

        SeekBar mSeekBar;
        LinearLayout mChoiceListLayout;
        TextView mediaStartText;
        TextView mediaEndText;

        Handler mHandler;
        Action action;
        long currentTime = -1;
        long totalStartSeconds = 0;
        long totalEndSeconds = 0;
        int numTicks = 0;
        bool isMediaTimerStarted = true;

        TextView sliderHeader;
        TextView sliderFooter;
        Button sliderSave;
        Button sliderCancel;
        List<String> sliderFooterList;
        int sliderCurrentPosition = 0;

		LayoutInflater layoutIinflater;

        string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
        string[] vehicleDataType = Enum.GetNames(typeof(VehicleDataType));
        string[] languages = Enum.GetNames(typeof(Language));
		string[] transportType = Enum.GetNames(typeof(HmiApiLib.Common.Enums.TransportType));
		string[] eventTypes = Enum.GetNames(typeof(HmiApiLib.Common.Enums.EventTypes));
		string[] fileType = Enum.GetNames(typeof(HmiApiLib.Common.Enums.FileType));
		string[] requestType = Enum.GetNames(typeof(HmiApiLib.Common.Enums.RequestType));
		string[] appsCloseReason = Enum.GetNames(typeof(HmiApiLib.Common.Enums.ApplicationsCloseReason));
		string[] appsExitReason = Enum.GetNames(typeof(HmiApiLib.Common.Enums.ApplicationExitReason));
		string[] buttonNames = Enum.GetNames(typeof(HmiApiLib.ButtonName));
		string[] buttonEventMode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.ButtonEventMode));
		string[] buttonPressMode = Enum.GetNames(typeof(HmiApiLib.ButtonPressMode));
		string[] imageType = Enum.GetNames(typeof(HmiApiLib.Common.Enums.ImageType));
		string[] driverDistractionState = Enum.GetNames(typeof(DriverDistractionState));
		string[] keyBoardEvent = Enum.GetNames(typeof(KeyboardEvent));

		public FullHmiFragment()
        {
            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.full_hmi_fragment, container,
                false);

            layoutIinflater = inflater;

            appID = Arguments.GetInt(sClickedAppID);

            mainField1 = (TextView)rootView.FindViewById(Resource.Id.show_main_field_1);
            mainField2 = (TextView)rootView.FindViewById(Resource.Id.show_main_field_2);
            mainField3 = (TextView)rootView.FindViewById(Resource.Id.show_main_field_3);
            mainField4 = (TextView)rootView.FindViewById(Resource.Id.show_main_field_4);

            mediaClockTimer = (TextView)rootView.FindViewById(Resource.Id.show_media_track_timer);
            mediaTrackText = (TextView)rootView.FindViewById(Resource.Id.show_media_track__timer_text);

            softButton1 = (Button)rootView.FindViewById(Resource.Id.soft_btn_1);
            softButton2 = (Button)rootView.FindViewById(Resource.Id.soft_btn_2);
            softButton3 = (Button)rootView.FindViewById(Resource.Id.soft_btn_3);
            softButton4 = (Button)rootView.FindViewById(Resource.Id.soft_btn_4);
            softButton5 = (Button)rootView.FindViewById(Resource.Id.soft_btn_5);
            softButton6 = (Button)rootView.FindViewById(Resource.Id.soft_btn_6);
            softButton7 = (Button)rootView.FindViewById(Resource.Id.soft_btn_7);
            softButton8 = (Button)rootView.FindViewById(Resource.Id.soft_btn_8);

            mainGraphic = (ImageView)rootView.FindViewById(Resource.Id.show_image);
            previousButton = (ImageView)rootView.FindViewById(Resource.Id.button_hmi_previous);
            playPauseButton = (ImageView)rootView.FindViewById(Resource.Id.button_hmi_play_pause);
            nextButton = (ImageView)rootView.FindViewById(Resource.Id.button_hmi_next);

            mediaStartText = (TextView)rootView.FindViewById(Resource.Id.full_hmi_set_media_start);
            mSeekBar = (SeekBar)rootView.FindViewById(Resource.Id.full_hmi_set_media_seekBar);
            mediaEndText = (TextView)rootView.FindViewById(Resource.Id.full_hmi_set_media_end);

            mChoiceListLayout = (LinearLayout)rootView.FindViewById(Resource.Id.full_hmi_linear_layout);

            sliderSave = (Button)rootView.FindViewById(Resource.Id.full_hmi_slider_save);
            sliderCancel = (Button)rootView.FindViewById(Resource.Id.full_hmi_slider_cancel);
            sliderHeader = (TextView)rootView.FindViewById(Resource.Id.full_hmi_slider_header);
            sliderFooter = (TextView)rootView.FindViewById(Resource.Id.full_hmi_slider_footer);

            return rootView;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.full_hmi_menu, menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.full_hmi_options:
                    ((MainActivity)Activity).setOptionsFragment(appID);
                    return true;
				case Resource.Id.full_hmi_rpc_list:
					showRPCListDialog();
					return true;
				case Resource.Id.full_hmi_choice:
					//((MainActivity)Activity).setOptionsFragment(appID);
					return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        internal void onUiShowRequestCallback(Show msg)
        {
            Activity.RunOnUiThread(() => UpdateShowUI(msg));
        }

        private void UpdateShowUI(Show msg)
        {
            HideSliderUI();
            if ((msg.getShowStrings() != null) && (msg.getShowStrings().Count > 0))
            {
                ClearText();
                for (int i = 0; i < msg.getShowStrings().Count; i++)
                {
                    switch (msg.getShowStrings()[i].fieldName)
                    {
                        case TextFieldName.mainField1:
                            mainField1.Text = msg.getShowStrings()[i].fieldText;
                            break;
                        case TextFieldName.mainField2:
                            mainField2.Text = msg.getShowStrings()[i].fieldText;
                            break;
                        case TextFieldName.mainField3:
                            mainField3.Text = msg.getShowStrings()[i].fieldText;
                            break;
                        case TextFieldName.mainField4:
                            mainField4.Text = msg.getShowStrings()[i].fieldText;
                            break;
                        case TextFieldName.mediaClock:
                            mediaClockTimer.Text = msg.getShowStrings()[i].fieldText;
                            break;
                        case TextFieldName.mediaTrack:
                            mediaTrackText.Text = msg.getShowStrings()[i].fieldText;
                            break;
                        default:
                            break;
                    }
                }
            }
            invisibleSoftButtons();
            mSeekBar.Visibility = ViewStates.Invisible;

            if ((msg.getSoftButtons() != null) && (msg.getSoftButtons().Count > 0))
            {
                for (int i = 0; i < msg.getSoftButtons().Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            softButton1.Text = msg.getSoftButtons()[0].getText();
                            softButton1.Visibility = ViewStates.Visible;
                            break;
                        case 1:
                            softButton2.Text = msg.getSoftButtons()[1].getText();
                            softButton2.Visibility = ViewStates.Visible;
                            break;
                        case 2:
                            softButton3.Text = msg.getSoftButtons()[2].getText();
                            softButton3.Visibility = ViewStates.Visible;
                            break;
                        case 3:
                            softButton4.Text = msg.getSoftButtons()[3].getText();
                            softButton4.Visibility = ViewStates.Visible;
                            break;
                        case 4:
                            softButton5.Text = msg.getSoftButtons()[4].getText();
                            softButton5.Visibility = ViewStates.Visible;
                            break;
                        case 5:
                            softButton6.Text = msg.getSoftButtons()[5].getText();
                            softButton6.Visibility = ViewStates.Visible;
                            break;
                        case 6:
                            softButton7.Text = msg.getSoftButtons()[6].getText();
                            softButton7.Visibility = ViewStates.Visible;
                            break;
                        case 7:
                            softButton8.Text = msg.getSoftButtons()[7].getText();
                            softButton8.Visibility = ViewStates.Visible;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        internal void onUiScrollableMessageRequestCallback(ScrollableMessage msg)
        {
            Activity.RunOnUiThread(() => UpdateScrollUI(msg));
        }

        void UpdateScrollUI(ScrollableMessage msg)
        {
            HideSliderUI();
            ClearText();

            if (msg.getMessageText().fieldName == TextFieldName.scrollableMessageBody)
            {
                mainField1.Text = msg.getMessageText().fieldText;
            }

            invisibleSoftButtons();
            mSeekBar.Visibility = ViewStates.Invisible;

            if ((msg.getSoftButtons() != null) && (msg.getSoftButtons().Count > 0))
            {
                for (int i = 0; i < msg.getSoftButtons().Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            softButton1.Text = msg.getSoftButtons()[0].getText();
                            softButton1.Visibility = ViewStates.Visible;
                            break;
                        case 1:
                            softButton2.Text = msg.getSoftButtons()[1].getText();
                            softButton2.Visibility = ViewStates.Visible;
                            break;
                        case 2:
                            softButton3.Text = msg.getSoftButtons()[2].getText();
                            softButton3.Visibility = ViewStates.Visible;
                            break;
                        case 3:
                            softButton4.Text = msg.getSoftButtons()[3].getText();
                            softButton4.Visibility = ViewStates.Visible;
                            break;
                        case 4:
                            softButton5.Text = msg.getSoftButtons()[4].getText();
                            softButton5.Visibility = ViewStates.Visible;
                            break;
                        case 5:
                            softButton6.Text = msg.getSoftButtons()[5].getText();
                            softButton6.Visibility = ViewStates.Visible;
                            break;
                        case 6:
                            softButton7.Text = msg.getSoftButtons()[6].getText();
                            softButton7.Visibility = ViewStates.Visible;
                            break;
                        case 7:
                            softButton8.Text = msg.getSoftButtons()[7].getText();
                            softButton8.Visibility = ViewStates.Visible;
                            break;
                        default:
                            break;
                    }
                }
            }

			int? totalDuration = msg.getTimeout();
			if (totalDuration != null)
			{
				Handler handler = new Handler(Looper.MainLooper);
				action = delegate
				{
					ClearText();
                    invisibleSoftButtons();
				};
				handler.PostDelayed(action, (long)totalDuration);
			}
        }

        void invisibleSoftButtons()
        {
            softButton1.Visibility = ViewStates.Invisible;
            softButton2.Visibility = ViewStates.Invisible;
            softButton3.Visibility = ViewStates.Invisible;
            softButton4.Visibility = ViewStates.Invisible;
            softButton5.Visibility = ViewStates.Invisible;
            softButton6.Visibility = ViewStates.Invisible;
            softButton7.Visibility = ViewStates.Invisible;
            softButton8.Visibility = ViewStates.Invisible;
        }

        void ClearText()
        {
            mainField1.Text = null;
            mainField2.Text = null;
            mainField3.Text = null;
            mainField4.Text = null;
            mediaClockTimer.Text = null;
            mediaTrackText.Text = null;
            mChoiceListLayout.RemoveViews(0, mChoiceListLayout.ChildCount);
        }

        internal void onUiSetMediaClockTimerRequestCallback(SetMediaClockTimer msg)
        {
            Activity.RunOnUiThread(() => UpdateSetMediaTimerUI(msg));
        }

        private void UpdateSetMediaTimerUI(SetMediaClockTimer msg)
        {
            HideSliderUI();
            mSeekBar.Visibility = ViewStates.Visible;
            mSeekBar.Enabled = false;

			if ((mHandler != null) && (action != null))
			{
				mHandler.RemoveCallbacks(action);
			}
			mHandler = new Handler();
				
            if ((msg.getUpdateMode() == ClockUpdateMode.COUNTUP) || (msg.getUpdateMode() == ClockUpdateMode.COUNTDOWN))
            {
                string startText = msg.getStartTime().getHours().ToString();
                startText = startText + ":" + msg.getStartTime().getMinutes().ToString();
                startText = startText + ":" + msg.getStartTime().getSeconds().ToString();

                totalStartSeconds = (msg.getStartTime().getHours() * 3600)
                                            + (msg.getStartTime().getMinutes() * 60)
                                            + (msg.getStartTime().getSeconds());

                if ((msg.getEndTime() != null))
                {
					string endText = msg.getEndTime().getHours().ToString();
					endText = endText + ":" + msg.getEndTime().getMinutes().ToString();
					endText = endText + ":" + msg.getEndTime().getSeconds().ToString();

					totalEndSeconds = (msg.getEndTime().getHours() * 3600)
												+ (msg.getEndTime().getMinutes() * 60)
												+ (msg.getEndTime().getSeconds());

					mediaStartText.Text = startText;
					mediaEndText.Text = endText;
                }

                if ((msg.getUpdateMode() == ClockUpdateMode.COUNTUP) && (msg.getEndTime() != null))
                {
                    if (totalEndSeconds < totalStartSeconds)
                        return;
                    double initProgress = (((double)totalStartSeconds) / totalEndSeconds) * 100;
                    int initialProgress = Convert.ToInt32(initProgress);

                    currentTime = 0;
                    mSeekBar.SetProgress(initialProgress, false);
                }
				else if ((msg.getUpdateMode() == ClockUpdateMode.COUNTUP) && (msg.getEndTime() == null))
				{
					currentTime = 0;
                    mSeekBar.Visibility = ViewStates.Invisible;
                    mediaEndText.Visibility = ViewStates.Invisible;
				}
                else if ((msg.getUpdateMode() == ClockUpdateMode.COUNTDOWN) && (msg.getEndTime() != null))
                {
                    if (totalEndSeconds > totalStartSeconds)
                        return;
                    mSeekBar.SetProgress(100, false);
                }
				else if ((msg.getUpdateMode() == ClockUpdateMode.COUNTDOWN) && (msg.getEndTime() == null))
				{
                    currentTime = totalStartSeconds;
					mSeekBar.Visibility = ViewStates.Invisible;
					mediaEndText.Visibility = ViewStates.Invisible;
				}
            }
            else if (msg.getUpdateMode() == ClockUpdateMode.PAUSE)
            {
                if ((mHandler != null) && (action != null))
                {
                    mHandler.RemoveCallbacks(action);
                }
                mHandler = null;
                return;
            }
            else if (msg.getUpdateMode() == ClockUpdateMode.RESUME)
            {
                if (totalEndSeconds == 0)
                {
                    return;
                }
                if ((mHandler != null) && (action != null))
                {
                    mHandler.RemoveCallbacks(action);
                }
                mHandler = new Handler();
            }
            else if (msg.getUpdateMode() == ClockUpdateMode.CLEAR)
            {
                if ((mHandler != null) && (action != null))
                {
                    mHandler.RemoveCallbacks(action);
                }
                mHandler = null;
                currentTime = 0;
                totalStartSeconds = 0;
                totalEndSeconds = 0;
                mediaStartText.Visibility = ViewStates.Invisible;
                mSeekBar.Visibility = ViewStates.Invisible;
                mediaEndText.Visibility = ViewStates.Invisible;
                return;
            }

            action = delegate
            {
                HandleAction(totalStartSeconds, totalEndSeconds, msg);
            };
            if (null != mHandler)
                mHandler.PostDelayed(action, 100);
        }

        void HandleAction(long startTime, long endTime, SetMediaClockTimer msg)
        {
            if(msg.getUpdateMode() == ClockUpdateMode.COUNTUP)
            {
				if (currentTime == 0)
				{
					currentTime = startTime;
				}
				int hours = (int)(currentTime / 3600);
				int minutes = (int)(currentTime % 3600) / 60;
				int seconds = (int)((currentTime % 3600) % 60);

				string currentTimeText = hours + ":" + minutes + ":" + seconds;
				mediaStartText.Text = currentTimeText;

				currentTime++;
            }
            else if (msg.getUpdateMode() == ClockUpdateMode.COUNTDOWN)
            {
                if (isMediaTimerStarted)
                {
                    currentTime = startTime;
                    isMediaTimerStarted = false;
                }
				int hours = (int)(currentTime / 3600);
				int minutes = (int)(currentTime % 3600) / 60;
				int seconds = (int)((currentTime % 3600) % 60);

				string currentTimeText = hours + ":" + minutes + ":" + seconds;
				mediaStartText.Text = currentTimeText;

				currentTime--;
            }
            if (msg.getEndTime() != null)
            {
				if (startTime < endTime)
				{
					if (endTime == 0)
						return;

					double initProgress = (((double)currentTime) / endTime) * 100;
					int initialProgress = Convert.ToInt32(initProgress);
					mSeekBar.SetProgress(initialProgress, false);

					if (initialProgress >= 100)
					{
						if (null != mHandler)
							mHandler.RemoveCallbacks(action);
						currentTime = 0;
						return;
					}
				}
				else
				{
					double initProgress = (((double)currentTime) / startTime) * 100;
					int initialProgress = Convert.ToInt32(initProgress);
					mSeekBar.SetProgress(initialProgress, false);

					if (currentTime <= endTime)
					{
						if (null != mHandler)
							mHandler.RemoveCallbacks(action);
						currentTime = 0;
						return;
					}
				}
            }

            if (null != mHandler)
                mHandler.PostDelayed(action, 1000);
        }

        internal void OnButtonSubscriptionNotificationCallback(OnButtonSubscription msg)
        {
            Activity.RunOnUiThread(() => UpdateButtonSubscription(msg));
        }

        void UpdateButtonSubscription(OnButtonSubscription msg)
        {
            HideSliderUI();
            if (msg.getName() == HmiApiLib.ButtonName.OK)
			{
				if ((msg.getSubscribe() != null) && ((bool)msg.getSubscribe()))
				{
					playPauseButton.Visibility = ViewStates.Visible;
				}
				else
				{
					playPauseButton.Visibility = ViewStates.Invisible;
				}
			}
            else if (msg.getName() == HmiApiLib.ButtonName.SEEKLEFT)
			{
				if ((msg.getSubscribe() != null) && ((bool)msg.getSubscribe()))
				{
					previousButton.Visibility = ViewStates.Visible;
				}
				else
				{
					previousButton.Visibility = ViewStates.Invisible;
				}
			}
            else if (msg.getName() == HmiApiLib.ButtonName.SEEKRIGHT)
			{
				if ((msg.getSubscribe() != null) && ((bool)msg.getSubscribe()))
				{
					nextButton.Visibility = ViewStates.Visible;
				}
				else
				{
					nextButton.Visibility = ViewStates.Invisible;
				}
			}
        }

        internal void onUiSliderRequestCallback(Slider msg)
        {
            Activity.RunOnUiThread(() => UpdateSliderUI(msg));
        }

        private void UpdateSliderUI(Slider msg)
        {
            ShowSliderUI();
            int currentPosition = 0;
			sliderFooterList = msg.getSliderFooter();
			if (msg.getNumTicks() != null)
				numTicks = (int)msg.getNumTicks();
			if (msg.getPosition() != null)
			    currentPosition = (int)msg.getPosition();

			double partitionLenghthInDouble = (double)100 / numTicks;
			int actualPartitionLength = (int)Math.Round(partitionLenghthInDouble);

            int actualSeekbarPosition = actualPartitionLength * currentPosition;
			mSeekBar.SetProgress(actualSeekbarPosition, true);

            sliderCurrentPosition = currentPosition;

			mSeekBar.Visibility = ViewStates.Visible;
			mSeekBar.Enabled = true;
            mSeekBar.SetOnSeekBarChangeListener(this);

            sliderHeader.Text = msg.getSliderHeader();

            if ((sliderFooterList != null) && (sliderFooterList.Count == 1))
            {
                sliderFooter.Text = sliderFooterList[0];
            }
            else if ((sliderFooterList != null) && (sliderFooterList.Count > 1))
            {
				if (currentPosition > 0)
				{
					sliderFooter.Text = sliderFooterList[currentPosition - 1];
				}
				else
				{
					sliderFooter.Text = null;
				}
            }
            else
            {
                sliderFooter.Text = null;
			}

			int totalDuration = 1000;
			if (msg.getTimeout() != null)
			{
				totalDuration = (int)msg.getTimeout();
			}

			Handler handler = new Handler(Looper.MainLooper);
			action = delegate
			{
                HideSliderUI();
			};
			if (null != handler)
				handler.PostDelayed(action, totalDuration);

			sliderSave.Click += (sender, e) =>
			{
                AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiSliderResponse(msg.getId(), HmiApiLib.Common.Enums.Result.SUCCESS, sliderCurrentPosition));
                HideSliderUI();
			};

            sliderCancel.Click += (sender, e) =>
			{
                AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiSliderResponse(msg.getId(), HmiApiLib.Common.Enums.Result.ABORTED, sliderCurrentPosition));
                HideSliderUI();
			};
		}

        public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
        {

        }

        public void OnStartTrackingTouch(SeekBar seekBar)
        {

        }

        public void OnStopTrackingTouch(SeekBar seekBar)
        {
            int currentProgres = seekBar.Progress;

            double partitionLenghthInDouble = (double) 100 / numTicks;
            int actualPartitionLength = (int)Math.Round(partitionLenghthInDouble);

            double nextPosition = (double) currentProgres / actualPartitionLength;
            int setPos = (int)Math.Round(nextPosition);

            int finalSeekbar = actualPartitionLength * setPos;
            mSeekBar.SetProgress(finalSeekbar, true);

			if ((sliderFooterList != null) && (sliderFooterList.Count > 1))
			{
                if (setPos > 0)
                {
                    sliderFooter.Text = sliderFooterList[setPos - 1];
                }
                else
                {
                    sliderFooter.Text = null;
				}
			}
            sliderCurrentPosition = setPos;
        }

        void HideSliderUI()
        {
            sliderSave.Visibility = ViewStates.Gone;
            sliderCancel.Visibility = ViewStates.Gone;
            sliderHeader.Visibility = ViewStates.Gone;
            sliderFooter.Visibility = ViewStates.Gone;
            mSeekBar.Visibility = ViewStates.Gone;
        }

        void ShowSliderUI()
        {
            sliderSave.Visibility = ViewStates.Visible;
			sliderCancel.Visibility = ViewStates.Visible;
			sliderHeader.Visibility = ViewStates.Visible;
			sliderFooter.Visibility = ViewStates.Visible;
            mSeekBar.Visibility = ViewStates.Visible;
        }

        public void showRPCListDialog()
        {
            String[] rpcListStringArary = { BCResponseAllowDeviceToConnect, BCResponseDialNumber, BCResponseGetSystemInfo, 
                BCResponseMixingAudioSupported, BCResponsePolicyUpdate, BCResponseSystemRequest, BCResponseUpdateAppList, 
                BCResponseUpdateDeviceList, BCNotificationOnAppActivated, BCNotificationOnAppDeactivated, 
                BCNotificationOnAwakeSDL, BCNotificationOnDeactivateHMI, BCNotificationOnDeviceChosen,
                BCNotificationOnEmergencyEvent, BCNotificationOnEventChanged, BCNotificationOnExitAllApplications, 
                BCNotificationOnExitApplication, BCNotificationOnFindApplications, BCNotificationOnIgnitionCycleOver, 
                BCNotificationOnPhoneCall, BCNotificationOnReady, BCNotificationOnStartDeviceDiscovery, 
                BCNotificationOnSystemInfoChanged, BCNotificationOnSystemRequest, BCNotificationOnUpdateDeviceList,
                ButtonsResponseGetCapabilities, ButtonsNotificationOnButtonEvent, ButtonsNotificationOnButtonPress,
                NavigationResponseAlertManeuver, NavigationResponseGetWayPoints, NavigationResponseIsReady,
                NavigationResponseSendLocation, NavigationResponseShowConstantTBT, NavigationResponseStartAudioStream,
                NavigationResponseStartStream, NavigationResponseStopAudioStream, NavigationResponseStopStream,
                NavigationResponseSubscribeWayPoints, NavigationResponseUnsubscribeWayPoints, NavigationResponseUpdateTurnList,
                NavigationNotificationOnTBTClientState, SDLRequestActivateApp, SDLRequestGetListOfPermissions,
                SDLRequestGetStatusUpdate, SDLRequestGetURLS, SDLRequestGetUserFriendlyMessage, SDLRequestUpdateSDL, 
                SDLNotificationOnAllowSDLFunctionality, SDLNotificationOnAppPermissionConsent, SDLNotificationOnPolicyUpdate,
                SDLNotificationOnReceivedPolicyUpdate, TTSResponseChangeRegistration, TTSResponseGetCapabilities,
                TTSResponseGetLanguage, TTSResponseGetSupportedLanguages, TTSResponseIsReady, TTSResponseSetGlobalProperties,
                TTSResponseSpeak, TTSResponseStopSpeaking, TTSNotificationOnLanguageChange, TTSNotificationOnResetTimeout,
				TTSNotificationStarted, TTSNotificationStopped, UIResponseAddCommand, UIResponseAddSubMenu, UIResponseAlert,
                UIResponseChangeRegistration, UIResponseClosePopUp, UIResponseDeleteCommand, UIResponseDeleteSubMenu,
				UIResponseEndAudioPassThru, UIResponseGetCapabilities, UIResponseGetLanguage, UIResponseGetSupportedLanguages,
                UIResponseIsReady, UIResponsePerformAudioPassThru, UIResponsePerformInteraction, UIResponseScrollableMessage,
                UIResponseSetAppIcon, UIResponseSetDisplayLayout, UIResponseSetGlobalProperties, UIResponseSetMediaClockTimer,
                UIResponseShow, UIResponseShowCustomForm, UIResponseSlider, UINotificationOnCommand, UINotificationOnDriverDistraction,
                UINotificationOnKeyboardInput, UINotificationOnLanguageChange, UINotificationOnRecordStart, UINotificationOnResetTimeout,
                UINotificationOnSystemContext, UINotificationOnTouchEvent,  
                "DiagnosticMessageResponse", "GetDTCsResponse","GetVehicleDataResponse", "GetVehicleTypeResponse", "ReadDidResponse", "SubscribeVehicleDataResponse", "UnsubscribeVehicleDataResponse", "VRGetSupportedLanguageResponse"};

            AlertDialog.Builder rpcListAlertDialog = new AlertDialog.Builder(this.Context);
            //LayoutInflater inflaterr = Context.GetSystemService("nm");
            View layout = (View)layoutIinflater.Inflate(Resource.Layout.rpclistLayout, null);
            rpcListAlertDialog.SetView(layout);
            rpcListAlertDialog.SetTitle("Pick an RPC");

            rpcListAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                rpcListAlertDialog.Dispose();
            });

            ListView rpcListView = (ListView)layout.FindViewById(Resource.Id.listView1);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleListItem1, rpcListStringArary);
            rpcListView.Adapter = adapter;

            rpcListView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) =>
             {
                 string clickedItem = rpcListView.GetItemAtPosition(e.Position).ToString();
                 if (clickedItem.Equals(BCResponseActivateApp))
                 {
                     CreateBCResponseActivateApp();
                 }
                 else if (clickedItem.Equals(BCResponseAllowDeviceToConnect))
                 {
                     CreateBCResponseAllowDeviceToConnect();
                 }
                 else if (clickedItem.Equals(BCResponseDialNumber))
                 {
                     CreateBCResponseDialNumber();
                 }
                 else if (clickedItem.Equals(BCResponseGetSystemInfo))
                 {
                     CreateBCResponseGetSystemInfo();
                 }
                 else if (clickedItem.Equals(BCResponseMixingAudioSupported))
                 {
                     CreateBCResponseMixingAudioSupported();
                 }
                 else if (clickedItem.Equals(BCResponsePolicyUpdate))
                 {
                     CreateBCResponsePolicyUpdate();
                 }
                 else if (clickedItem.Equals(BCResponseSystemRequest))
                 {
                     CreateBCResponseSystemRequest();
                 }
                 else if (clickedItem.Equals(BCResponseUpdateAppList))
                 {
                     CreateBCResponseUpdateAppList();
                 }
                 else if (clickedItem.Equals(BCResponseUpdateDeviceList))
                 {
                     CreateBCResponseUpdateDeviceList();
                 }
                 else if (clickedItem.Equals(BCNotificationOnAppActivated))
                 {
                     CreateBCNotificationOnAppActivated();
                 }
                 else if (clickedItem.Equals(BCNotificationOnAppDeactivated))
                 {
                     CreateBCNotificationOnAppDeactivated();
                 }
                 else if (clickedItem.Equals(BCNotificationOnAwakeSDL))
                 {
                     CreateBCNotificationOnAwakeSDL();
                 }
                 else if (clickedItem.Equals(BCNotificationOnDeactivateHMI))
                 {
                     CreateBCNotificationOnDeactivateHMI();
                 }
                 else if (clickedItem.Equals(BCNotificationOnDeviceChosen))
                 {
                     CreateBCNotificationOnDeviceChosen();
                 }
                 else if (clickedItem.Equals(BCNotificationOnEmergencyEvent))
                 {
                     CreateBCNotificationOnEmergencyEvent();
                 }
                 else if (clickedItem.Equals(BCNotificationOnEventChanged))
                 {
                     CreateBCNotificationOnEventChanged();
                 }
                 else if (clickedItem.Equals(BCNotificationOnExitAllApplications))
                 {
                     CreateBCNotificationOnExitAllApplications();
                 }
                 else if (clickedItem.Equals(BCNotificationOnExitApplication))
                 {
                     CreateBCNotificationOnExitApplication();
                 }
                 else if (clickedItem.Equals(BCNotificationOnFindApplications))
                 {
                     CreateBCNotificationOnFindApplications();
                 }
                 else if (clickedItem.Equals(BCNotificationOnIgnitionCycleOver))
                 {
                     CreateBCNotificationOnIgnitionCycleOver();
                 }
                 else if (clickedItem.Equals(BCNotificationOnPhoneCall))
                 {
                     CreateBCNotificationOnPhoneCall();
                 }
                 else if (clickedItem.Equals(BCNotificationOnReady))
                 {
                     CreateBCNotificationOnReady();
                 }
                 else if (clickedItem.Equals(BCNotificationOnStartDeviceDiscovery))
                 {
                     CreateBCNotificationOnStartDeviceDiscovery();
                 }
                 else if (clickedItem.Equals(BCNotificationOnSystemInfoChanged))
                 {
                     CreateBCNotificationOnSystemInfoChanged();
                 }
                 else if (clickedItem.Equals(BCNotificationOnSystemRequest))
                 {
                     CreateBCNotificationOnSystemRequest();
                 }
                 else if (clickedItem.Equals(BCNotificationOnUpdateDeviceList))
                 {
                     CreateBCNotificationOnUpdateDeviceList();
                 }
                 else if (clickedItem.Equals(ButtonsResponseGetCapabilities))
                 {
                     CreateButtonsResponseGetCapabilities();
                 }
                 else if (clickedItem.Equals(ButtonsNotificationOnButtonEvent))
                 {
                     CreateButtonsNotificationOnButtonEvent();
                 }
                 else if (clickedItem.Equals(ButtonsNotificationOnButtonPress))
                 {
                     CreateButtonsNotificationOnButtonPress();
                 }
                 else if (clickedItem.Equals(NavigationResponseAlertManeuver))
                 {
                     CreateNavigationResponseAlertManeuver();
                 }
                 else if (clickedItem.Equals(NavigationResponseGetWayPoints))
                 {
                     CreateNavigationResponseGetWayPoints();
                 }
                 else if (clickedItem.Equals(NavigationResponseIsReady))
                 {
                     CreateNavigationResponseIsReady();
                 }
                 else if (clickedItem.Equals(NavigationResponseSendLocation))
                 {
                     CreateNavigationResponseSendLocation();
                 }
                 else if (clickedItem.Equals(NavigationResponseShowConstantTBT))
                 {
                     CreateNavigationResponseShowConstantTBT();
                 }
                 else if (clickedItem.Equals(NavigationResponseStartAudioStream))
                 {
                     CreateNavigationResponseStartAudioStream();
                 }
                 else if (clickedItem.Equals(NavigationResponseStartStream))
                 {
                     CreateNavigationResponseStartStream();
                 }
                 else if (clickedItem.Equals(NavigationResponseStopAudioStream))
                 {
                     CreateNavigationResponseStopAudioStream();
                 }
                 else if (clickedItem.Equals(NavigationResponseStopStream))
                 {
                     CreateNavigationResponseStopStream();
                 }
                 else if (clickedItem.Equals(NavigationResponseSubscribeWayPoints))
                 {
                     CreateNavigationResponseSubscribeWayPoints();
                 }
                 else if (clickedItem.Equals(NavigationResponseUnsubscribeWayPoints))
                 {
                     CreateNavigationResponseUnsubscribeWayPoints();
                 }
                 else if (clickedItem.Equals(NavigationResponseUpdateTurnList))
                 {
                     CreateNavigationResponseUpdateTurnList();
                 }
                 else if (clickedItem.Equals(NavigationNotificationOnTBTClientState))
                 {
                     CreateNavigationNotificationOnTBTClientState();
                 }
                 else if (clickedItem.Equals(SDLRequestActivateApp))
                 {
                     CreateSDLRequestActivateApp();
                 }
                 else if (clickedItem.Equals(SDLRequestGetListOfPermissions))
                 {
                     CreateSDLRequestGetListOfPermissions();
                 }
                 else if (clickedItem.Equals(SDLRequestGetStatusUpdate))
                 {
                     CreateSDLRequestGetStatusUpdate();
                 }
                 else if (clickedItem.Equals(SDLRequestGetURLS))
                 {
                     CreateSDLRequestGetURLS();
                 }
                 else if (clickedItem.Equals(SDLRequestGetUserFriendlyMessage))
                 {
                     CreateSDLRequestGetUserFriendlyMessage();
                 }
                 else if (clickedItem.Equals(SDLRequestUpdateSDL))
                 {
                     CreateSDLRequestUpdateSDL();
                 }
                 else if (clickedItem.Equals(SDLNotificationOnAllowSDLFunctionality))
                 {
                     CreateSDLNotificationOnAllowSDLFunctionality();
                 }
                 else if (clickedItem.Equals(SDLNotificationOnAppPermissionConsent))
                 {
                     CreateSDLNotificationOnAppPermissionConsent();
                 }
                 else if (clickedItem.Equals(SDLNotificationOnPolicyUpdate))
                 {
                     CreateSDLNotificationOnPolicyUpdate();
                 }
                 else if (clickedItem.Equals(SDLNotificationOnReceivedPolicyUpdate))
                 {
                     CreateSDLNotificationOnReceivedPolicyUpdate();
                 }
                 else if (clickedItem.Equals(TTSResponseChangeRegistration))
                 {
                     CreateTTSResponseChangeRegistration();
                 }
                 else if (clickedItem.Equals(TTSResponseGetCapabilities))
                 {
                     CreateTTSResponseGetCapabilities();
                 }
                 else if (clickedItem.Equals(TTSResponseGetLanguage))
                 {
                     CreateTTSResponseGetLanguage();
                 }
                 else if (clickedItem.Equals(TTSResponseGetSupportedLanguages))
                 {
                     CreateTTSResponseGetSupportedLanguages();
                 }
                 else if (clickedItem.Equals(TTSResponseIsReady))
                 {
                     CreateTTSResponseIsReady();
                 }
                 else if (clickedItem.Equals(TTSResponseSetGlobalProperties))
                 {
                     CreateTTSResponseSetGlobalProperties();
                 }
                 else if (clickedItem.Equals(TTSResponseSpeak))
                 {
                     CreateTTSResponseSpeak();
                 }
                 else if (clickedItem.Equals(TTSResponseStopSpeaking))
                 {
                     CreateTTSResponseStopSpeaking();
                 }
                 else if (clickedItem.Equals(TTSNotificationOnLanguageChange))
                 {
                     CreateTTSNotificationOnLanguageChange();
                 }
                 else if (clickedItem.Equals(TTSNotificationOnResetTimeout))
                 {
                     OnResetTimeoutNotification();
                 }
                 else if (clickedItem.Equals(TTSNotificationStarted))
                 {
                     CreateTTSNotificationStarted();
                 }
                 else if (clickedItem.Equals(TTSNotificationStopped))
                 {
                     CreateTTSNotificationStopped();
                 }
                 else if (clickedItem.Equals(UIResponseAddCommand))
                 {
                     CreateUIResponseAddCommand();
                 }
                 else if (clickedItem.Equals(UIResponseAddSubMenu))
                 {
                     CreateUIResponseAddSubMenu();
                 }
                 else if (clickedItem.Equals(UIResponseAlert))
                 {
                     CreateUIResponseAlert();
                 }
                 else if (clickedItem.Equals(UIResponseChangeRegistration))
                 {
                     CreateUIResponseChangeRegistration();
                 }
                 else if (clickedItem.Equals(UIResponseClosePopUp))
                 {
                     CreateUIResponseClosePopUp();
                 }
                 else if (clickedItem.Equals(UIResponseDeleteCommand))
                 {
                     CreateUIResponseDeleteCommand();
                 }
                 else if (clickedItem.Equals(UIResponseDeleteSubMenu))
                 {
                     CreateUIResponseDeleteSubMenu();
                 }
                 else if (clickedItem.Equals(UIResponseEndAudioPassThru))
                 {
                     CreateUIResponseEndAudioPassThru();
                 }
                 else if (clickedItem.Equals(UIResponseGetCapabilities))
                 {
                     CreateUIResponseGetCapabilities();
                 }
                 else if (clickedItem.Equals(UIResponseGetLanguage))
                 {
                     CreateUIResponseGetLanguage();
                 }
                 else if (clickedItem.Equals(UIResponseGetSupportedLanguages))
                 {
                     CreateUIResponseGetSupportedLanguages();
                 }
                 else if (clickedItem.Equals(UIResponseIsReady))
                 {
                     CreateUIResponseIsReady();
                 }
                 else if (clickedItem.Equals(UIResponsePerformAudioPassThru))
                 {
                     CreateUIResponsePerformAudioPassThru();
                 }
                 else if (clickedItem.Equals(UIResponsePerformInteraction))
                 {
                     CreateUIResponsePerformInteraction();
                 }
                 else if (clickedItem.Equals(UIResponseScrollableMessage))
                 {
                     CreateUIResponseScrollableMessage();
                 }
                 else if (clickedItem.Equals(UIResponseSetAppIcon))
                 {
                     CreateUIResponseSetAppIcon();
                 }
                 else if (clickedItem.Equals(UIResponseSetDisplayLayout))
                 {
                     CreateUIResponseSetDisplayLayout();
                 }
                 else if (clickedItem.Equals(UIResponseSetGlobalProperties))
                 {
                     CreateUIResponseSetGlobalProperties();
                 }
                 else if (clickedItem.Equals(UIResponseSetMediaClockTimer))
                 {
                     CreateUIResponseSetMediaClockTimer();
                 }
                 else if (clickedItem.Equals(UIResponseShow))
                 {
                     CreateUIResponseShow();
                 }
                 else if (clickedItem.Equals(UIResponseShowCustomForm))
                 {
                     CreateUIResponseShowCustomForm();
                 }
                 else if (clickedItem.Equals(UIResponseSlider))
                 {
                     CreateUIResponseSlider();
                 }
                 else if (clickedItem.Equals(UINotificationOnCommand))
                 {
                     CreateUINotificationOnCommand();
                 }
                 else if (clickedItem.Equals(UINotificationOnDriverDistraction))
                 {
                     CreateUINotificationOnDriverDistraction();
                 }
                 else if (clickedItem.Equals(UINotificationOnKeyboardInput))
                 {
                     CreateUINotificationOnKeyboardInput();
                 }
                 else if (clickedItem.Equals(UINotificationOnLanguageChange))
                 {
                     CreateUINotificationOnLanguageChange();
                 }
                 else if (clickedItem.Equals(UINotificationOnRecordStart))
                 {
                     CreateUINotificationOnRecordStart();
                 }
                 else if (clickedItem.Equals(UINotificationOnResetTimeout))
                 {
                     OnResetTimeoutNotification();
                 }
                 else if (clickedItem.Equals(UINotificationOnSystemContext))
                 {
                     CreateUINotificationOnSystemContext();
                 }
                 else if (clickedItem.Equals(UINotificationOnTouchEvent))
                 {
                     CreateUINotificationOnTouchEvent();
                 }
                else if (clickedItem.Equals(VIResponseDiagnosticMessage))
				 {
					 CreateVIResponseDiagnosticMessage();
				 }
                else if (clickedItem.Equals(VIResponseGetDTCs))
				 {
					 CreateVIResponseGetDTCs();
				 }
                else if (clickedItem.Equals(VIResponseGetVehicleData))
				 {
                    CreateVIResponseGetVehicleData();
				 }
                else if (clickedItem.Equals(VIResponseGetVehicleType))
				 {
                    CreateVIResponseGetVehicleType();
				 }
                else if (clickedItem.Equals(VIResponseReadDID))
				 {
                    CreateVIResponseReadDID();
				 }
                else if (clickedItem.Equals(VIResponseSubscribeVehicleData))
				 {
                    CreateVIResponseSubscribeVehicleData();
				 }
                else if (clickedItem.Equals(VIResponseUnsubscribeVehicleData))
				 {
					 CreateVIResponseSubscribeVehicleData();
				 }
                else if (clickedItem.Equals("VRGetSupportedLanguageResponse"))
				 {
					 CreateVRGetSupportedLanguageResponse(); 
				 }
                				 
             };

            rpcListAlertDialog.Show();
        }

        private void CreateUIResponseAddCommand()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("AddCommand");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			HmiApiLib.Controllers.UI.OutgoingResponses.AddCommand tmpObj = new HmiApiLib.Controllers.UI.OutgoingResponses.AddCommand();
			tmpObj = (HmiApiLib.Controllers.UI.OutgoingResponses.AddCommand)AppUtils.getSavedPreferenceValueForRpc<HmiApiLib.Controllers.UI.OutgoingResponses.AddCommand>(adapter.Context, tmpObj.getMethod(), appID);
			if (tmpObj != null)
			{
				spnGeneric.SetSelection((int)tmpObj.getResultCode());
			}

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				RpcMessage rpcMessage = null;
				rpcMessage = BuildRpc.buildUiAddCommandResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition);
				AppUtils.savePreferenceValueForRpc(adapter.Context, ((RpcResponse)rpcMessage).getMethod(), rpcMessage, appID);
				AppInstanceManager.Instance.sendRpc(rpcMessage);

			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{
				AppUtils.removeSavedPreferenceValueForRpc(adapter.Context, tmpObj.getMethod(), appID);
			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseAddSubMenu()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("AddSubMenu");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			HmiApiLib.Controllers.UI.OutgoingResponses.AddSubMenu tmpObj = new HmiApiLib.Controllers.UI.OutgoingResponses.AddSubMenu();
			tmpObj = (HmiApiLib.Controllers.UI.OutgoingResponses.AddSubMenu)AppUtils.getSavedPreferenceValueForRpc<HmiApiLib.Controllers.UI.OutgoingResponses.AddSubMenu>(adapter.Context, tmpObj.getMethod(), appID);
			if (tmpObj != null)
			{
				spnGeneric.SetSelection((int)tmpObj.getResultCode());
			}

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				RpcMessage rpcMessage = BuildRpc.buildUiAddSubMenuResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition);
				AppUtils.savePreferenceValueForRpc(adapter.Context, ((RpcResponse)rpcMessage).getMethod(), rpcMessage, appID);
				AppInstanceManager.Instance.sendRpc(rpcMessage);
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{
				AppUtils.removeSavedPreferenceValueForRpc(adapter.Context, tmpObj.getMethod(), appID);
			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseAlert()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.on_exit_application, null);
			rpcAlertDialog.SetView(rpcView);

			TextView textViewApplicationId = (TextView)rpcView.FindViewById(Resource.Id.appplication_id_tv);
			EditText editTextdApplicationId = (EditText)rpcView.FindViewById(Resource.Id.appplication_id_et);

			TextView textViewAppExitReason = (TextView)rpcView.FindViewById(Resource.Id.app_exit_reason_tv);
			Spinner spnAppExitReason = (Spinner)rpcView.FindViewById(Resource.Id.app_exit_reason);

			editTextdApplicationId.Text = "2000000000";

			var appExitReasonAdapter = new ArrayAdapter<String>(Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnAppExitReason.Adapter = appExitReasonAdapter;

			HmiApiLib.Controllers.UI.OutgoingResponses.Alert tmpObj = new HmiApiLib.Controllers.UI.OutgoingResponses.Alert();
			tmpObj = (HmiApiLib.Controllers.UI.OutgoingResponses.Alert)AppUtils.getSavedPreferenceValueForRpc<HmiApiLib.Controllers.UI.OutgoingResponses.Alert>(Context, tmpObj.getMethod(), appID);
			if (tmpObj != null)
			{
				spnAppExitReason.SetSelection((int)tmpObj.getResultCode());

				if (tmpObj.getTryAgainTime() != null)
					editTextdApplicationId.Text = tmpObj.getTryAgainTime().ToString();
			}

			rpcAlertDialog.SetTitle("Alert");
			textViewApplicationId.Text = "TryAgainTime";
			textViewAppExitReason.Text = "ResultCode";

			RpcMessage rpcMessage = null;

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				if (editTextdApplicationId.Text.Equals(""))
					rpcMessage = BuildRpc.buildUiAlertResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnAppExitReason.SelectedItemPosition, 0);
				else
					rpcMessage = BuildRpc.buildUiAlertResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnAppExitReason.SelectedItemPosition, Java.Lang.Integer.ParseInt(editTextdApplicationId.Text));

				AppUtils.savePreferenceValueForRpc(Context, ((RpcResponse)rpcMessage).getMethod(), rpcMessage, appID);
				AppInstanceManager.Instance.sendRpc(rpcMessage);
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{
				AppUtils.removeSavedPreferenceValueForRpc(Context, tmpObj.getMethod(), appID);
			});
			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});
			rpcAlertDialog.Show();
        }

        private void CreateUIResponseChangeRegistration()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetTitle("ChangeRegistration");

            TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
            Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

            string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
            var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
            spnGeneric.Adapter = adapter;

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
              {
                  rpcAlertDialog.Dispose();
              });

            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
               {

                   AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiChangeRegistrationResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));

               });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateUIResponseClosePopUp()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetTitle("ClosePopUp");

            TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
            Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

            string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
            var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
            spnGeneric.Adapter = adapter;

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
	        {
	            rpcAlertDialog.Dispose();
	        });

            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {
                AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiClosePopUpResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateUIResponseDeleteCommand()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("DeleteCommand");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiDeleteCommandResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseDeleteSubMenu()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("DeleteSubMenu");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiDeleteSubMenuResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseEndAudioPassThru()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("EndAudioPassThru");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiEndAudioPassThruResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseGetCapabilities()
        {
            
        }

        private void CreateUIResponseGetLanguage()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_language, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("GetLanguage");

			TextView textViewLanguage = (TextView)rpcView.FindViewById(Resource.Id.tts_language_tv);
			textViewLanguage.Text = "Language";

			Spinner spnLanguage = (Spinner)rpcView.FindViewById(Resource.Id.tts_language_spn);

			TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.tts_result_code_tv);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.tts_result_code_spn);


			string[] language = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Language));
			var languageAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
			spnLanguage.Adapter = languageAdapter;


			string[] result = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var resultAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, result);
			spnResultCode.Adapter = resultAdapter;


			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			  {
				  AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiGetLanguageResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Language)spnLanguage.SelectedItemPosition, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition));
			  });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseGetSupportedLanguages()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View getSupportedLanguagesRpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_support_languages, null);
			rpcAlertDialog.SetView(getSupportedLanguagesRpcView);
			rpcAlertDialog.SetTitle("GetSupportedLanguages");

			TextView textViewresultCode = (TextView)getSupportedLanguagesRpcView.FindViewById(Resource.Id.result_code_tv);
			Spinner spnResultCode = (Spinner)getSupportedLanguagesRpcView.FindViewById(Resource.Id.get_supported_language_result_code_spn);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = adapter;


			ListView languagesListView = (Android.Widget.ListView)getSupportedLanguagesRpcView.FindViewById(Resource.Id.tts_language_listview);


			List<Language> languagesList = new List<Language>();

			Button languagesButton = (Button)getSupportedLanguagesRpcView.FindViewById(Resource.Id.add_tts_language_listview_btn);
			languagesButton.Click += delegate
			 {
				 AlertDialog.Builder languagesAlertDialog = new AlertDialog.Builder(this.Context);
				 View languagesView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
				 languagesAlertDialog.SetView(languagesView);
				 languagesAlertDialog.SetTitle("Language");

				 TextView textViewLanguages = (TextView)languagesView.FindViewById(Resource.Id.result_code_spinner);
				 textViewLanguages.Text = "SelectLanguage";
				 Spinner spnLanguages = (Spinner)languagesView.FindViewById(Resource.Id.genericspinner_Spinner);

				 string[] language = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Language));
				 var languageAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
				 spnLanguages.Adapter = languageAdapter;

				 languagesAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
			  {
				  languagesList.Add((HmiApiLib.Common.Enums.Language)spnLanguages.SelectedItemPosition);

			  });

				 languagesAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
			 {
				 languagesAlertDialog.Dispose();
			 });

				 languagesAlertDialog.Show();

			 };

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			  {
				  AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiGetSupportedLanguagesResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition, languagesList));
			  });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseIsReady()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.allow_device_to_Connect, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("IsReady");

			CheckBox checkBoxAllow = (CheckBox)rpcView.FindViewById(Resource.Id.allow);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spn);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.result_Code);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			 {
				 rpcAlertDialog.Dispose();
			 });

			checkBoxAllow.Text = ("Available");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildIsReadyResponse(BuildRpc.getNextId(), HmiApiLib.Types.InterfaceType.UI, checkBoxAllow.Checked, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateUIResponsePerformAudioPassThru()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("PerformAudioPassThru");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiPerformAudioPassThruResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponsePerformInteraction()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.perform_interaction_response, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("PerformInteraction");

			TextView textViewChoiceID = (TextView)rpcView.FindViewById(Resource.Id.choice_id_tv);
			EditText editTextChoiceID = (EditText)rpcView.FindViewById(Resource.Id.choice_id_et);

			TextView textViewManualTextEntry = (TextView)rpcView.FindViewById(Resource.Id.manual_text_entry_tv);
			EditText editTextManualTextEntry = (EditText)rpcView.FindViewById(Resource.Id.manual_text_entry_et);

			TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_tv);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.perform_interaction_result_code_spn);


			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			 {
				 if (editTextChoiceID.Text.Equals(""))
					 AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiPerformInteractionResponse(BuildRpc.getNextId(), 0, editTextManualTextEntry.Text, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition));
				 else
					 AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiPerformInteractionResponse(BuildRpc.getNextId(), Java.Lang.Integer.ParseInt(editTextChoiceID.Text), editTextManualTextEntry.Text, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition));
			 });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseScrollableMessage()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("ScrollableMessage");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			var adapter = new ArrayAdapter<String>(Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;
			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiScrollableMessageResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseSetAppIcon()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("SetAppIcon");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			var adapter = new ArrayAdapter<String>(Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
			    rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiSetAppIconResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseSetDisplayLayout()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.set_display_layout, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("SetDisplayLayout");

			DisplayCapabilities dspCap = new DisplayCapabilities();

			Button addDisplayCapabilitiesButton = (Button)rpcView.FindViewById(Resource.Id.set_display_capabilities_add_display_capabilities_btn);

			addDisplayCapabilitiesButton.Click += delegate
		 {
			 AlertDialog.Builder displayCapabilitiesAlertDialog = new AlertDialog.Builder(this.Context);
			 View displayCapabilitiesView = (View)layoutIinflater.Inflate(Resource.Layout.display_capabilities, null);
			 displayCapabilitiesAlertDialog.SetView(displayCapabilitiesView);
			 displayCapabilitiesAlertDialog.SetTitle("DisplayCapabilities");

			 CheckBox checkBoxDisplayType = (CheckBox)displayCapabilitiesView.FindViewById(Resource.Id.display_type_checkbox);

			 Spinner spnButtonDisplayType = (Spinner)displayCapabilitiesView.FindViewById(Resource.Id.display_type_spinner);
			 string[] displayType = Enum.GetNames(typeof(HmiApiLib.Common.Enums.DisplayType));
			 var displayTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, displayType);
			 spnButtonDisplayType.Adapter = displayTypeAdapter;


			 List<TextField> textFieldsList = new List<TextField>();
			 ListView textFieldsListView = (Android.Widget.ListView)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_text_fields_listview);
			 Button textFieldsButton = (Button)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_add_text_fields_btn);

			 textFieldsButton.Click += delegate
				 {
					 AlertDialog.Builder textFieldsAlertDialog = new AlertDialog.Builder(this.Context);
					 View textFieldsView = (View)layoutIinflater.Inflate(Resource.Layout.text_field, null);
					 textFieldsAlertDialog.SetView(textFieldsView);
					 textFieldsAlertDialog.SetTitle("TextField");

					 CheckBox checkBoxName = (CheckBox)textFieldsView.FindViewById(Resource.Id.text_field_name_checkbox);

					 Spinner spnName = (Spinner)textFieldsView.FindViewById(Resource.Id.text_field_name_spinner);
					 string[] textFieldNames = Enum.GetNames(typeof(HmiApiLib.Common.Enums.TextFieldName));
					 var textFieldNamesAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, textFieldNames);
					 spnName.Adapter = textFieldNamesAdapter;


					 CheckBox checkBoxCharacterSet = (CheckBox)textFieldsView.FindViewById(Resource.Id.text_field_characterSet_checkbox);

					 Spinner spnCharacterSet = (Spinner)textFieldsView.FindViewById(Resource.Id.text_field_characterSet_spinner);
					 string[] characterSet = Enum.GetNames(typeof(HmiApiLib.Common.Enums.CharacterSet));
					 var characterSetAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, characterSet);
					 spnCharacterSet.Adapter = characterSetAdapter;

					 CheckBox checkBoxWidth = (CheckBox)textFieldsView.FindViewById(Resource.Id.text_field_width_checkbox);
					 EditText editTextWidth = (EditText)textFieldsView.FindViewById(Resource.Id.text_field_width_edittext);

					 CheckBox checkBoxRow = (CheckBox)textFieldsView.FindViewById(Resource.Id.text_field_row_checkbox);
					 EditText editTextRow = (EditText)textFieldsView.FindViewById(Resource.Id.text_field_row_edittext);


					 textFieldsAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
				   {

					   TextField txtField = new TextField();

					   txtField.name = (HmiApiLib.Common.Enums.TextFieldName)spnName.SelectedItemPosition;
					   txtField.characterSet = (HmiApiLib.Common.Enums.CharacterSet)spnCharacterSet.SelectedItemPosition;

					   if (editTextWidth.Text.Equals(""))
						   txtField.width = 0;
					   else
						   txtField.width = Java.Lang.Integer.ParseInt(editTextWidth.Text);

					   if (editTextRow.Text.Equals(""))
						   txtField.rows = 0;
					   else
						   txtField.rows = Java.Lang.Integer.ParseInt(editTextRow.Text);

					   textFieldsList.Add(txtField);

				   });

					 textFieldsAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
					 {
						 textFieldsAlertDialog.Dispose();
					 });
					 textFieldsAlertDialog.Show();

				 };





			 List<ImageField> imageFieldsList = new List<ImageField>();
			 ListView imageFieldsListView = (Android.Widget.ListView)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_image_fields_listview);
			 Button imageFieldsButton = (Button)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_add_image_fields_btn);

			 imageFieldsButton.Click += delegate
				  {
					  AlertDialog.Builder imageFieldsAlertDialog = new AlertDialog.Builder(this.Context);
					  View imageFieldsView = (View)layoutIinflater.Inflate(Resource.Layout.image_field, null);
					  imageFieldsAlertDialog.SetView(imageFieldsView);
					  imageFieldsAlertDialog.SetTitle("ImageField");

					  CheckBox checkBoxName = (CheckBox)imageFieldsView.FindViewById(Resource.Id.image_field_name_checkbox);

					  Spinner spnName = (Spinner)imageFieldsView.FindViewById(Resource.Id.image_field_name_spinner);
					  string[] imageFieldNames = Enum.GetNames(typeof(HmiApiLib.Common.Enums.ImageFieldName));
					  var imageFieldNamesAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, imageFieldNames);
					  spnName.Adapter = imageFieldNamesAdapter;


					  List<FileType> fileTypeList = new List<FileType>();
					  ListView fileTypeListView = (Android.Widget.ListView)imageFieldsView.FindViewById(Resource.Id.image_field_image_type_supported_listview);

					  string[] fileTypes = Enum.GetNames(typeof(HmiApiLib.Common.Enums.FileType));
					  bool[] fileTypeBoolArray = new bool[fileTypes.Length];

					  Button fileTypeButton = (Button)imageFieldsView.FindViewById(Resource.Id.image_field_image_type_supported_btn);

					  fileTypeButton.Click += (sender, e1) =>
					   {
							 AlertDialog.Builder fileTypeAlertDialog = new AlertDialog.Builder(rpcAlertDialog.Context);
							 fileTypeAlertDialog.SetTitle("FileType");

							 fileTypeAlertDialog.SetMultiChoiceItems(fileTypes, fileTypeBoolArray, (object sender1, Android.Content.DialogMultiChoiceClickEventArgs e) => fileTypeBoolArray[e.Which] = e.IsChecked);

							 fileTypeAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
						   {
							   fileTypeAlertDialog.Dispose();
						   });

							 fileTypeAlertDialog.SetPositiveButton("Add", (senderAlert, args) =>
							  {


							  });

							 fileTypeAlertDialog.Show();
						 };




					  CheckBox checkBoxResolutionWidth = (CheckBox)imageFieldsView.FindViewById(Resource.Id.image_field_resolution_width_checkbox);
					  EditText editTextResolutionWidth = (EditText)imageFieldsView.FindViewById(Resource.Id.image_field_resolution_width_edit_text);

					  CheckBox checkBoxResolutionHeight = (CheckBox)imageFieldsView.FindViewById(Resource.Id.image_field_resolution_height_checkbox);
					  EditText editTextResolutionHeight = (EditText)imageFieldsView.FindViewById(Resource.Id.image_field_resolution_height_edit_text);


					  imageFieldsAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
						{

					   ImageField imgField = new ImageField();



					   imgField.name = (HmiApiLib.Common.Enums.ImageFieldName)spnName.SelectedItemPosition;


					   for (int i = 0; i < fileTypes.Length; i++)
					   {
						   if (fileTypeBoolArray[i])
						   {
							   fileTypeList.Add(((FileType)typeof(FileType).GetEnumValues().GetValue(i)));
						   }
					   }
					   imgField.imageTypeSupported = fileTypeList;




					   ImageResolution resolution = new ImageResolution();

					   if (editTextResolutionWidth.Text.Equals(""))
						   resolution.resolutionWidth = 0;
					   else
						   resolution.resolutionWidth = Java.Lang.Integer.ParseInt(editTextResolutionWidth.Text);

					   if (editTextResolutionHeight.Text.Equals(""))
						   resolution.resolutionHeight = 0;
					   else
						   resolution.resolutionHeight = Java.Lang.Integer.ParseInt(editTextResolutionHeight.Text);

					   imgField.imageResolution = resolution;



					   imageFieldsList.Add(imgField);

				   });

					  imageFieldsAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
					 {
						 imageFieldsAlertDialog.Dispose();
					 });
					  imageFieldsAlertDialog.Show();

				  };





			 List<MediaClockFormat> mediaClockFormatsList = new List<MediaClockFormat>();
			 ListView mediaClockFormatsListView = (Android.Widget.ListView)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_media_clock_formats_listview);


			 string[] mediaClockFormats = Enum.GetNames(typeof(HmiApiLib.Common.Enums.MediaClockFormat));
			 bool[] mediaClockFormatsBoolArray = new bool[mediaClockFormats.Length];

			 Button mediaClockFormatsButton = (Button)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_add_media_clock_formats_btn);

			 mediaClockFormatsButton.Click += (sender, e1) =>
				  {
					  AlertDialog.Builder mediaClockFormatsAlertDialog = new AlertDialog.Builder(rpcAlertDialog.Context);
					  mediaClockFormatsAlertDialog.SetTitle("MediaClockFormats");

					  mediaClockFormatsAlertDialog.SetMultiChoiceItems(mediaClockFormats, mediaClockFormatsBoolArray, (object sender1, Android.Content.DialogMultiChoiceClickEventArgs e) => mediaClockFormatsBoolArray[e.Which] = e.IsChecked);

					  mediaClockFormatsAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
							   {
						   mediaClockFormatsAlertDialog.Dispose();
					   });

					  mediaClockFormatsAlertDialog.SetPositiveButton("Add", (senderAlert, args) =>
								{


					  });

					  mediaClockFormatsAlertDialog.Show();
				  };



			 List<ImageType> imageCapabilitiesList = new List<ImageType>();
			 ListView imageCapabilitiesListView = (Android.Widget.ListView)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_image_types_listview);

			 string[] imageCapabilities = Enum.GetNames(typeof(HmiApiLib.Common.Enums.ImageType));
			 bool[] imageCapabilitiesBoolArray = new bool[imageCapabilities.Length];


			 Button imageCapabilitiesButton = (Button)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_add_image_types_btn);

			 imageCapabilitiesButton.Click += (sender, e1) =>
						  {
							  AlertDialog.Builder imageCapabilitiesAlertDialog = new AlertDialog.Builder(rpcAlertDialog.Context);
							  imageCapabilitiesAlertDialog.SetTitle("ImageCapabilities");

							  imageCapabilitiesAlertDialog.SetMultiChoiceItems(imageCapabilities, imageCapabilitiesBoolArray, (object sender1, Android.Content.DialogMultiChoiceClickEventArgs e) => imageCapabilitiesBoolArray[e.Which] = e.IsChecked);

							  imageCapabilitiesAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
							   {
								   imageCapabilitiesAlertDialog.Dispose();
							   });

							  imageCapabilitiesAlertDialog.SetPositiveButton("Add", (senderAlert, args) =>
								{


							  });

							  imageCapabilitiesAlertDialog.Show();
						  };





			 CheckBox checkBoxGraphicSupported = (CheckBox)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_graphic_supported_checkbox);


			 CheckBox checkBoxTemplatesAvailable = (CheckBox)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_templates_available_checkbox);
			 EditText editTextTemplatesAvailable = (EditText)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_templates_available_edittext);

			 ScreenParams scrnParam = new ScreenParams();
			 ListView screenParamsListView = (Android.Widget.ListView)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_screen_params_listview);
			 Button screenParamsButton = (Button)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_add_screen_params_btn);



			 screenParamsButton.Click += delegate
				 {
					 AlertDialog.Builder screenParamsAlertDialog = new AlertDialog.Builder(this.Context);
					 View screenParamsView = (View)layoutIinflater.Inflate(Resource.Layout.screen_param, null);
					 screenParamsAlertDialog.SetView(screenParamsView);
					 screenParamsAlertDialog.SetTitle("ScreenParams");


					 CheckBox checkBoxResolutionWidth = (CheckBox)screenParamsView.FindViewById(Resource.Id.screen_param_resolution_width_checkbox);
					 EditText editTextResolutionWidth = (EditText)screenParamsView.FindViewById(Resource.Id.screen_param_resolution_width_edit_text);

					 CheckBox checkBoxResolutionHeight = (CheckBox)screenParamsView.FindViewById(Resource.Id.screen_param_resolution_height_checkbox);
					 EditText editTextResolutionHeight = (EditText)screenParamsView.FindViewById(Resource.Id.screen_param_resolution_height_edit_text);

					 CheckBox checkBoxpressAvailable = (CheckBox)screenParamsView.FindViewById(Resource.Id.screen_param_press_available_checkbox);
					 CheckBox checkBoxMultiTouchAvailable = (CheckBox)screenParamsView.FindViewById(Resource.Id.screen_param_multi_touch_available_checkbox);
					 CheckBox checkBoxDoubleTouchAvailable = (CheckBox)screenParamsView.FindViewById(Resource.Id.screen_param_double_press_available_checkbox);

					 screenParamsAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
				   {


						//ScreenParams scrnParam = new ScreenParams();

						ImageResolution imgResolution = new ImageResolution();

					  if (editTextResolutionWidth.Text.Equals(""))
						  imgResolution.resolutionWidth = 0;
					  else
						  imgResolution.resolutionWidth = Java.Lang.Integer.ParseInt(editTextResolutionWidth.Text);

					  if (editTextResolutionHeight.Text.Equals(""))
						  imgResolution.resolutionHeight = 0;
					  else
						  imgResolution.resolutionHeight = Java.Lang.Integer.ParseInt(editTextResolutionHeight.Text);


					  scrnParam.resolution = imgResolution;




					  TouchEventCapabilities touchEventCapabilities = new TouchEventCapabilities();

					  touchEventCapabilities.pressAvailable = checkBoxpressAvailable.Checked;
					  touchEventCapabilities.pressAvailable = checkBoxpressAvailable.Checked;
					  touchEventCapabilities.pressAvailable = checkBoxpressAvailable.Checked;

					  scrnParam.touchEventAvailable = touchEventCapabilities;


				  });

					 screenParamsAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
					 {
						screenParamsAlertDialog.Dispose();
					});
					 screenParamsAlertDialog.Show();

				 };





			 CheckBox checkBoxNumCustomPresetsAvailable = (CheckBox)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_num_custom_presets_available_checkbox);
			 EditText editTextNumCustomPresetsAvailable = (EditText)displayCapabilitiesView.FindViewById(Resource.Id.display_capabilities_num_custom_presets_available_edittext);




			 displayCapabilitiesAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
		   {

				 //DisplayCapabilities dspCap = new DisplayCapabilities();
				 dspCap.displayType = (HmiApiLib.Common.Enums.DisplayType)spnButtonDisplayType.SelectedItemPosition;


			   dspCap.textFields = textFieldsList;
			   dspCap.imageFields = imageFieldsList;


			   for (int i = 0; i < mediaClockFormats.Length; i++)
			   {
				   if (mediaClockFormatsBoolArray[i])
				   {
					   mediaClockFormatsList.Add(((MediaClockFormat)typeof(MediaClockFormat).GetEnumValues().GetValue(i)));
				   }
			   }
			   dspCap.mediaClockFormats = mediaClockFormatsList;




			   for (int i = 0; i < imageCapabilities.Length; i++)
			   {
				   if (imageCapabilitiesBoolArray[i])
				   {
					   imageCapabilitiesList.Add(((ImageType)typeof(ImageType).GetEnumValues().GetValue(i)));
				   }
			   }
			   dspCap.imageCapabilities = imageCapabilitiesList;




			   if (checkBoxGraphicSupported.Checked)
				   dspCap.graphicSupported = true;
			   else
				   dspCap.graphicSupported = false;

			   List<String> templatesAvailable = new List<string>();
			   templatesAvailable.AddRange(editTextTemplatesAvailable.Text.Split(','));
			   dspCap.templatesAvailable = templatesAvailable;




			   dspCap.screenParams = scrnParam;




			   if (editTextNumCustomPresetsAvailable.Text.Equals(""))
				   dspCap.numCustomPresetsAvailable = 0;
			   else
				   dspCap.numCustomPresetsAvailable = Java.Lang.Integer.ParseInt(editTextNumCustomPresetsAvailable.Text);


				 //displayCapabilitiesList.Add(dspCap);

			 });

			 displayCapabilitiesAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
			{
				displayCapabilitiesAlertDialog.Dispose();
			});
			 displayCapabilitiesAlertDialog.Show();

		 };


			List<ButtonCapabilities> btnCapList = new List<ButtonCapabilities>();

			Button addButtonCapabilitiesButton = (Button)rpcView.FindViewById(Resource.Id.set_display_capabilities_add_button_capabilities_btn);

			addButtonCapabilitiesButton.Click += delegate
			{
				AlertDialog.Builder btnCapabilitiesAlertDialog = new AlertDialog.Builder(this.Context);
				View btnCapabilitiesView = (View)layoutIinflater.Inflate(Resource.Layout.button_capabilities, null);
				btnCapabilitiesAlertDialog.SetView(btnCapabilitiesView);
				btnCapabilitiesAlertDialog.SetTitle("ButtonCapabilities");

				TextView textViewButtonName = (TextView)btnCapabilitiesView.FindViewById(Resource.Id.get_capabilities_button_name_tv);

				Spinner spnButtonNames = (Spinner)btnCapabilitiesView.FindViewById(Resource.Id.get_capabilities_button_name_spn);
				string[] btnCapabilitiesButtonName = Enum.GetNames(typeof(HmiApiLib.ButtonName));
				var btnCapabilitiesButtonNameAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, btnCapabilitiesButtonName);
				spnButtonNames.Adapter = btnCapabilitiesButtonNameAdapter;


				CheckBox checkBoxShortPressAvailable = (CheckBox)btnCapabilitiesView.FindViewById(Resource.Id.short_press_available_cb);

				CheckBox checkBoxLongPressAvailable = (CheckBox)btnCapabilitiesView.FindViewById(Resource.Id.long_press_available_cb);

				CheckBox checkBoxUpDownAvailable = (CheckBox)btnCapabilitiesView.FindViewById(Resource.Id.up_down_available_cb);


				btnCapabilitiesAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
			  {

				   ButtonCapabilities btn = new ButtonCapabilities();
				   btn.name = (HmiApiLib.ButtonName)spnButtonNames.SelectedItemPosition;

				   if (checkBoxShortPressAvailable.Checked)
					   btn.shortPressAvailable = true;
				   else
					   btn.shortPressAvailable = false;

				   if (checkBoxLongPressAvailable.Checked)
					   btn.longPressAvailable = true;
				   else
					   btn.longPressAvailable = false;

				   if (checkBoxUpDownAvailable.Checked)
					   btn.upDownAvailable = true;
				   else
					   btn.upDownAvailable = false;

				   btnCapList.Add(btn);

			   });

				btnCapabilitiesAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
			 {
					 btnCapabilitiesAlertDialog.Dispose();
				 });
				btnCapabilitiesAlertDialog.Show();

			  //  var namesAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonNames);

			  //  spnButtonNames.Adapter = namesAdapter;
		  };



			List<SoftButtonCapabilities> btnSoftButtonCapList = new List<SoftButtonCapabilities>();

			Button addSoftButtonCapabilitiesButton = (Button)rpcView.FindViewById(Resource.Id.set_display_capabilities_add_soft_button_capabilities_btn);

			addSoftButtonCapabilitiesButton.Click += delegate
			 {
				 AlertDialog.Builder SoftButtonCapabilitiesAlertDialog = new AlertDialog.Builder(this.Context);
				 View SoftButtonCapabilitiesView = (View)layoutIinflater.Inflate(Resource.Layout.soft_button_capabilities, null);
				 SoftButtonCapabilitiesAlertDialog.SetView(SoftButtonCapabilitiesView);
				 SoftButtonCapabilitiesAlertDialog.SetTitle("SoftButtonCapabilities");

				 CheckBox checkBoxShortPressAvailable = (CheckBox)SoftButtonCapabilitiesView.FindViewById(Resource.Id.soft_button_capabilities_short_press_available_checkbox);

				 CheckBox checkBoxLongPressAvailable = (CheckBox)SoftButtonCapabilitiesView.FindViewById(Resource.Id.soft_button_capabilities_long_press_available_checkbox);

				 CheckBox checkBoxUpDownAvailable = (CheckBox)SoftButtonCapabilitiesView.FindViewById(Resource.Id.soft_button_capabilities_up_down_available_checkbox);

				 CheckBox checkBoxImageSupported = (CheckBox)SoftButtonCapabilitiesView.FindViewById(Resource.Id.soft_button_capabilities_image_supported_checkbox);


				 SoftButtonCapabilitiesAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
				 {

					 SoftButtonCapabilities btn = new SoftButtonCapabilities();

					 if (checkBoxShortPressAvailable.Checked)
						 btn.shortPressAvailable = true;
					 else
						 btn.shortPressAvailable = false;

					 if (checkBoxLongPressAvailable.Checked)
						 btn.longPressAvailable = true;
					 else
						 btn.longPressAvailable = false;

					 if (checkBoxUpDownAvailable.Checked)
						 btn.upDownAvailable = true;
					 else
						 btn.upDownAvailable = false;

					 if (checkBoxImageSupported.Checked)
						 btn.imageSupported = true;
					 else
						 btn.imageSupported = false;

					 btnSoftButtonCapList.Add(btn);

				 });

				 SoftButtonCapabilitiesAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
			  {
					   SoftButtonCapabilitiesAlertDialog.Dispose();
				   });
				 SoftButtonCapabilitiesAlertDialog.Show();

			 };


			CheckBox checkBoxOnScreenPresetsAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.set_display_capabilities_preset_bank_capabilities_cb);

			PresetBankCapabilities prstCap = new PresetBankCapabilities();
			prstCap.onScreenPresetsAvailable = checkBoxOnScreenPresetsAvailable.Checked;


			TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.set_display_capabilities_result_code_tv);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.set_display_capabilities_result_code_spn);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;



			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			 {
				 rpcAlertDialog.Dispose();
			 });

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			  {

				  AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiSetDisplayLayoutResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition, dspCap, btnCapList, btnSoftButtonCapList, prstCap));
			  });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});
			rpcAlertDialog.Show();
        }

        private void CreateUIResponseSetGlobalProperties()
        {
            // yet to be implemented
        }

        void CreateUIResponseSetMediaClockTimer()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetTitle("SetMediaClockTimer");

            TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
            Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

            string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
            var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
            spnGeneric.Adapter = adapter;

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {
                AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiSetMediaClockTimerResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateUIResponseShow()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("Show");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});
			HmiApiLib.Controllers.UI.OutgoingResponses.Show tmpObj = new HmiApiLib.Controllers.UI.OutgoingResponses.Show();
			tmpObj = (HmiApiLib.Controllers.UI.OutgoingResponses.Show)AppUtils.getSavedPreferenceValueForRpc<HmiApiLib.Controllers.UI.OutgoingResponses.Show>(adapter.Context, tmpObj.getMethod(), appID);
			if (tmpObj != null)
			{
				spnGeneric.SetSelection((int)tmpObj.getResultCode());
			}

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				RpcMessage rpcMessage = null;
				rpcMessage = BuildRpc.buildUiShowResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition);

				AppUtils.savePreferenceValueForRpc(adapter.Context, ((RpcResponse)rpcMessage).getMethod(), rpcMessage, appID);
				AppInstanceManager.Instance.sendRpc(rpcMessage);
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{
				AppUtils.removeSavedPreferenceValueForRpc(adapter.Context, tmpObj.getMethod(), appID);
			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseShowCustomForm()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.on_exit_application, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("ShowCustomForm");

			TextView textViewInfo = (TextView)rpcView.FindViewById(Resource.Id.appplication_id_tv);
			textViewInfo.Text = "Info";
			EditText editTextdInfo = (EditText)rpcView.FindViewById(Resource.Id.appplication_id_et);
			editTextdInfo.InputType = Android.Text.InputTypes.ClassText;

			TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.app_exit_reason_tv);
			textViewResultCode.Text = "ResultCode";

			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.app_exit_reason);
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
			    rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiShowCustomFormResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition, textViewInfo.Text));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{
                
			});

			rpcAlertDialog.Show();
        }

        private void CreateUIResponseSlider()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.slider_response, null);
            rpcAlertDialog.SetView(rpcView);

            CheckBox slider_position_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.slider_position_checkbox);
            EditText slider_position_edittext = (EditText)rpcView.FindViewById(Resource.Id.slider_position_et);

            CheckBox slider_result_code_spinner = (CheckBox)rpcView.FindViewById(Resource.Id.slider_result_code_checkbox);
            Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.slider_result_code_spinner);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("Slider");
            slider_position_checkbox.Text = "Slider Position";
            var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
            spnResultCode.Adapter = resultCodeAdapter;


            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {
                slider_position_edittext.Text = "1";
                spnResultCode.SetSelection(0);
            });

            rpcAlertDialog.Show();
        }

        private void CreateUINotificationOnCommand()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_command_notification, null);
            rpcAlertDialog.SetView(rpcView);
            rpcAlertDialog.SetTitle("On Command");

            List<int?> cmdIDList = new List<int?>();
            cmdIDList.AddRange(AppInstanceManager.commandIdList[appID]);

            CheckBox appIdCheck = (CheckBox)rpcView.FindViewById(Resource.Id.on_command_app_id_check);
            Spinner spnAppId = (Spinner)rpcView.FindViewById(Resource.Id.on_command_app_id_spinner);

            CheckBox cmdIdCheck = (CheckBox)rpcView.FindViewById(Resource.Id.on_command_cmd_id_check);
            Spinner spnCmdId = (Spinner)rpcView.FindViewById(Resource.Id.on_command_cmd_id_spinner);

            var cmdIdAdapter = new ArrayAdapter<int?>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, cmdIDList);
            spnCmdId.Adapter = cmdIdAdapter;

            List<int> appIdList = new List<int>();
            foreach (AppItem item in AppInstanceManager.appList)
            {
                appIdList.Add(item.getAppID());
            }
            var appIdAdapter = new ArrayAdapter<int>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appIdList);
            spnAppId.Adapter = appIdAdapter;

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateUINotificationOnDriverDistraction()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, driverDistractionState);
			spnGeneric.Adapter = adapter1;

			rsltCode.Text = "Driver Distraction State";

			rpcAlertDialog.SetTitle("Driver Distraction");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});
			rpcAlertDialog.Show();
        }

        private void CreateUINotificationOnKeyboardInput()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.slider_response, null);
			rpcAlertDialog.SetView(rpcView);

			CheckBox slider_position_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.slider_position_checkbox);
			EditText slider_position_edittext = (EditText)rpcView.FindViewById(Resource.Id.slider_position_et);

			CheckBox slider_result_code_spinner = (CheckBox)rpcView.FindViewById(Resource.Id.slider_result_code_checkbox);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.slider_result_code_spinner);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			slider_position_checkbox.Text = "Data";
			var keyBoardEventAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, keyBoardEvent);
			spnResultCode.Adapter = keyBoardEventAdapter;

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUINotificationOnLanguageChange()
        {
            
        }

        private void CreateUINotificationOnRecordStart()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			List<int> appIdList = new List<int>();
			foreach (AppItem item in AppInstanceManager.appList)
			{
				appIdList.Add(item.getAppID());
			}
			var adapter1 = new ArrayAdapter<int>(Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appIdList);
			spnGeneric.Adapter = adapter1;

			rsltCode.Text = "App ID";

			rpcAlertDialog.SetTitle("Record Notification");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});
			rpcAlertDialog.Show();
        }

        private void CreateUINotificationOnSystemContext()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.on_command_notification, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("OnSystemContext Notification");

			CheckBox appIdCheck = (CheckBox)rpcView.FindViewById(Resource.Id.on_command_app_id_check);
			Spinner spnAppId = (Spinner)rpcView.FindViewById(Resource.Id.on_command_app_id_spinner);

			CheckBox cmdIdCheck = (CheckBox)rpcView.FindViewById(Resource.Id.on_command_cmd_id_check);
			Spinner spnSystemContext = (Spinner)rpcView.FindViewById(Resource.Id.on_command_cmd_id_spinner);
			cmdIdCheck.Text = "System Context";

			string[] systemContext = Enum.GetNames(typeof(SystemContext));
			var systemContextAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, systemContext);
			spnSystemContext.Adapter = systemContextAdapter;

			List<int> appIdList = new List<int>();
			foreach (AppItem item in AppInstanceManager.appList)
			{
				appIdList.Add(item.getAppID());
			}
			var appIdAdapter = new ArrayAdapter<int>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appIdList);
			spnAppId.Adapter = appIdAdapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateUINotificationOnTouchEvent()
        {
			List<TouchEvent> touchEvents = new List<TouchEvent>();
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.on_touch_event_notification, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("OnTouchEvent Notification");
			rpcAlertDialog.SetCancelable(false);


			CheckBox touchTypeCheck = (CheckBox)rpcView.FindViewById(Resource.Id.on_touch_event_touch_type_checkbox);
			Spinner spnTouchType = (Spinner)rpcView.FindViewById(Resource.Id.on_touch_event_touch_type_spinner);

			string[] touchType = Enum.GetNames(typeof(TouchType));
			var touchTypeAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, touchType);
			spnTouchType.Adapter = touchTypeAdapter;

			ListView listViewTouchEvent = (ListView)rpcView.FindViewById(Resource.Id.touch_event_listview);

			var touchEventAdapter = new ArrayAdapter<TouchEvent>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, touchEvents);
			listViewTouchEvent.Adapter = touchEventAdapter;

			Button createTouchEvent = (Button)rpcView.FindViewById(Resource.Id.on_touch_event_create_touch_event);
			createTouchEvent.Click += (sender, e) =>
			{
				List<TouchCoord> touchCoordList = new List<TouchCoord>();
				AlertDialog.Builder touchEventAlertDialog = new AlertDialog.Builder(rpcAlertDialog.Context);
				View touchEventView = layoutIinflater.Inflate(Resource.Layout.touch_event, null);
				touchEventAlertDialog.SetView(touchEventView);
				touchEventAlertDialog.SetTitle("Touch Event");

				CheckBox touchEventIdCheckbox = (CheckBox)touchEventView.FindViewById(Resource.Id.touch_event_id_checkbox);
				EditText touchEventIdEditText = (EditText)touchEventView.FindViewById(Resource.Id.touch_event_id_edit_text);
				CheckBox touchEventTsCheckbox = (CheckBox)touchEventView.FindViewById(Resource.Id.touch_event_ts_checkbox);
				EditText touchEventTsEditText = (EditText)touchEventView.FindViewById(Resource.Id.touch_event_ts_edittext);
				CheckBox touchEventCordCheckbox = (CheckBox)touchEventView.FindViewById(Resource.Id.touch_event_cord_checkbox);
				ListView touchCordListView = (ListView)touchEventView.FindViewById(Resource.Id.touch_cord_list_view);
				Button createTouchCordButton = (Button)touchEventView.FindViewById(Resource.Id.create_touch_cord_button);

				var touchCoordAdapter = new ArrayAdapter<TouchCoord>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, touchCoordList);
				touchCordListView.Adapter = touchCoordAdapter;

				createTouchCordButton.Click += (sender1, e1) =>
				{
					AlertDialog.Builder touchCoordAlertDialog = new AlertDialog.Builder(rpcAlertDialog.Context);
					View touchCoordView = layoutIinflater.Inflate(Resource.Layout.touch_cord, null);
					touchCoordAlertDialog.SetView(touchCoordView);
					touchCoordAlertDialog.SetTitle("Touch Coord");

					CheckBox xCheckBox = (CheckBox)touchCoordView.FindViewById(Resource.Id.touch_cord_x_checkbox);
					EditText xEditText = (EditText)touchCoordView.FindViewById(Resource.Id.touch_cord_x_edittext);
					CheckBox yCheckBox = (CheckBox)touchCoordView.FindViewById(Resource.Id.touch_cord_y_checkbox);
					EditText yEditText = (EditText)touchCoordView.FindViewById(Resource.Id.touch_cord_y_edittext);


					touchCoordAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					{
						touchCoordAlertDialog.Dispose();
					});

					touchCoordAlertDialog.SetPositiveButton("Add", (senderAlert, args) =>
					{
						TouchCoord coord = new TouchCoord();
						try
						{
							coord.x = Int32.Parse(xEditText.Text.ToString());
							coord.y = Int32.Parse(yEditText.Text.ToString());
						}
						catch (Exception e11)
						{

						}
						touchCoordList.Add(coord);
						touchCoordAdapter.NotifyDataSetChanged();
					});

					touchCoordAlertDialog.Show();
				};


				touchEventAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
				{
					touchEventAlertDialog.Dispose();
				});

				touchEventAlertDialog.SetPositiveButton("Add", (senderAlert, args) =>
				{
					TouchEvent touchEvent = new TouchEvent();
					try
					{
						touchEvent.id = Int32.Parse(touchEventIdEditText.Text.ToString());
					}
					catch (Exception e2)
					{
						touchEvent.id = 0;
					}
					List<int> tsList = new List<int>();
					string[] t = touchEventTsEditText.Text.Split(',');
					foreach (string ts in t)
					{
						try
						{
							tsList.Add(Int32.Parse(ts));
						}
						catch (Exception e3)
						{

						}
					}

					touchEvent.ts = tsList;
					touchEvent.c = touchCoordList;

					touchEvents.Add(touchEvent);
					touchEventAdapter.NotifyDataSetChanged();
				});

				touchEventAlertDialog.Show();
			};

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateTTSResponseChangeRegistration()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("ChangeRegistration");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

				AppInstanceManager.Instance.sendRpc(BuildRpc.buildTTSChangeRegistrationResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));

			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateTTSResponseGetCapabilities()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.tts_get_capabilities, null);
            rpcAlertDialog.SetView(rpcView);
            rpcAlertDialog.SetTitle("GetCapabilities");


            List<SpeechCapabilities> speechCapabilitiesList = new List<SpeechCapabilities>();

            ListView ListViewSpeechCapabilities = (ListView)rpcView.FindViewById(Resource.Id.speech_capabilities_lv);

            Button speechCapabilitiesButton = (Button)rpcView.FindViewById(Resource.Id.speech_capabilities_btn);
            speechCapabilitiesButton.Click += delegate
            {
                AlertDialog.Builder speechCapabilitiesAlertDialog = new AlertDialog.Builder(this.Context);
                View speechCapabilitiesView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
                speechCapabilitiesAlertDialog.SetView(speechCapabilitiesView);
                speechCapabilitiesAlertDialog.SetTitle("SpeechCapabilities");


                TextView textViewSpeechCapabilities = (TextView)speechCapabilitiesView.FindViewById(Resource.Id.result_code_spinner);
                textViewSpeechCapabilities.Text = "SpeechCapabilities";
                Spinner spnSpeechCapabilities = (Spinner)speechCapabilitiesView.FindViewById(Resource.Id.genericspinner_Spinner);

                string[] speechCapabilities = Enum.GetNames(typeof(HmiApiLib.Common.Enums.SpeechCapabilities));
                var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, speechCapabilities);
                spnSpeechCapabilities.Adapter = adapter;


                speechCapabilitiesAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
               {

                   speechCapabilitiesList.Add((HmiApiLib.Common.Enums.SpeechCapabilities)spnSpeechCapabilities.SelectedItemPosition);

               });

                speechCapabilitiesAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
               {
                   speechCapabilitiesAlertDialog.Dispose();
               });

                speechCapabilitiesAlertDialog.Show();

            };



            List<PrerecordedSpeech> prerecordedSpeechList = new List<PrerecordedSpeech>();

            ListView ListViewPrerecordedSpeech = (ListView)rpcView.FindViewById(Resource.Id.prerecorded_speech_lv);

            Button prerecordedSpeechButton = (Button)rpcView.FindViewById(Resource.Id.prerecorded_speech_btn);
            prerecordedSpeechButton.Click += delegate
            {
                AlertDialog.Builder prerecordedSpeechAlertDialog = new AlertDialog.Builder(this.Context);
                View prerecordedSpeechView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
                prerecordedSpeechAlertDialog.SetView(prerecordedSpeechView);
                prerecordedSpeechAlertDialog.SetTitle("PrerecordedSpeech");

                TextView textViewConsentSource = (TextView)prerecordedSpeechView.FindViewById(Resource.Id.result_code_spinner);
                textViewConsentSource.Text = "PrerecordedSpeech";
                Spinner spnPrerecordedSpeech = (Spinner)prerecordedSpeechView.FindViewById(Resource.Id.genericspinner_Spinner);

                string[] prerecordedSpeech = Enum.GetNames(typeof(HmiApiLib.Common.Enums.PrerecordedSpeech));
                var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, prerecordedSpeech);
                spnPrerecordedSpeech.Adapter = adapter;


                prerecordedSpeechAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
               {

                   prerecordedSpeechList.Add((HmiApiLib.Common.Enums.PrerecordedSpeech)spnPrerecordedSpeech.SelectedItemPosition);

               });

                prerecordedSpeechAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
               {
                   prerecordedSpeechAlertDialog.Dispose();
               });

                prerecordedSpeechAlertDialog.Show();

            };


            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {
                //Method currently not present in buildRpc
            });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
           {

           });

            rpcAlertDialog.Show();
        }

        private void CreateTTSResponseGetLanguage()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_language, null);
            rpcAlertDialog.SetView(rpcView);
            rpcAlertDialog.SetTitle("GetLanguage");

            TextView textViewLanguage = (TextView)rpcView.FindViewById(Resource.Id.tts_language_tv);
            textViewLanguage.Text = "Language";

            Spinner spnLanguage = (Spinner)rpcView.FindViewById(Resource.Id.tts_language_spn);

            TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.tts_result_code_tv);
            Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.tts_result_code_spn);


            string[] language = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Language));
            var languageAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
            spnLanguage.Adapter = languageAdapter;


            string[] result = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
            var resultAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, result);
            spnResultCode.Adapter = resultAdapter;


            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {
                AppInstanceManager.Instance.sendRpc(BuildRpc.buildTtsGetLanguageResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Language)spnLanguage.SelectedItemPosition, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition));
            });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
           {

           });

            rpcAlertDialog.Show();
        }

        private void CreateTTSResponseGetSupportedLanguages()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View getSystemInfoRpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_support_languages, null);
			rpcAlertDialog.SetView(getSystemInfoRpcView);
			rpcAlertDialog.SetTitle("GetSupportedLanguages");

			TextView textViewConsentSource = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.result_code_tv);
			Spinner spnResultCode = (Spinner)getSystemInfoRpcView.FindViewById(Resource.Id.get_supported_language_result_code_spn);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = adapter;


			ListView languagesListView = (Android.Widget.ListView)getSystemInfoRpcView.FindViewById(Resource.Id.tts_language_listview);


			List<Language> languagesList = new List<Language>();

			Button languagesButton = (Button)getSystemInfoRpcView.FindViewById(Resource.Id.add_tts_language_listview_btn);
			languagesButton.Click += delegate
			{
				AlertDialog.Builder languagesAlertDialog = new AlertDialog.Builder(this.Context);
				View languagesView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
				languagesAlertDialog.SetView(languagesView);
				languagesAlertDialog.SetTitle("Language");

				TextView textViewLanguages = (TextView)languagesView.FindViewById(Resource.Id.result_code_spinner);
				textViewLanguages.Text = "SelectLanguage";
				Spinner spnLanguages = (Spinner)languagesView.FindViewById(Resource.Id.genericspinner_Spinner);

				string[] language = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Language));
				var languageAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
				spnLanguages.Adapter = languageAdapter;

				languagesAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
			   {
				   languagesList.Add((HmiApiLib.Common.Enums.Language)spnLanguages.SelectedItemPosition);

			   });

				languagesAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
			   {
				   languagesAlertDialog.Dispose();
			   });

				languagesAlertDialog.Show();

			};

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildTTSGetSupportedLanguagesResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition, languagesList));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
		   {

		   });

			rpcAlertDialog.Show();
        }

        private void CreateTTSResponseIsReady()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.allow_device_to_Connect, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("IsReady");

			CheckBox checkBoxAllow = (CheckBox)rpcView.FindViewById(Resource.Id.allow);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spn);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.result_Code);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			checkBoxAllow.Text = ("Available");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildIsReadyResponse(BuildRpc.getNextId(), HmiApiLib.Types.InterfaceType.TTS, checkBoxAllow.Checked, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateTTSResponseSetGlobalProperties()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("SetGlobalProperties");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

				AppInstanceManager.Instance.sendRpc(BuildRpc.buildTTSSetGlobalPropertiesResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));

			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateTTSResponseSpeak()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("Speak");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

				AppInstanceManager.Instance.sendRpc(BuildRpc.buildTtsSpeakResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));

			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateTTSResponseStopSpeaking()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("StopSpeaking");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

				AppInstanceManager.Instance.sendRpc(BuildRpc.buildTtsStopSpeakingResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));

			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateTTSNotificationOnLanguageChange()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_language, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("OnLanguageChange");

			TextView textViewLanguage = (TextView)rpcView.FindViewById(Resource.Id.tts_language_tv);
			textViewLanguage.Text = "Language";

			Spinner spnLanguage = (Spinner)rpcView.FindViewById(Resource.Id.tts_language_spn);

			TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.tts_result_code_tv);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.tts_result_code_spn);


			string[] language = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Language));
			var languageAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
			spnLanguage.Adapter = languageAdapter;


			string[] result = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var resultAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, result);
			spnResultCode.Adapter = resultAdapter;


			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				//Method not available in Build RPC
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
		   {

		   });

			rpcAlertDialog.Show();
        }

        private void CreateTTSNotificationStarted()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("Started");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
				 //Method currently not present in buildRpc
			 });
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateTTSNotificationStopped()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("Stopped");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
				 //Method currently not present in buildRpc
			 });
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateSDLRequestActivateApp()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.on_app_activated, null);
			rpcAlertDialog.SetView(rpcView);

			TextView textView = (TextView)rpcView.FindViewById(Resource.Id.app_id_tv);
			EditText editTextAppId = (EditText)rpcView.FindViewById(Resource.Id.app_id);

			rpcAlertDialog.SetTitle("ActivateAppRequestSDL");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			{
				 //AppInstanceManager.Instance.sendRpc(BuildRpc.buildSdlActivateAppRequest(BuildRpc.getNextId(), Java.Lang.Integer.ParseInt(editTextAppId.Text)));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateSDLRequestGetListOfPermissions()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_app_activated, null);
			rpcAlertDialog.SetView(rpcView);

			TextView textView = (TextView)rpcView.FindViewById(Resource.Id.app_id_tv);
			EditText editTextAppId = (EditText)rpcView.FindViewById(Resource.Id.app_id);

			rpcAlertDialog.SetTitle("GetListOfPermissions");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
				 
			 });
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateSDLRequestGetStatusUpdate()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("GetStatusUpdate");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
				 //Method currently not present in buildRpc
			 });
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateSDLRequestGetURLS()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_app_activated, null);
			rpcAlertDialog.SetView(rpcView);

			TextView textView = (TextView)rpcView.FindViewById(Resource.Id.app_id_tv);
			textView.Text = "Service";

			EditText editTextService = (EditText)rpcView.FindViewById(Resource.Id.app_id);

			rpcAlertDialog.SetTitle("GetURLS");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
				 //Method currently not available in BuildRPC.cs
			 });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateSDLRequestGetUserFriendlyMessage()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_exit_application, null);
			rpcAlertDialog.SetView(rpcView);

			TextView textViewMessageCode = (TextView)rpcView.FindViewById(Resource.Id.appplication_id_tv);
			textViewMessageCode.Text = "MessageCode";
			EditText editTextdApplicationId = (EditText)rpcView.FindViewById(Resource.Id.appplication_id_et);
			editTextdApplicationId.InputType = Android.Text.InputTypes.ClassText;

			TextView textViewlanguage = (TextView)rpcView.FindViewById(Resource.Id.app_exit_reason_tv);
			textViewlanguage.Text = "Language";

			Spinner spnlanguage = (Spinner)rpcView.FindViewById(Resource.Id.app_exit_reason);
			string[] language = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Language));
			var appExitReasonAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
			spnlanguage.Adapter = appExitReasonAdapter;

			rpcAlertDialog.SetTitle("GetUserFriendlyMessage");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			{
				List<String> messageCodes = new List<string>();
				messageCodes.AddRange(editTextdApplicationId.Text.Split(','));
				//Method currently not available in BuildRPC.cs

			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateSDLRequestUpdateSDL()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetTitle("UpdateSDL");

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                 //Method currently not present in buildRpc
             });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateSDLNotificationOnAllowSDLFunctionality()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_allow_sdl_functionality, null);
            rpcAlertDialog.SetView(rpcView);
            rpcAlertDialog.SetTitle("OnAllowSDLFunctionality");

            TextView textViewName = (TextView)rpcView.FindViewById(Resource.Id.device_name_sdl_tv);
            EditText editTextName = (EditText)rpcView.FindViewById(Resource.Id.device_name_sdl_et);

            TextView textId = (TextView)rpcView.FindViewById(Resource.Id.device_id_sdl_tv);
            EditText editTextId = (EditText)rpcView.FindViewById(Resource.Id.device_id_sdl_et);

            TextView textViewTransportType = (TextView)rpcView.FindViewById(Resource.Id.transport_type_sdl_tv);
            Spinner spnTransportType = (Spinner)rpcView.FindViewById(Resource.Id.transport_type_sdl_spn);

            CheckBox checkBoxIsSDLAllowed = (CheckBox)rpcView.FindViewById(Resource.Id.is_sdl_allowed_cb);

            CheckBox checkBoxAllowed = (CheckBox)rpcView.FindViewById(Resource.Id.allowed_cb);

            TextView textViewSource = (TextView)rpcView.FindViewById(Resource.Id.consent_source_tv);
            Spinner spnConsentSource = (Spinner)rpcView.FindViewById(Resource.Id.consent_source_spn);

            string[] transportType = Enum.GetNames(typeof(HmiApiLib.Common.Enums.TransportType));
            var transportTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, transportType);
            spnTransportType.Adapter = transportTypeAdapter;

            string[] consentSource = Enum.GetNames(typeof(HmiApiLib.Common.Enums.ConsentSource));
            var consentSourceAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, consentSource);
            spnConsentSource.Adapter = consentSourceAdapter;


            DeviceInfo devInfo = new DeviceInfo();
            devInfo.name = editTextName.Text;
            devInfo.name = editTextId.Text;
            devInfo.transportType = (HmiApiLib.Common.Enums.TransportType)spnTransportType.SelectedItemPosition;
            devInfo.isSDLAllowed = checkBoxIsSDLAllowed.Checked;


            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {
                //Method currently not present in buildRpc
            });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateSDLNotificationOnAppPermissionConsent()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_app_permission_consent, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("OnAppPermissionConsent");

			TextView textViewAppID = (TextView)rpcView.FindViewById(Resource.Id.app_id_sdl_tv);
			EditText editTextAppID = (EditText)rpcView.FindViewById(Resource.Id.app_id_sdl_et);

			TextView textViewConsentSource = (TextView)rpcView.FindViewById(Resource.Id.consented_source__sdl_tv);
			Spinner spnConsentSource = (Spinner)rpcView.FindViewById(Resource.Id.consented_source_sdl_spn);

			string[] consentSource = Enum.GetNames(typeof(HmiApiLib.Common.Enums.ConsentSource));
			var consentSourceAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, consentSource);
			spnConsentSource.Adapter = consentSourceAdapter;


			ListView ListViewConsentedFunctions = (ListView)rpcView.FindViewById(Resource.Id.consented_functions_sdl_lv);


			List<PermissionItem> consentedFunctions = new List<PermissionItem>();

			Button consentFunctionsButton = (Button)rpcView.FindViewById(Resource.Id.consented_functions_sdl_btn);
			consentFunctionsButton.Click += delegate
			{
				AlertDialog.Builder consentFunctionsAlertDialog = new AlertDialog.Builder(this.Context);
				View consentFunctionsView = (View)layoutIinflater.Inflate(Resource.Layout.permissison_item, null);
				consentFunctionsAlertDialog.SetView(consentFunctionsView);
				consentFunctionsAlertDialog.SetTitle("PermissionItem");

				TextView textViewName = (TextView)consentFunctionsView.FindViewById(Resource.Id.permission_item_name_tv);
				EditText editTextName = (EditText)consentFunctionsView.FindViewById(Resource.Id.permission_item_name_et);

				TextView textViewID = (TextView)consentFunctionsView.FindViewById(Resource.Id.permission_item_id_tv);
				EditText editTextID = (EditText)consentFunctionsView.FindViewById(Resource.Id.permission_item_id_et);

				CheckBox checkBoxAllowed = (CheckBox)consentFunctionsView.FindViewById(Resource.Id.permission_item_allowed_cb);

				consentFunctionsAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
			   {
				   PermissionItem item = new PermissionItem();
				   item.name = editTextName.Text;

				   if (editTextAppID.Text.Equals(""))
					   item.id = 0;
				   else
					   item.id = Java.Lang.Integer.ParseInt(editTextID.Text);
				   item.allowed = checkBoxAllowed.Checked;


				   consentedFunctions.Add(item);

			   });

				consentFunctionsAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
			   {
				   consentFunctionsAlertDialog.Dispose();
			   });

				consentFunctionsAlertDialog.Show();

			};

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				//Method currently not present in buildRpc
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
		   {

		   });

			rpcAlertDialog.Show();
        }

        private void CreateSDLNotificationOnPolicyUpdate()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("OnPolicyUpdate");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
				 //Method currently not present in buildRpc
			 });
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateSDLNotificationOnReceivedPolicyUpdate()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_app_activated, null);
			rpcAlertDialog.SetView(rpcView);

			TextView textView = (TextView)rpcView.FindViewById(Resource.Id.app_id_tv);
			textView.Text = "policyfile";

			EditText editTextService = (EditText)rpcView.FindViewById(Resource.Id.app_id);
			editTextService.InputType = Android.Text.InputTypes.ClassText;

			rpcAlertDialog.SetTitle("OnReceivedPolicyUpdate");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
				 //Method currently not available in BuildRPC.cs
			 });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseAlertManeuver()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
            rpcAlertDialog.SetView(rpcView);

            TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
            Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
            spnGeneric.Adapter = adapter1;

            rsltCode.Text = "ResultCode";

            rpcAlertDialog.SetTitle("AlertManeuver");
            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {
                AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavAlertManeuverResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
            });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseGetWayPoints()
        {
            AlertDialog.Builder getSystemInfoRpcAlertDialog = new AlertDialog.Builder(this.Context);
            View getSystemInfoRpcView = layoutIinflater.Inflate(Resource.Layout.get_way_points, null);
            getSystemInfoRpcAlertDialog.SetView(getSystemInfoRpcView);
            getSystemInfoRpcAlertDialog.SetTitle("GetWayPoints");

            TextView textViewAppID = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_app_id_tv);
            EditText editTextAppID = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_app_id_et);

            TextView textViewResultCode = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_result_code_tv);
            Spinner spnResultCode = (Spinner)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_result_code_spn);

            var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
            spnResultCode.Adapter = resultCodeAdapter;

            ListView locationListview = (ListView)getSystemInfoRpcView.FindViewById(Resource.Id.location_listview);

            List<LocationDetails> wayPoints = new List<LocationDetails>();

            Button addLocationDetailsButton = (Button)getSystemInfoRpcView.FindViewById(Resource.Id.add_location_details);
            addLocationDetailsButton.Click += delegate
            {
                AlertDialog.Builder locationDetailsAlertDialog = new AlertDialog.Builder(this.Context);
                View locationDetailsView = (View)layoutIinflater.Inflate(Resource.Layout.location_details, null);
                locationDetailsAlertDialog.SetView(locationDetailsView);
                locationDetailsAlertDialog.SetTitle("AddLocationDetails");

                TextView textViewLatitudeDegree = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_latitude_degree_tv);
                EditText editTextLatitudeDegree = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_latitude_degree_et);

                TextView textViewLongitudeDegree = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_longitude_degree_tv);
                EditText editTextLongitudeDegree = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_longitude_degree_et);

                TextView textViewLocationName = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_location_name_tv);
                EditText editTextLocationName = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_location_name_et);

                TextView textViewAddress = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_address_tv);
                EditText editTextAddressLine1 = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_address_line1_et);
                EditText editTextAddressLine2 = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_address_line2_et);
                EditText editTextAddressLine3 = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_address_line3_et);

                TextView textViewLocationDescription = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_location_description_tv);
                EditText editTextLocationDescription = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_location_description_et);

                TextView textViewPhoneNumber = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_phone_number_tv);
                EditText editTextPhoneNumber = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_phone_number_et);

                TextView textViewImageValue = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_image_value_tv);
                EditText editTextImageValue = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_image_value_et);

                TextView textViewImageType = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_image_type_tv);
                Spinner spnImageType = (Spinner)locationDetailsView.FindViewById(Resource.Id.get_way_points_image_type_spn);
                var imageTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, imageType);
                spnImageType.Adapter = imageTypeAdapter;

                TextView textViewCountryName = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_country_name_tv);
                EditText editTextCountryName = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_country_name_et);

                TextView textViewCountryCode = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_country_code_tv);
                EditText editTextCountryCode = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_country_code_et);

                TextView textViewPostalCode = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_postal_code_tv);
                EditText editTextPostalCode = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_postal_code_et);

                TextView textViewAdministrativeArea = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_administrative_area_tv);
                EditText editTextAdministrativeArea = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_administrative_area_et);

                TextView textViewSubAdministrativeArea = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_sub_administrative_area_tv);
                EditText editTextSubAdministrativeArea = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_sub_administrative_area_et);

                TextView textViewLocality = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_locality_tv);
                EditText editTextLocality = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_locality_et);

                TextView textViewSubLocality = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_sub_locality_tv);
                EditText editTextSubLocality = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_sub_locality_et);

                TextView textViewThoruoghFare = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_thorough_fare_tv);
                EditText editTextThoruoghFare = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_thorough_fare_et);

                TextView textViewSubThoruoghFare = (TextView)locationDetailsView.FindViewById(Resource.Id.get_way_points_sub_thorough_fare_tv);
                EditText editTextSubThoruoghFare = (EditText)locationDetailsView.FindViewById(Resource.Id.get_way_points_sub_thorough_fare_et);

                locationDetailsAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
               {

                   Coordinate coordinate = new Coordinate();

                   if (editTextLatitudeDegree.Text.Equals(""))
                       coordinate.latitudeDegrees = 0;
                   else
                       coordinate.latitudeDegrees = Java.Lang.Float.ParseFloat(editTextLatitudeDegree.Text);


                   if (editTextLongitudeDegree.Text.Equals(""))
                       coordinate.longitudeDegrees = 0;
                   else
                       coordinate.longitudeDegrees = Java.Lang.Float.ParseFloat(editTextLongitudeDegree.Text);

                   List<string> addressLines = new List<string>();
                   addressLines.Add(editTextAddressLine1.Text);
                   addressLines.Add(editTextAddressLine2.Text);
                   addressLines.Add(editTextAddressLine3.Text);

                   Image locationImage = new Image();
                   locationImage.value = editTextImageValue.Text;
                   locationImage.imageType = (HmiApiLib.Common.Enums.ImageType)spnResultCode.SelectedItemPosition;

                   OASISAddress searchAddress = new OASISAddress();
                   searchAddress.countryName = editTextCountryName.Text;
                   searchAddress.countryCode = editTextCountryName.Text;
                   searchAddress.postalCode = editTextPostalCode.Text;
                   searchAddress.administrativeArea = editTextAdministrativeArea.Text;
                   searchAddress.subAdministrativeArea = editTextSubAdministrativeArea.Text;
                   searchAddress.locality = editTextLocality.Text;
                   searchAddress.subLocality = editTextSubLocality.Text;
                   searchAddress.thoroughfare = editTextThoruoghFare.Text;
                   searchAddress.subThoroughfare = editTextSubThoruoghFare.Text;


                   LocationDetails lctnDetails = new LocationDetails();
                   lctnDetails.coordinate = coordinate;
                   lctnDetails.locationName = editTextLocationName.Text;
                   lctnDetails.addressLines = addressLines;
                   lctnDetails.locationDescription = editTextLocationDescription.Text;
                   lctnDetails.phoneNumber = editTextPhoneNumber.Text;
                   lctnDetails.locationImage = locationImage;
                   lctnDetails.searchAddress = searchAddress;

                   wayPoints.Add(lctnDetails);

               });

                locationDetailsAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
               {
                   locationDetailsAlertDialog.Dispose();
               });

                locationDetailsAlertDialog.Show();

            };

            getSystemInfoRpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                getSystemInfoRpcAlertDialog.Dispose();
            });




            getSystemInfoRpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {
                if (editTextAppID.Text.Equals(""))
                    AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavGetWayPointsResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition, 0, wayPoints));
                else
                    AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavGetWayPointsResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition, Java.Lang.Integer.ParseInt(editTextAppID.Text), wayPoints));

            });

            getSystemInfoRpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
           {

           });

            getSystemInfoRpcAlertDialog.Show();
        }

        private void CreateNavigationResponseIsReady()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.allow_device_to_Connect, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("IsReadyResponse");

			CheckBox checkBoxAllow = (CheckBox)rpcView.FindViewById(Resource.Id.allow);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spn);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.result_Code);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			checkBoxAllow.Text = ("Available");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildIsReadyResponse(BuildRpc.getNextId(), HmiApiLib.Types.InterfaceType.Navigation, checkBoxAllow.Checked, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseSendLocation()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("SendLocation");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

				AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavSendLocationResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));

			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseShowConstantTBT()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetTitle("ShowConstantTBT");

            TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
            Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

            string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
            var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
            spnGeneric.Adapter = adapter;

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
            {
                AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavShowConstantTBTResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseStartAudioStream()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("StartAudioStream");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavStartAudioStreamResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseStartStream()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("StartStream");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavStartStreamResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseStopAudioStream()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("StopAudioStream");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavStopAudioStreamResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseStopStream()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("StopStream");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavStopStreamResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseSubscribeWayPoints()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("SubscribeWayPoints");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavSubscribeWayPointsResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseUnsubscribeWayPoints()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("UnsubscribeWayPoints");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavUnsubscribeWayPointsResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationResponseUpdateTurnList()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("UpdateTurnList");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavUpdateTurnListResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateNavigationNotificationOnTBTClientState()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetTitle("StopStream");

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
			var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				//Method currently not present in buildRpc
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void CreateButtonsNotificationOnButtonPress()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_button, null);
            rpcAlertDialog.SetView(rpcView);

            TextView textViewButtonName = (TextView)rpcView.FindViewById(Resource.Id.on_button_button_name_tv);
            Spinner spnButtonName = (Spinner)rpcView.FindViewById(Resource.Id.on_button_button_name_spn);

            TextView textViewButtonMode = (TextView)rpcView.FindViewById(Resource.Id.on_button_mode_tv);
            Spinner spnButtonMode = (Spinner)rpcView.FindViewById(Resource.Id.on_button_mode_spn);

            TextView textViewCustomButton = (TextView)rpcView.FindViewById(Resource.Id.on_button_custom_button_id_tv);
            EditText editFieldCustomButton = (EditText)rpcView.FindViewById(Resource.Id.on_button_custom_button_spn);

            TextView textViewAppID = (TextView)rpcView.FindViewById(Resource.Id.on_button_app_id_spn);
            EditText editFieldAppID = (EditText)rpcView.FindViewById(Resource.Id.device_id);

            var buttonNamesAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonNames);
            spnButtonName.Adapter = buttonNamesAdapter;

            rpcAlertDialog.SetTitle("OnButtonPress");
            textViewButtonMode.Text = "ButtonPressMode";

            var buttonPressModeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonPressMode);
            spnButtonMode.Adapter = buttonPressModeAdapter;

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {
                //Method currently not present in buildRpc
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });
            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.Show();
        }

        private void CreateButtonsNotificationOnButtonEvent()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_button, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            TextView textViewButtonName = (TextView)rpcView.FindViewById(Resource.Id.on_button_button_name_tv);
            Spinner spnButtonName = (Spinner)rpcView.FindViewById(Resource.Id.on_button_button_name_spn);

            TextView textViewButtonMode = (TextView)rpcView.FindViewById(Resource.Id.on_button_mode_tv);
            Spinner spnButtonMode = (Spinner)rpcView.FindViewById(Resource.Id.on_button_mode_spn);

            TextView textViewCustomButton = (TextView)rpcView.FindViewById(Resource.Id.on_button_custom_button_id_tv);
            EditText editFieldCustomButton = (EditText)rpcView.FindViewById(Resource.Id.on_button_custom_button_spn);

            TextView textViewAppID = (TextView)rpcView.FindViewById(Resource.Id.on_button_app_id_spn);
            EditText editFieldAppID = (EditText)rpcView.FindViewById(Resource.Id.device_id);

            rpcAlertDialog.SetTitle("OnButtonEvent");
            textViewButtonMode.Text = "ButtonEventMode";

            var buttonEventModeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonEventMode);
            spnButtonMode.Adapter = buttonEventModeAdapter;

            var buttonNamesAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonNames);
            spnButtonName.Adapter = buttonNamesAdapter;

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {
                //Method currently not present in buildRpc
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateButtonsResponseGetCapabilities()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.button_get_capabilities_response, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("GetCapabilities");

			List<ButtonCapabilities> btnCap = new List<ButtonCapabilities>();

			Button addCapabilitiesButton = (Button)rpcView.FindViewById(Resource.Id.add_capabilities_button);

			addCapabilitiesButton.Click += delegate
			{
				AlertDialog.Builder btnCapabilitiesAlertDialog = new AlertDialog.Builder(this.Context);
				View btnCapabilitiesView = (View)layoutIinflater.Inflate(Resource.Layout.button_capabilities, null);
				btnCapabilitiesAlertDialog.SetView(btnCapabilitiesView);
				btnCapabilitiesAlertDialog.SetTitle("ButtonCapabilities");

				TextView textViewButtonName = (TextView)btnCapabilitiesView.FindViewById(Resource.Id.get_capabilities_button_name_tv);
				Spinner spnButtonNames = (Spinner)btnCapabilitiesView.FindViewById(Resource.Id.get_capabilities_button_name_spn);

				CheckBox checkBoxShortPressAvailable = (CheckBox)btnCapabilitiesView.FindViewById(Resource.Id.short_press_available_cb);

				CheckBox checkBoxLongPressAvailable = (CheckBox)btnCapabilitiesView.FindViewById(Resource.Id.long_press_available_cb);

				CheckBox checkBoxUpDownAvailable = (CheckBox)btnCapabilitiesView.FindViewById(Resource.Id.up_down_available_cb);


				btnCapabilitiesAlertDialog.SetNegativeButton("ok", (senderAlert, args) =>
			   {

				   ButtonCapabilities btn = new ButtonCapabilities();
				   btn.name = (HmiApiLib.ButtonName)spnButtonNames.SelectedItemPosition;

				   if (checkBoxShortPressAvailable.Checked)
					   btn.shortPressAvailable = true;
				   else
					   btn.shortPressAvailable = false;

				   if (checkBoxLongPressAvailable.Checked)
					   btn.longPressAvailable = true;
				   else
					   btn.longPressAvailable = false;

				   if (checkBoxUpDownAvailable.Checked)
					   btn.upDownAvailable = true;
				   else
					   btn.upDownAvailable = false;

				   btnCap.Add(btn);

			   });

				btnCapabilitiesAlertDialog.SetPositiveButton("Cancel", (senderAlert, args) =>
				 {
					 btnCapabilitiesAlertDialog.Dispose();
				 });
				btnCapabilitiesAlertDialog.Show();

				var namesAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonNames);

				spnButtonNames.Adapter = namesAdapter;
			};

			//TextView textViewButtonName = (TextView)rpcView.FindViewById(Resource.Id.get_capabilities_button_name_tv);
			//Spinner spnButtonName = (Spinner)rpcView.FindViewById(Resource.Id.get_capabilities_button_name_spn);

			//CheckBox checkBoxShortPressAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.short_press_available_cb);

			//CheckBox checkBoxLongPressAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.long_press_available_cb);

			//CheckBox checkBoxUpDownAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.up_down_available_cb);

			CheckBox checkBoxOnScreenPresetsAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.on_screen_presets_available);


			TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.get_capabilities_result_code_tv);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.get_capabilities_result_code_spn);


			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;


			//List < ButtonCapabilities > btnCap = new List<ButtonCapabilities>();

			//ButtonCapabilities btn = new ButtonCapabilities();
			//btn.name = (HmiApiLib.ButtonName)spnButtonName.SelectedItemPosition;

			//if (checkBoxShortPressAvailable.Checked)
			//       btn.shortPressAvailable = true;
			//else
			//      btn.shortPressAvailable = false;

			//if (checkBoxLongPressAvailable.Checked)
			//      btn.longPressAvailable = true;
			//else
			//    btn.longPressAvailable = false;

			//if (checkBoxUpDownAvailable.Checked)
			//      btn.upDownAvailable = true;
			//else
			//    btn.upDownAvailable = false;

			//btnCap.Add(btn);


			PresetBankCapabilities prstCap = new PresetBankCapabilities();

			if (checkBoxOnScreenPresetsAvailable.Checked)
				prstCap.onScreenPresetsAvailable = true;
			else
				prstCap.onScreenPresetsAvailable = false;

			Console.WriteLine(btnCap);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
					  //BuildRpc.buildButtonsGetCapabilitiesResponse method call is returning String but we need return type RPCResponse:::::::::::::::::::::

					  Console.WriteLine(btnCap);

					  // AppInstanceManager.Instance.sendRpc(BuildRpc.buildButtonsGetCapabilitiesResponse(BuildRpc.getNextId(), btnCap, prstCap, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition ));
				  });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });
			rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnSystemInfoChanged()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
            rpcAlertDialog.SetView(rpcView);

            TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
            Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, languages);
            spnGeneric.Adapter = adapter1;

            rsltCode.Text = "Language";

            rpcAlertDialog.SetTitle("OnSystemInfoChanged");
            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {

            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });
            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnUpdateDeviceList()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnUpdateDeviceList");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                 //Method currently not present in buildRpc
             });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });
            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnSystemRequest()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View getSystemInfoRpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_system_request, null);
            rpcAlertDialog.SetTitle("OnSystemRequest");

            TextView textViewRequestType = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.request_type_tv);
            Spinner spnRequestType = (Spinner)getSystemInfoRpcView.FindViewById(Resource.Id.request_type_spn);

            TextView textViewURL = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.url_tv);
            EditText editTextViewURL = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.url_et);

            TextView textViewFileType = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.file_type_tv);
            Spinner spnFileType = (Spinner)getSystemInfoRpcView.FindViewById(Resource.Id.file_type_spn);

            TextView textViewOffset = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.offset_tv);
            EditText editTextOffset = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.offset_et);

            TextView textViewLength = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.length_tv);
            EditText editTextLength = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.length_et);

            TextView textViewTimeOut = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.timeout_tv);
            EditText editTextTimeOut = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.time_out_et);

            TextView textViewFileName = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.file_name_tv);
            EditText editTextFileName = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.file_name_et);

            TextView textViewAppId = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.app_id_tv);
            EditText editTextAppId = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.app_id_et);

            var requsetTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, requestType);
            spnRequestType.Adapter = requsetTypeAdapter;

            var fileTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, fileType);
            spnFileType.Adapter = fileTypeAdapter;

            rpcAlertDialog.SetView(getSystemInfoRpcView);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                 //Method currently not present in buildRpc
             });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnStartDeviceDiscovery()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnStartDeviceDiscovery");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {
                //Method currently not present in buildRpc
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnReady()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnReady");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {
                //Buid RPC method not returning RPCResponse bt string 
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnPhoneCall()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_deactivate_HMI, null);
            rpcAlertDialog.SetView(rpcView);

            CheckBox checkBoxIsDeactivated = (CheckBox)rpcView.FindViewById(Resource.Id.is_deactivated);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnPhoneCall");
            checkBoxIsDeactivated.Text = "Active";

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                 //Method currently not present in buildRpc
             });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnIgnitionCycleOver()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
            rpcAlertDialog.SetView(rpcView);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnIngitionCycleOver");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {
                 //Method currently not present in buildRpc
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnFindApplications()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_exit_application, null);
            rpcAlertDialog.SetView(rpcView);

            TextView textViewApplicationId = (TextView)rpcView.FindViewById(Resource.Id.appplication_id_tv);
            EditText editTextdApplicationId = (EditText)rpcView.FindViewById(Resource.Id.appplication_id_et);

            TextView textViewAppExitReason = (TextView)rpcView.FindViewById(Resource.Id.app_exit_reason_tv);
            Spinner spnAppExitReason = (Spinner)rpcView.FindViewById(Resource.Id.app_exit_reason);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            var appExitReasonAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appsExitReason);
            spnAppExitReason.Adapter = appExitReasonAdapter;

            rpcAlertDialog.SetTitle("OnFindApplication");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {

            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnExitApplication()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_exit_application, null);
            rpcAlertDialog.SetView(rpcView);

            TextView textViewApplicationId = (TextView)rpcView.FindViewById(Resource.Id.appplication_id_tv);
            EditText editTextdApplicationId = (EditText)rpcView.FindViewById(Resource.Id.appplication_id_et);

            TextView textViewAppExitReason = (TextView)rpcView.FindViewById(Resource.Id.app_exit_reason_tv);
            Spinner spnAppExitReason = (Spinner)rpcView.FindViewById(Resource.Id.app_exit_reason);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            var appExitReasonAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appsExitReason);
            spnAppExitReason.Adapter = appExitReasonAdapter;

            rpcAlertDialog.SetTitle("OnExitApplication");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {

            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
            {

            });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnExitAllApplications()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
            rpcAlertDialog.SetView(rpcView);

            TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
            Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appsCloseReason);
            spnGeneric.Adapter = adapter1;

            rsltCode.Text = "App Close Reason";

            rpcAlertDialog.SetTitle("onExitAllApplications");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
            {
                
            });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnEventChanged()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.allow_device_to_Connect, null);
            rpcAlertDialog.SetView(rpcView);

            CheckBox checkBoxAllow = (CheckBox)rpcView.FindViewById(Resource.Id.allow);

            TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spn);
            Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.result_Code);

            checkBoxAllow.Text = ("IsActive");

            var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, eventTypes);
            spnResultCode.Adapter = adapter1;

            rsltCode.Text = "Event Types";

            rpcAlertDialog.SetTitle("OnEventChanged");
            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
           {
               //Method currently not available in Bild RPC...

           });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });
            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
             {
                 rpcAlertDialog.Dispose();
             });
            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnEmergencyEvent()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_deactivate_HMI, null);
            rpcAlertDialog.SetView(rpcView);

            CheckBox checkBoxIsDeactivated = (CheckBox)rpcView.FindViewById(Resource.Id.is_deactivated);

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnEmergencyEvent");
            checkBoxIsDeactivated.Text = "Eabled";

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                
             });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnDeviceChosen()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_device_chosen, null);
            rpcAlertDialog.SetView(rpcView);
            rpcAlertDialog.SetTitle("OnDeviceChosen");

            TextView textViewDeviceName = (TextView)rpcView.FindViewById(Resource.Id.device_name_tv);
            EditText editFieldDeviceName = (EditText)rpcView.FindViewById(Resource.Id.device_name);

            TextView textViewDeviceId = (TextView)rpcView.FindViewById(Resource.Id.device_id_tv);
            EditText editFieldDeviceId = (EditText)rpcView.FindViewById(Resource.Id.device_id);

            TextView textViewTransportType = (TextView)rpcView.FindViewById(Resource.Id.transport_type_tv);
            Spinner spnTransportType = (Spinner)rpcView.FindViewById(Resource.Id.transport_type);

            CheckBox checkBoxIsSDLAllowed = (CheckBox)rpcView.FindViewById(Resource.Id.is_sdl_allowed);

            var transportTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, transportType);
            spnTransportType.Adapter = transportTypeAdapter;

            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                 //Method not present in buildRpc

             });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnDeactivateHMI()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_deactivate_HMI, null);
            rpcAlertDialog.SetView(rpcView);

            CheckBox checkBoxIsDeactivated = (CheckBox)rpcView.FindViewById(Resource.Id.is_deactivated);


            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnDeactivateHMI");
            checkBoxIsDeactivated.Text = "Deactivated";

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                 //Method currently not present in buildRpc

             });

            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnAwakeSDL()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
			rpcAlertDialog.SetView(rpcView);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetTitle("OnAwakeSDL");

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
						  //Method currently not present in buildRpc
					  });
			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnAppDeactivated()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_app_activated, null);
            rpcAlertDialog.SetView(rpcView);

            TextView textView = (TextView)rpcView.FindViewById(Resource.Id.app_id_tv);
            EditText editTextAppId = (EditText)rpcView.FindViewById(Resource.Id.app_id);


            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnAppDeactivated");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                 //AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationOnAppDeactivated(Java.Lang.Integer.ParseInt(editTextAppId.Text)));

             });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateBCNotificationOnAppActivated()
        {
            AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_app_activated, null);
            rpcAlertDialog.SetView(rpcView);

            TextView textView = (TextView)rpcView.FindViewById(Resource.Id.app_id_tv);
            EditText editTextAppId = (EditText)rpcView.FindViewById(Resource.Id.app_id);


            rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
            {
                rpcAlertDialog.Dispose();
            });

            rpcAlertDialog.SetTitle("OnAppActivated");

            rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
             {
                 //AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationOnAppActivated(Java.Lang.Integer.ParseInt(editTextAppId.Text)));

             });
            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
             {

             });

            rpcAlertDialog.Show();
        }

        private void CreateVRGetSupportedLanguageResponse()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = layoutIinflater.Inflate(Resource.Layout.on_touch_event_notification, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("Get Supported Language Response");

			CheckBox resultCodeCheck = (CheckBox)rpcView.FindViewById(Resource.Id.on_touch_event_touch_type_checkbox);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.on_touch_event_touch_type_spinner);

			resultCodeCheck.Text = "Result Code";

			var resultCodeAdapter = new ArrayAdapter<string>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;

			rpcView.FindViewById<ListView>(Resource.Id.touch_event_listview).Visibility = ViewStates.Gone;
			bool[] langBoolArray = new bool[languages.Length];

			Button createTouchEvent = (Button)rpcView.FindViewById(Resource.Id.on_touch_event_create_touch_event);
			createTouchEvent.Text = "Add Languages";
			createTouchEvent.Click += (sender, e1) =>
			{
				AlertDialog.Builder languageAlertDialog = new AlertDialog.Builder(rpcAlertDialog.Context);
				languageAlertDialog.SetTitle("Languages");

				languageAlertDialog.SetMultiChoiceItems(languages, langBoolArray, (object sender1, Android.Content.DialogMultiChoiceClickEventArgs e) => langBoolArray[e.Which] = e.IsChecked);

				languageAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
				{
					languageAlertDialog.Dispose();
				});

				languageAlertDialog.SetPositiveButton("Add", (senderAlert, args) =>
				{

				});

				languageAlertDialog.Show();
			};

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			 {
				 List<Language> languageList = new List<Language>();
				 for (int i = 0; i < languages.Length; i++)
				 {
					 if (langBoolArray[i])
					 {
						 languageList.Add(((Language)typeof(Language).GetEnumValues().GetValue(i)));
					 }
				 }
			 });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
        }

        private void OnResetTimeoutNotification()
        {
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
            View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_reset_timeout, null);
			rpcAlertDialog.SetView(rpcView);


			TextView textViewAppID = (TextView)rpcView.FindViewById(Resource.Id.tts_notification_app_id_tv);
			EditText editTextAppID = (EditText)rpcView.FindViewById(Resource.Id.tts_notification_app_id_et);

			TextView textViewMethodName = (TextView)rpcView.FindViewById(Resource.Id.tts_notification_method_name_tv);
			EditText editTextMethodName = (EditText)rpcView.FindViewById(Resource.Id.tts_notification_method_name_et);


			rpcAlertDialog.SetTitle("OnResetTimeout");

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
			 {
				 //Method currently not available in BuildRPC.cs
			 });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			 {

			 });

			rpcAlertDialog.Show();
        }

        private void CreateVIResponseSubscribeVehicleData()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.subscribe_vehicle_data_response, null);
			rpcAlertDialog.SetView(rpcView);
			rpcAlertDialog.SetTitle("Subscribe Vehicle Data");

			var gps_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_gps);
			var speed_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_speed);
			var rpm_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_rpm);
			var fuelLevel_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_fuel_level);
			var fuelLevel_State_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_fuel_level_state);
			var instantFuelConsumption_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_instant_fuel_consumption);
			var externalTemperature_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_external_temperature);
			var prndl_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_prndl);
			var tirePressure_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_tire_pressure);
			var odometer_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_odometer);
			var beltStatus_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_belt_status);
			var bodyInformation_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_body_info);
			var deviceStatus_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_device_status);
			var driverBraking_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_driver_braking);
			var wiperStatus_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_wiper_status);
			var headLampStatus_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_head_lamp_status);
			var engineTorque_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_engine_torque);
			var accPedalPosition_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_acc_pedal_pos);
			var steeringWheelAngle_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_steering_whel_angle);
			var eCallInfo_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_ecall_info);
			var airbagStatus_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_airbag_status);
			var emergencyEvent_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_emergency_event);
			var clusterModes_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_cluster_modes);
			var myKey_chk = rpcView.FindViewById<CheckBox>(Resource.Id.subscribe_vehicle_my_key);

			var gps_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_gps_data_type_spinner);
			var speed_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_speed_data_type_spinner);
			var rpm_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_rpm_date_type_spinner);
			var fuelLevel_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_fuel_level_data_type_spinner);
			var fuelLevel_State_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_fuel_level_state_data_type_spinner);
			var instantFuelConsumption_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_instant_fuel_consumption_data_type_spinner);
			var externalTemperature_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_external_temperature_data_type_spinner);
			var prndl_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_prndl_data_type_spinner);
			var tirePressure_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_tire_pressure_data_type_spinner);
			var odometer_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_odometer_data_type_spinner);
			var beltStatus_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_belt_status_data_type_spinner);
			var bodyInformation_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_body_info_data_type_spinner);
			var deviceStatus_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_device_status_data_type_spinner);
			var driverBraking_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_driver_braking_data_type_spinner);
			var wiperStatus_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_wiper_status_data_type_spinner);
			var headLampStatus_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_head_lamp_status_data_type_spinner);
			var engineTorque_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_engine_torque_data_type_spinner);
			var accPedalPosition_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_acc_pedal_pos_data_type_spinner);
			var steeringWheelAngle_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_steering_whel_angle_data_type_spinner);
			var eCallInfo_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_ecall_info_data_type_spinner);
			var airbagStatus_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_airbag_status_data_type_spinner);
			var emergencyEvent_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_emergency_event_data_type_spinner);
			var clusterModes_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_cluster_modes_data_type_spinner);
			var myKey_data_type_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_my_key_data_type_spinner);

			var result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_result_code_spinner);
			var gps_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_gps_result_code_spinner);
			var speed_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_speed_result_code_spinner);
			var rpm_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_rpm_result_code_spinner);
			var fuelLevel_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_fuel_level_result_code_spinner);
			var fuelLevel_State_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_fuel_level_state_result_code_spinner);
			var instantFuelConsumption_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_instant_fuel_consumption_result_code_spinner);
			var externalTemperature_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_external_temperature_result_code_spinner);
			var prndl_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_prndl_result_code_spinner);
			var tirePressure_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_tire_pressure_result_code_spinner);
			var odometer_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_odometer_result_code_spinner);
			var beltStatus_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_belt_status_result_code_spinner);
			var bodyInformation_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_body_info_result_code_spinner);
			var deviceStatus_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_device_status_result_code_spinner);
			var driverBraking_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_driver_braking_result_code_spinner);
			var wiperStatus_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_wiper_status_result_code_spinner);
			var headLampStatus_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_head_lamp_status_result_code_spinner);
			var engineTorque_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_engine_torque_result_code_spinner);
			var accPedalPosition_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_acc_pedal_pos_result_code_spinner);
			var steeringWheelAngle_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_steering_whel_angle_result_code_spinner);
			var eCallInfo_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_ecall_info_result_code_spinner);
			var airbagStatus_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_airbag_status_result_code_spinner);
			var emergencyEvent_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_emergency_event_result_code_spinner);
			var clusterModes_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_cluster_modes_result_code_spinner);
			var myKey_result_code_spinner = rpcView.FindViewById<Spinner>(Resource.Id.subscribe_vehicle_my_key_result_code_spinner);


			var vehicleDataTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, vehicleDataType);
			gps_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			speed_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			rpm_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			fuelLevel_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			fuelLevel_State_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			instantFuelConsumption_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			externalTemperature_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			prndl_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			tirePressure_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			odometer_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			beltStatus_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			bodyInformation_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			deviceStatus_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			driverBraking_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			wiperStatus_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			headLampStatus_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			engineTorque_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			accPedalPosition_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			steeringWheelAngle_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			eCallInfo_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			airbagStatus_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			emergencyEvent_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			clusterModes_data_type_spinner.Adapter = vehicleDataTypeAdapter;
			myKey_data_type_spinner.Adapter = vehicleDataTypeAdapter;


			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			result_code_spinner.Adapter = resultCodeAdapter;
			gps_result_code_spinner.Adapter = resultCodeAdapter;
			speed_result_code_spinner.Adapter = resultCodeAdapter;
			rpm_result_code_spinner.Adapter = resultCodeAdapter;
			fuelLevel_result_code_spinner.Adapter = resultCodeAdapter;
			fuelLevel_State_result_code_spinner.Adapter = resultCodeAdapter;
			instantFuelConsumption_result_code_spinner.Adapter = resultCodeAdapter;
			externalTemperature_result_code_spinner.Adapter = resultCodeAdapter;
			prndl_result_code_spinner.Adapter = resultCodeAdapter;
			tirePressure_result_code_spinner.Adapter = resultCodeAdapter;
			odometer_result_code_spinner.Adapter = resultCodeAdapter;
			beltStatus_result_code_spinner.Adapter = resultCodeAdapter;
			bodyInformation_result_code_spinner.Adapter = resultCodeAdapter;
			deviceStatus_result_code_spinner.Adapter = resultCodeAdapter;
			driverBraking_result_code_spinner.Adapter = resultCodeAdapter;
			wiperStatus_result_code_spinner.Adapter = resultCodeAdapter;
			headLampStatus_result_code_spinner.Adapter = resultCodeAdapter;
			engineTorque_result_code_spinner.Adapter = resultCodeAdapter;
			accPedalPosition_result_code_spinner.Adapter = resultCodeAdapter;
			steeringWheelAngle_result_code_spinner.Adapter = resultCodeAdapter;
			eCallInfo_result_code_spinner.Adapter = resultCodeAdapter;
			airbagStatus_result_code_spinner.Adapter = resultCodeAdapter;
			emergencyEvent_result_code_spinner.Adapter = resultCodeAdapter;
			clusterModes_result_code_spinner.Adapter = resultCodeAdapter;
			myKey_result_code_spinner.Adapter = resultCodeAdapter;


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.Show();
		}

		private void CreateVIResponseReadDID()
		{
			List<DIDResult> didResultList = new List<DIDResult>();
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.read_did_response, null);
			rpcAlertDialog.SetView(rpcView);

			CheckBox did_result_cb = (CheckBox)rpcView.FindViewById(Resource.Id.read_did_result_cb);
			Button createDidResultBtn = (Button)rpcView.FindViewById(Resource.Id.create_did_result_button);
			ListView didResultListView = (ListView)rpcView.FindViewById(Resource.Id.read_did_result_listview);

			var didResultAdapter = new DIDResultAdapter(Activity, didResultList);
			didResultListView.Adapter = didResultAdapter;

			CheckBox result_code_spinner = (CheckBox)rpcView.FindViewById(Resource.Id.read_did_result_code_cb);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.read_did_result_code_spinner);

			rpcAlertDialog.SetTitle("Read DID");
			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;

			createDidResultBtn.Click += (sender, e) =>
			{
				AlertDialog.Builder didResultAlertDialog = new AlertDialog.Builder(rpcAlertDialog.Context);
				View didResultView = layoutIinflater.Inflate(Resource.Layout.did_result_response, null);
				didResultAlertDialog.SetView(didResultView);
				didResultAlertDialog.SetTitle("DID Result");

				CheckBox vehicleDataResultCodeCB = (CheckBox)didResultView.FindViewById(Resource.Id.vehicle_data_result_code_cb);
				Spinner vehicleDataResultCodeSpinner = (Spinner)didResultView.FindViewById(Resource.Id.vehicle_data_result_code_spinner);
				CheckBox didLocationCB = (CheckBox)didResultView.FindViewById(Resource.Id.did_location_cb);
				EditText didLocationET = (EditText)didResultView.FindViewById(Resource.Id.did_location_et);
				CheckBox dataCB = (CheckBox)didResultView.FindViewById(Resource.Id.data_cb);
				EditText dataET = (EditText)didResultView.FindViewById(Resource.Id.data_et);

				string[] vehicleDataResultCodeEnum = Enum.GetNames(typeof(VehicleDataResultCode));
				var vehicleDataResultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, vehicleDataResultCodeEnum);
				vehicleDataResultCodeSpinner.Adapter = vehicleDataResultCodeAdapter;


				didResultAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
				{
					didResultAlertDialog.Dispose();
				});

				didResultAlertDialog.SetPositiveButton("Add", (senderAlert, args) =>
				{
					DIDResult dIDResult = new DIDResult();
					dIDResult.data = dataET.Text;
					dIDResult.resultCode = (VehicleDataResultCode)vehicleDataResultCodeSpinner.SelectedItemPosition;
					try
					{
						dIDResult.didLocation = Int32.Parse(didLocationET.Text.ToString());
					}
					catch (Exception e1)
					{

					}
					didResultList.Add(dIDResult);
					didResultAdapter.NotifyDataSetChanged();
				});

				didResultAlertDialog.Show();
			};

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.Show();
		}

		private void CreateVIResponseGetVehicleType()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_vehicle_type, null);
			rpcAlertDialog.SetView(rpcView);

			CheckBox make_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.vehicle_type_make_cb);
			EditText make_edittext = (EditText)rpcView.FindViewById(Resource.Id.vehicle_type_make_et);

			CheckBox model_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.vehicle_type_model_cb);
			EditText model_edittext = (EditText)rpcView.FindViewById(Resource.Id.vehicle_type_model_et);

			CheckBox model_year_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.vehicle_type_model_year_cb);
			EditText model_year_edittext = (EditText)rpcView.FindViewById(Resource.Id.vehicle_type_model_year_et);

			CheckBox trim_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.vehicle_type_trim_cb);
			EditText trim_edittext = (EditText)rpcView.FindViewById(Resource.Id.vehicle_type_trim_et);

			CheckBox result_code_spinner = (CheckBox)rpcView.FindViewById(Resource.Id.vehicle_type_result_cb);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.vehicle_type_result_code_spinner);

			rpcAlertDialog.SetTitle("Get Vehicle Type");
			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				string make = make_edittext.Text;
				string model = model_edittext.Text;
				string modelYear = model_year_edittext.Text;
				string trim = trim_edittext.Text;
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.Show();
		}

		private void CreateVIResponseGetVehicleData()
		{

		}

        private void CreateVIResponseGetDTCs()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_dtc_response, null);
			rpcAlertDialog.SetView(rpcView);

			CheckBox ecu_header_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.dtc_ecu_header_checkbox);
			EditText ecu_header_edittext = (EditText)rpcView.FindViewById(Resource.Id.dtc_ecu_header_edittext);

			CheckBox dtc_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.dtc_checkbox);
			EditText dtc_edittext = (EditText)rpcView.FindViewById(Resource.Id.dtc_edittext);

			CheckBox result_code_spinner = (CheckBox)rpcView.FindViewById(Resource.Id.dtc_result_code_checkbox);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.dtc_result_code_spinner);

			rpcAlertDialog.SetTitle("Get DTCs");
			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				string[] t = dtc_edittext.Text.Split(',');
				List<string> messageDataResultList = new List<string>(t);

				int? ecuHeader = Int32.Parse(ecu_header_edittext.Text);
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.Show();
		}

        private void CreateVIResponseDiagnosticMessage()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.slider_response, null);
			rpcAlertDialog.SetView(rpcView);

			CheckBox message_data_checkbox = (CheckBox)rpcView.FindViewById(Resource.Id.slider_position_checkbox);
			EditText message_data_edittext = (EditText)rpcView.FindViewById(Resource.Id.slider_position_et);

			CheckBox result_code_spinner = (CheckBox)rpcView.FindViewById(Resource.Id.slider_result_code_checkbox);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.slider_result_code_spinner);

			rpcView.FindViewById(Resource.Id.diagnostic_hint).Visibility = ViewStates.Visible;

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			rpcAlertDialog.SetTitle("Diagnostic Message");
			message_data_checkbox.Text = "Message Data Result";
			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;


			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				List<int> messageDataResultList = new List<int>();
				string[] t = message_data_edittext.Text.Split(',');
				foreach (string ts in t)
				{
					try
					{
						messageDataResultList.Add(Int32.Parse(ts));
					}
					catch (Exception e)
					{

					}
				}
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
		}

		private void CreateBCResponseUpdateDeviceList()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter1;

			rsltCode.Text = "ResultCode";

			rpcAlertDialog.SetTitle("UpdateDeviceList");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationUpdateDeviceListResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
		}

		private void CreateBCResponseUpdateAppList()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter1;

			rsltCode.Text = "ResultCode";

			rpcAlertDialog.SetTitle("UpdateAppList");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationUpdateAppListResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
		}

		private void CreateBCResponseSystemRequest()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter1;

			rsltCode.Text = "ResultCode";

			rpcAlertDialog.SetTitle("SystemRequest");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationSystemRequestResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
		}

		private void CreateBCResponsePolicyUpdate()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter1;

			rsltCode.Text = "ResultCode";

			rpcAlertDialog.SetTitle("PolicyUpdate");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationPolicyUpdateResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
		}

		private void CreateBCResponseMixingAudioSupported()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.allow_device_to_Connect, null);
			rpcAlertDialog.SetView(rpcView);

			CheckBox checkBoxAllow = (CheckBox)rpcView.FindViewById(Resource.Id.allow);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spn);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.result_Code);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			checkBoxAllow.Text = ("AttenuatedSupported");

			var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = adapter1;

			rsltCode.Text = "Result Code";

			rpcAlertDialog.SetTitle("MixingAudioSupported");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
		   {
			   AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationMixingAudioSupportedResponse(BuildRpc.getNextId(), checkBoxAllow.Checked, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition));
		   });

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
		}

		private void CreateBCResponseGetSystemInfo()
		{
			AlertDialog.Builder getSystemInfoRpcAlertDialog = new AlertDialog.Builder(this.Context);
			View getSystemInfoRpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_system_info, null);
			getSystemInfoRpcAlertDialog.SetView(getSystemInfoRpcView);
			getSystemInfoRpcAlertDialog.SetTitle("GetSystemInfo");


			TextView textViewCCPUVesrion = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.ccpu_version_tv);
			EditText editTextCCPUVesrion = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.ccpu_version);

			TextView textViewLanguage = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.language_tv);
			Spinner spnLanguage = (Spinner)getSystemInfoRpcView.FindViewById(Resource.Id.language);

			TextView textViewCountryCode = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.wers_country_code_tv);
			EditText editTextCountryCode = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.wersCountryCode);

			TextView textViewResultCode = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.result_code_getSystemInfo_tv);
			Spinner spnResultCode = (Spinner)getSystemInfoRpcView.FindViewById(Resource.Id.result_code_getSystemInfo);


			var languageAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, languages);
			// adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spnLanguage.Adapter = languageAdapter;

			var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = resultCodeAdapter;

			getSystemInfoRpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				getSystemInfoRpcAlertDialog.Dispose();
			});

			getSystemInfoRpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			 {
				 //AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationGetSystemInfoResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition), editTextCCPUVesrion.Text, (HmiApiLib.Common.Enums.Language)spnResultCode.SelectedItemPosition, editTextCountryCode.Text);

			 });

			getSystemInfoRpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			getSystemInfoRpcAlertDialog.Show();
		}

		private void CreateBCResponseDialNumber()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
			rpcAlertDialog.SetView(rpcView);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
			Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnGeneric.Adapter = adapter1;

			rsltCode.Text = "ResultCode";

			rpcAlertDialog.SetTitle("DialNumber");
			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationDialNumberResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
		}

		private void CreateBCResponseAllowDeviceToConnect()
		{
			AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
			View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.allow_device_to_Connect, null);
			rpcAlertDialog.SetView(rpcView);

			CheckBox checkBoxAllow = (CheckBox)rpcView.FindViewById(Resource.Id.allow);

			TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spn);
			Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.result_Code);

			rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
			{
				rpcAlertDialog.Dispose();
			});

			checkBoxAllow.Text = ("Allow");

			var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
			spnResultCode.Adapter = adapter1;

			rsltCode.Text = "Result Code";

			rpcAlertDialog.SetTitle("AllowDeviceToConnect");

			rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
			{
				AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationAllowDeviceToConnectResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition, checkBoxAllow.Checked));
			});

			rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
			{

			});

			rpcAlertDialog.Show();
		}

		private void CreateBCResponseActivateApp()
		{

		}        private void UISetGlobalPropertiesResponse()        {           AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);             View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);             rpcAlertDialog.SetView(rpcView);            rpcAlertDialog.SetTitle("SetGlobalProperties");             TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);            Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);             string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));             var adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);            spnGeneric.Adapter = adapter;           rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>            {               rpcAlertDialog.Dispose();           } );            rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>             {               AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiSetGlobalPropertiesResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition));             } );            rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>            {           } );            rpcAlertDialog.Show();      } 
    }
}