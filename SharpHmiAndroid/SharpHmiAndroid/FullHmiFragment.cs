﻿﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using HmiApiLib.Builder;
using HmiApiLib.Common.Enums;
using HmiApiLib.Common.Structs;
using HmiApiLib.Controllers.Buttons.IncomingNotifications;
using HmiApiLib.Controllers.UI.IncomingRequests;

namespace SharpHmiAndroid
{
    public class FullHmiFragment : Fragment, SeekBar.IOnSeekBarChangeListener
    {
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
            String[] rpcListStringArary = { "AllowDeviceToConnectResponse", "DialNumberResponse", "GetSystemInfoResponse", "MixingAudioSupportedResponse", "PolicyUpdateResponse",
                 "SystemRequestResponse", "UpdateAppListResponse", "UpdateDeviceListResponse", "OnAppActivatedNotification", "OnAppDeactivatedNotification", "OnAwakeSDLNotification", "OnDeactivateHMINotification",
                "OnDeviceChosenNotification", "OnEmergencyEventNotification", "OnEventChangedNotification", "onExitAllApplicationsNotification", "OnExitApplicationNotification", "OnFindApplicationsNotification",
                "OnIngitionCycleOverNotification", "OnPhoneCallNotification", "OnReadyNotification", "OnStartDeviceDiscoveryNotification", "OnSystemInfoChangedNotification", "OnSystemRequestNotification",
                "OnUpdateDeviceListNotification", "ButtonGetCapabilitiesResponse", "OnButtonEventNotification", "OnButtonPressNotification", "AlertManeuverResponse", "GetWayPointsResponse", "IsReadyResponse",
                "SendLocationResponse", "ShowConstantTBTResponse", "StartAudioStreamResponse", "StartStreamResponse", "StopAudioStreamResponse", "StopStreamResponse", "SubscribeWayPointsResponse",
                "UnsubscribeWayPointsResponse", "UpdateTurnListResponse", "OnTBTClientStateNotification", "ActivateAppRequest", "GetListOfPermissionsRequest", "GetStatusUpdateRequest", "GetURLSRequest",
                "GetUserFriendlyMessageRequest", "UpdateSDLRequest", "OnAllowSDLFunctionalityNotification", "OnAppPermissionConsentNotification", "OnPolicyUpdateNotification", "OnReceivedPolicyUpdateNotification",
                "ChangeRegistrationResponse", "GetLanguageResponse", "GetSupportedLangusgesResponse", "SetGlobalPropertiesResponse", "SpeakResponse", "StopSpeakingResponse",
                "OnLanguageChangeNotification", "OnResetTimeoutNotification", "StartedNotification", "StoppedNotification", "AddCommandResponse","AddSubMenuResponse", "AlertResponse",
                "ClosePopUpResponse", "DeleteCommandResponse", "DeleteSubMenuResponse", "EndAudioPassThruResponse",
                "GetSupportedLanguagesResponse", "PerformAudioPassThruResponse", "PerformInteractionResponse", "ScrollableMessageResponse", "SetAppIconResponse",
                "SetDisplayLayoutResponse", "SetMediaClockTimerResponse", "ShowResponse", "ShowCustomFormResponse", "SliderResponse",
                "OnCommandNotification", "OnDriverDistrationNotification", "OnKeyboardInputNotification", "OnRecordStartNotification", "OnSystemContextNotification",
                "OnTouchEventNotification", "DiagnosticMessageResponse", "GetDTCsResponse","GetVehicleDataResponse", "GetVehicleTypeResponse", "ReadDidResponse", "SubscribeVehicleDataResponse",
                "UnsubscribeVehicleDataResponse", "GetSupportedLanguageResponse"};

            AlertDialog.Builder rpcListAlertDialog = new AlertDialog.Builder(this.Context);
            //LayoutInflater inflaterr = Context.GetSystemService("nm");
            View layout = (View)layoutIinflater.Inflate(Resource.Layout.rpclistLayout, null);
            rpcListAlertDialog.SetView(layout);
            rpcListAlertDialog.SetTitle("Pick an RPC");

            rpcListAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                rpcListAlertDialog.Dispose();
            });

            string[] resultCode = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Result));
            string[] language = Enum.GetNames(typeof(HmiApiLib.Common.Enums.Language));
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


            ListView rpcListView = (ListView)layout.FindViewById(Resource.Id.listView1);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleListItem1, rpcListStringArary);
            rpcListView.Adapter = adapter;

            rpcListView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) =>
             {

                 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("DialNumberResponse") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AlertManeuverResponse") ||
                     rpcListView.GetItemAtPosition(e.Position).ToString().Equals("PolicyUpdateResponse") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("SystemRequestResponse") ||
                     rpcListView.GetItemAtPosition(e.Position).ToString().Equals("UpdateAppListResponse") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("UpdateDeviceListResponse") ||
                     rpcListView.GetItemAtPosition(e.Position).ToString().Equals("onExitAllApplicationsNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnSystemInfoChangedNotification") ||
                    rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnDriverDistrationNotification") || (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnRecordStartNotification")))
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


                     if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("DialNumberResponse"))
                     {
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

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("PolicyUpdateResponse"))
                     {
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

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("SystemRequestResponse"))
                     {
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
                     }


                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("UpdateAppListResponse"))
                     {
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
                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("UpdateDeviceListResponse"))
                     {
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
                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AlertManeuverResponse"))
                     {
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
                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("onExitAllApplicationsNotification"))
                     {
                         var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appsCloseReason);
                         spnGeneric.Adapter = adapter1;

                         rsltCode.Text = "App Close Reason";

                         rpcAlertDialog.SetTitle("onExitAllApplications");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                         {
                             //RPCResponse return type needed:::::::::::::::::::::
                             //AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationOnExitAllApplications((ApplicationsCloseReason)spnGeneric.SelectedItemPosition);
                         });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });
                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnSystemInfoChangedNotification"))
                     {
                         var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
                         spnGeneric.Adapter = adapter1;

                         rsltCode.Text = "Language";

                         rpcAlertDialog.SetTitle("OnSystemInfoChanged");
                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                         {
                             //Method currently not available in Bild RPC...

                         });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }
                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnDriverDistrationNotification"))
                     {
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

                     }
					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnRecordStartNotification"))
					 {
                         List<int> appIdList = new List<int>();
                        foreach(AppItem item in AppInstanceManager.appList)
                        {
                            appIdList.Add(item.getAppID());
                        }
						 var adapter1 = new ArrayAdapter<int>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appIdList);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "App ID";

						 rpcAlertDialog.SetTitle("Record Notification");
						 rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
						 {

						 });

						 rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
						 {

						 });

					 }

                     rpcAlertDialog.Show();

                 }


                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AllowDeviceToConnectResponse") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("MixingAudioSupportedResponse")
                    || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnEventChangedNotification"))
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


                     if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AllowDeviceToConnectResponse"))
                     {
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

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("MixingAudioSupportedResponse"))
                     {
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

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnEventChangedNotification"))
                     {
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

                     }

                     rpcAlertDialog.Show();



                     checkBoxAllow.Click += (o, f) =>
                        {
                            //if (checkBoxGeneric.Checked)
                            //{
                            //    spnGeneric.Enabled = true;
                            //}
                            //else
                            //{
                            //    spnGeneric.Enabled = false;
                            //}
                        };
                 }


                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("GetSystemInfoResponse"))
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


                     var languageAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
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


                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAppActivatedNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAppDeactivatedNotification"))
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


                     if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAppActivatedNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnAppActivated");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationOnAppActivated(Java.Lang.Integer.ParseInt(editTextAppId.Text)));

                          });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }


                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAppDeactivatedNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnAppDeactivated");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationOnAppDeactivated(Java.Lang.Integer.ParseInt(editTextAppId.Text)));

                          });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }

                     rpcAlertDialog.Show();

                 }

                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnDeactivateHMINotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnEmergencyEventNotification")
                      || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnPhoneCallNotification"))
                 {

                     AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
                     View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_deactivate_HMI, null);
                     rpcAlertDialog.SetView(rpcView);

                     CheckBox checkBoxIsDeactivated = (CheckBox)rpcView.FindViewById(Resource.Id.is_deactivated);


                     rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
                     {
                         rpcAlertDialog.Dispose();
                     });


                     if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnDeactivateHMINotification"))
                     {
                         rpcAlertDialog.SetTitle("OnDeactivateHMI");
                         checkBoxIsDeactivated.Text = "Deactivated";

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //Method currently not present in buildRpc

                          });

                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnEmergencyEventNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnEmergencyEvent");
                         checkBoxIsDeactivated.Text = "Eabled";

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {

                              //Method currently not present in buildRpc
                          });

                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnPhoneCallNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnPhoneCall");
                         checkBoxIsDeactivated.Text = "Active";

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //Method currently not present in buildRpc
                          });

                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });
                     }

                     rpcAlertDialog.Show();
                 }


                 //:::::::::::::::::::::::::::::::::::::::::::
                 //BuildRpc.buildButtonsGetCapabilitiesResponse method call is returning String but we need return type RPCResponse:::::::::::::::::::::::::::::::::::::::
                 //:::::::::::::::::::::::::::::::::::::::::::


                 //else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("ButtonGetCapabilitiesResponse"))
                 //{
                 // AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
                 // View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.button_get_capabilities_response, null);
                 // rpcAlertDialog.SetView(rpcView);
                 // rpcAlertDialog.SetTitle("GetCapabilities");


                 // TextView textViewButtonName = (TextView)rpcView.FindViewById(Resource.Id.get_capabilities_button_name_tv);
                 // Spinner spnButtonName = (Spinner)rpcView.FindViewById(Resource.Id.get_capabilities_button_name_spn);

                 // CheckBox checkBoxShortPressAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.short_press_available_cb);

                 // CheckBox checkBoxLongPressAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.long_press_available_cb);

                 // CheckBox checkBoxUpDownAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.up_down_available_cb);

                 // CheckBox checkBoxOnScreenPresetsAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.on_screen_presets_available);


                 // TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.get_capabilities_result_code_tv);
                 // Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.get_capabilities_result_code_spn);

                 // var nameAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonNames);
                 // // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                 // spnButtonName.Adapter = nameAdapter;

                 // var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
                 // // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                 // spnResultCode.Adapter = resultCodeAdapter;


                 //                List < ButtonCapabilities > btnCap = new List<ButtonCapabilities>();
                 //                ButtonCapabilities btn = new ButtonCapabilities();
                 //                btn.name = (HmiApiLib.ButtonName)spnButtonName.SelectedItemPosition;

                 //                if (checkBoxShortPressAvailable.Checked)
                 //                    btn.shortPressAvailable = true;
                 //                else
                 //                    btn.shortPressAvailable = false;

                 // if (checkBoxLongPressAvailable.Checked)
                 //                   btn.longPressAvailable = true;
                 // else
                 //	 btn.longPressAvailable = false;

                 // if (checkBoxUpDownAvailable.Checked)
                 //                   btn.upDownAvailable = true;
                 // else
                 //	 btn.upDownAvailable = false;

                 //               btnCap.Add(btn);



                 //                PresetBankCapabilities prstCap = new PresetBankCapabilities();

                 // if (checkBoxOnScreenPresetsAvailable.Checked)
                 //	 prstCap.onScreenPresetsAvailable = true;
                 // else
                 //	 prstCap.onScreenPresetsAvailable = false;



                 // rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
                 // {
                 //	 rpcAlertDialog.Dispose();
                 // });

                 // rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
                 // {
                 //                     AppInstanceManager.Instance.sendRpc(BuildRpc.buildButtonsGetCapabilitiesResponse(BuildRpc.getNextId(), btnCap, prstCap, (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition ));
                 // });

                 // rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                 //  {

                 //  });
                 // rpcAlertDialog.Show();

                 //}



                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnDeviceChosenNotification"))
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

                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnExitApplicationNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnFindApplicationsNotification")
                          || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AlertResponse"))
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


                     if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnExitApplicationNotification"))
                     {
                         var appExitReasonAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appsExitReason);
                         spnAppExitReason.Adapter = appExitReasonAdapter;

                         rpcAlertDialog.SetTitle("OnExitApplication");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                         {
                             //AppInstanceManager.Instance.sendRpc(BuildRpc.buildBasicCommunicationOnExitApplication((HmiApiLib.Common.Enums.ApplicationExitReason)spnAppExitReason.SelectedItemPosition, BuildRpc.getNextId()));

                         });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnFindApplicationsNotification"))
                     {
                         var appExitReasonAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appsExitReason);
                         spnAppExitReason.Adapter = appExitReasonAdapter;

                         rpcAlertDialog.SetTitle("OnFindApplication");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                         {
                             //Method currently not present in buildRpc

                         });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AlertResponse"))
                     {
                         var appExitReasonAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
                         spnAppExitReason.Adapter = appExitReasonAdapter;

                         rpcAlertDialog.SetTitle("Alert");
                         textViewApplicationId.Text = "TryAgainTime";
                         textViewAppExitReason.Text = "ResultCode";

                         rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
                         {
                             if (editTextdApplicationId.Text.Equals(""))
                                 AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiAlertResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnAppExitReason.SelectedItemPosition, 0));
                             else
                                 AppInstanceManager.Instance.sendRpc(BuildRpc.buildUiAlertResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnAppExitReason.SelectedItemPosition, Java.Lang.Integer.ParseInt(editTextdApplicationId.Text)));

                         });

                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                         {

                         });

                     }

                     rpcAlertDialog.Show();

                 }


                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnSystemRequestNotification"))
                 {
                     AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
                     View getSystemInfoRpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_system_request, null);
                     rpcAlertDialog.SetView(getSystemInfoRpcView);
                     rpcAlertDialog.SetTitle("OnSystemRequest");

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
                 }


                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnButtonEventNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnButtonPressNotification"))
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


                     if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnButtonEventNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnButtonEvent");
                         textViewButtonMode.Text = "ButtonEventMode";

                         var buttonEventModeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonEventMode);
                         spnButtonMode.Adapter = buttonEventModeAdapter;

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                         {
                             //Method currently not present in buildRpc
                         });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnButtonPressNotification"))
                     {
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
                     }

                     rpcAlertDialog.Show();


                     var buttonNamesAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonNames);
                     spnButtonName.Adapter = buttonNamesAdapter;

                 }


                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("GetWayPointsResponse"))
                 {
                     AlertDialog.Builder getSystemInfoRpcAlertDialog = new AlertDialog.Builder(this.Context);
                     View getSystemInfoRpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_way_points, null);
                     getSystemInfoRpcAlertDialog.SetView(getSystemInfoRpcView);
                     getSystemInfoRpcAlertDialog.SetTitle("GetWayPoints");

                     TextView textViewAppID = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_app_id_tv);
                     EditText editTextAppID = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_app_id_et);

                     TextView textViewResultCode = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_result_code_tv);
                     Spinner spnResultCode = (Spinner)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_result_code_spn);

                     TextView textViewLatitudeDegree = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_latitude_degree_tv);
                     EditText editTextLatitudeDegree = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_latitude_degree_et);

                     TextView textViewLongitudeDegree = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_longitude_degree_tv);
                     EditText editTextLongitudeDegree = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_longitude_degree_et);

                     TextView textViewLocationName = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_location_name_tv);
                     EditText editTextLocationName = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_location_name_et);

                     TextView textViewAddress = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_address_tv);
                     EditText editTextAddressLine1 = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_address_line1_et);
                     EditText editTextAddressLine2 = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_address_line2_et);
                     EditText editTextAddressLine3 = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_address_line3_et);

                     TextView textViewLocationDescription = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_location_description_tv);
                     EditText editTextLocationDescription = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_location_description_et);

                     TextView textViewPhoneNumber = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_phone_number_tv);
                     EditText editTextPhoneNumber = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_phone_number_et);

                     TextView textViewImageValue = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_image_value_tv);
                     EditText editTextImageValue = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_image_value_et);

                     TextView textViewImageType = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_image_type_tv);
                     Spinner spnImageType = (Spinner)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_image_type_spn);

                     TextView textViewCountryName = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_country_name_tv);
                     EditText editTextCountryName = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_country_name_et);

                     TextView textViewCountryCode = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_country_code_tv);
                     EditText editTextCountryCode = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_country_code_et);

                     TextView textViewPostalCode = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_postal_code_tv);
                     EditText editTextPostalCode = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_postal_code_et);

                     TextView textViewAdministrativeArea = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_administrative_area_tv);
                     EditText editTextAdministrativeArea = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_administrative_area_et);

                     TextView textViewSubAdministrativeArea = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_sub_administrative_area_tv);
                     EditText editTextSubAdministrativeArea = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_sub_administrative_area_et);

                     TextView textViewLocality = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_locality_tv);
                     EditText editTextLocality = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_locality_et);

                     TextView textViewSubLocality = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_sub_locality_tv);
                     EditText editTextSubLocality = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_sub_locality_et);

                     TextView textViewThoruoghFare = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_thorough_fare_tv);
                     EditText editTextThoruoghFare = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_thorough_fare_et);

                     TextView textViewSubThoruoghFare = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_sub_thorough_fare_tv);
                     EditText editTextSubThoruoghFare = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_sub_thorough_fare_et);


                     var imageTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, imageType);
                     spnImageType.Adapter = imageTypeAdapter;

                     getSystemInfoRpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
                     {
                         getSystemInfoRpcAlertDialog.Dispose();
                     });

                     getSystemInfoRpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
                     {
                         //TO DO::::::::::::::::
                         //Location Details List To be added as parameter
                         //AppInstanceManager.Instance.sendRpc(BuildRpc.buildNavGetWayPointsResponse(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnResultCode.SelectedItemPosition, Java.Lang.Integer.ParseInt(editTextAppID.Text), ));

                     });

                     getSystemInfoRpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                     {

                     });

                     getSystemInfoRpcAlertDialog.Show();

                 }


                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("PerformInteractionResponse"))
                 {
                     AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
                     View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.perform_interaction_response, null);
                     rpcAlertDialog.SetView(rpcView);
                     rpcAlertDialog.SetTitle("PerformInteraction");

                     TextView textViewChoiceID = (TextView)rpcView.FindViewById(Resource.Id.choice_id_tv);
                     EditText editTextChoiceID = (EditText)rpcView.FindViewById(Resource.Id.choice_id_et);

                     TextView textViewManualTextEntry = (TextView)rpcView.FindViewById(Resource.Id.manual_text_entry_tv);
                     EditText editTextManualTextEntry = (EditText)rpcView.FindViewById(Resource.Id.manual_text_entry_et);

                     TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_tv);
                     Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.perform_interaction_result_code_spn);


                     var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
                     // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
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





                 //::::::::::::::::For Empty UI:::::::::::::::::::::::::::

                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAwakeSDLNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnIngitionCycleOverNotification")
                          || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnReadyNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnStartDeviceDiscoveryNotification")
                         || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnUpdateDeviceListNotification"))
                 {
                     AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
                     View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
                     rpcAlertDialog.SetView(rpcView);


                     rpcAlertDialog.SetNeutralButton("Cancel", (senderAlert, args) =>
                     {
                         rpcAlertDialog.Dispose();
                     });


                     if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAwakeSDLNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnAwakeSDL");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //Method currently not present in buildRpc
                          });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }


                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnIngitionCycleOverNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnIngitionCycleOver");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //Method currently not present in buildRpc
                          });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnReadyNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnReady");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //Buid RPC method not returning RPCResponse bt string 
                          });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }


                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnStartDeviceDiscoveryNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnStartDeviceDiscovery");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //Method currently not present in buildRpc
                          });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }

                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnUpdateDeviceListNotification"))
                     {
                         rpcAlertDialog.SetTitle("OnUpdateDeviceList");

                         rpcAlertDialog.SetNegativeButton("Tx Now", (senderAlert, args) =>
                          {
                              //Method currently not present in buildRpc
                          });
                         rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                          {

                          });

                     }
                     rpcAlertDialog.Show();
                 }
                 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("SliderResponse")
                         || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnKeyboardInputNotification"))
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

                     if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("SliderResponse"))
                     {
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
                     }
                     else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnKeyboardInputNotification"))
                     {
                         slider_position_checkbox.Text = "Data";
                         var keyBoardEventAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, keyBoardEvent);
                         spnResultCode.Adapter = keyBoardEventAdapter;

                        rpcAlertDialog.SetNegativeButton("Tx Later", (senderAlert, args) =>
                        {

                        });

                        rpcAlertDialog.SetPositiveButton("Reset", (senderAlert, args) =>
                        {

                        });
                     }



                     rpcAlertDialog.Show();
                 }
				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnCommandNotification"))
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
                    foreach(AppItem item in AppInstanceManager.appList)
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
             };
            rpcListAlertDialog.Show();
        }
    }
}