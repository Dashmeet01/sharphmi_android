﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
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
        long currentTime = 0;
        long totalStartSeconds = 0;
        long totalEndSeconds = 0;
        int numTicks = 0;

        TextView sliderHeader;
        TextView sliderFooter;
        Button sliderSave;
        Button sliderCancel;
        List<String> sliderFooterList;

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
				case Resource.Id.rpc_list:
					showRPCListDialog();
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

        internal void onUiPerformInteractionRequestCallback(PerformInteraction msg)
        {
            Activity.RunOnUiThread(() => UpdatePerformInteractionUI(msg));
        }

        private void UpdatePerformInteractionUI(PerformInteraction msg)
        {
            msg.getInitialText();
            msg.getChoiceSet();
            msg.getVrHelpTitle();
            msg.getVrHelp();
            msg.getTimeout();
            msg.getInteractionLayout();

            HideSliderUI();
            ClearText();
            invisibleSoftButtons();
            if (msg.getInitialText().fieldName == TextFieldName.initialInteractionText)
            {
                mainField1.Text = msg.getInitialText().fieldText;
            }
            int choiceListCount = msg.getChoiceSet().Count;
            if (choiceListCount > 0)
            {
                for (int i = 0; i < choiceListCount; i++)
                {
                    Choice choice = msg.getChoiceSet()[i];
                    Button button = new Button(Activity);
                    button.Text = choice.getMenuName();
                    mChoiceListLayout.AddView(button);
                }
            }
			int? totalDuration = msg.getTimeout();
            if (totalDuration != null)
            {
				Handler handler = new Handler(Looper.MainLooper);
				action = delegate
				{
                    ClearText();
				};
				handler.PostDelayed(action, (long)totalDuration);
            }
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

            if ((msg.getUpdateMode() == ClockUpdateMode.COUNTUP) || (msg.getUpdateMode() == ClockUpdateMode.COUNTDOWN))
            {

                string startText = msg.getStartTime().getHours().ToString();
                startText = startText + ":" + msg.getStartTime().getMinutes().ToString();
                startText = startText + ":" + msg.getStartTime().getSeconds().ToString();

                totalStartSeconds = (msg.getStartTime().getHours() * 3600)
                                            + (msg.getStartTime().getMinutes() * 60)
                                            + (msg.getStartTime().getSeconds());

                string endText = msg.getEndTime().getHours().ToString();
                endText = endText + ":" + msg.getEndTime().getMinutes().ToString();
                endText = endText + ":" + msg.getEndTime().getSeconds().ToString();

                totalEndSeconds = (msg.getEndTime().getHours() * 3600)
                                            + (msg.getEndTime().getMinutes() * 60)
                                            + (msg.getEndTime().getSeconds());

                mediaStartText.Text = startText;
                mediaEndText.Text = endText;

                if ((mHandler != null) && (action != null))
                {
                    mHandler.RemoveCallbacks(action);
                }
                mHandler = new Handler();

                if (msg.getUpdateMode() == ClockUpdateMode.COUNTUP)
                {
                    if (totalEndSeconds < totalStartSeconds)
                        return;
                    double initProgress = (((double)totalStartSeconds) / totalEndSeconds) * 100;
                    int initialProgress = Convert.ToInt32(initProgress);

                    currentTime = 0;
                    mSeekBar.SetProgress(initialProgress, false);
                }
                else if (msg.getUpdateMode() == ClockUpdateMode.COUNTDOWN)
                {
                    if (totalEndSeconds > totalStartSeconds)
                        return;
                    mSeekBar.SetProgress(100, false);
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
                HandleAction(totalStartSeconds, totalEndSeconds);
            };
            if (null != mHandler)
                mHandler.PostDelayed(action, 100);
        }

        void HandleAction(long startTime, long endTime)
        {
            if (startTime < endTime)
            {
                if (endTime == 0)
                    return;
                if (currentTime == 0)
                {
                    currentTime = startTime;
                }
                double initProgress = (((double)currentTime) / endTime) * 100;
                int initialProgress = Convert.ToInt32(initProgress);
                mSeekBar.SetProgress(initialProgress, false);

                int hours = (int)(currentTime / 3600);
                int minutes = (int)(currentTime % 3600) / 60;
                int seconds = (int)((currentTime % 3600) % 60);

                string currentTimeText = hours + ":" + minutes + ":" + seconds;
                mediaStartText.Text = currentTimeText;

                currentTime++;

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
                if (currentTime == 0)
                {
                    currentTime = startTime;
                }
                double initProgress = (((double)currentTime) / startTime) * 100;
                int initialProgress = Convert.ToInt32(initProgress);
                mSeekBar.SetProgress(initialProgress, false);

                int hours = (int)(currentTime / 3600);
                int minutes = (int)(currentTime % 3600) / 60;
                int seconds = (int)((currentTime % 3600) % 60);

                string currentTimeText = hours + ":" + minutes + ":" + seconds;
                mediaStartText.Text = currentTimeText;

                if (currentTime <= endTime)
                {
                    if (null != mHandler)
                        mHandler.RemoveCallbacks(action);
                    currentTime = 0;
                    return;
                }
                currentTime--;
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
            //int currentPosition = 0;
			sliderFooterList = msg.getSliderFooter();
			if (msg.getNumTicks() != null)
				numTicks = (int)msg.getNumTicks();
			//if (msg.getPosition() != null)
			//currentPosition = (int)msg.getPosition();

			mSeekBar.Visibility = ViewStates.Visible;
			mSeekBar.Enabled = true;
            mSeekBar.SetOnSeekBarChangeListener(this);

            sliderHeader.Text = msg.getSliderHeader();

            if ((sliderFooterList != null) && (sliderFooterList.Count == 1))
            {
                sliderFooter.Text = sliderFooterList[0];
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
        }

        void HideSliderUI()
        {
            sliderSave.Visibility = ViewStates.Gone;
            sliderCancel.Visibility = ViewStates.Gone;
            sliderHeader.Visibility = ViewStates.Gone;
            sliderFooter.Visibility = ViewStates.Gone;
        }

        void ShowSliderUI()
        {
            sliderSave.Visibility = ViewStates.Visible;
			sliderCancel.Visibility = ViewStates.Visible;
			sliderHeader.Visibility = ViewStates.Visible;
			sliderFooter.Visibility = ViewStates.Visible;
        }

        public void showRPCListDialog()
        {
			String[] rpcListStringArary = { "ActivateAppResponse", "AllowDeviceToConnectResponse", "DialNumberResponse", "GetSystemInfoResponse", "MixingAudioSupportedResponse", "PolicyUpdateResponse",
				 "SystemRequestResponse", "UpdateAppListResponse", "UpdateDeviceListResponse", "OnAppActivatedNotification", "OnAppDeactivatedNotification", "OnAwakeSDLNotification", "OnDeactivateHMINotification",
				"OnDeviceChosenNotification", "OnEmergencyEventNotification", "OnEventChangedNotification", "onExitAllApplicationsNotification", "OnExitApplicationNotification", "OnFindApplicationsNotification",
				"OnIngitionCycleOverNotification", "OnPhoneCallNotification", "OnReadyNotification", "OnStartDeviceDiscoveryNotification", "OnSystemInfoChangedNotification", "OnSystemRequestNotification",
				"OnUpdateDeviceListNotification", "GetCapabilitiesResponse", "OnButtonEventNotification", "OnButtonPressNotification", "AlertManeuverResponse", "GetWayPointsResponse", "IsReadyResponse",
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


			ListView rpcListView = (ListView)layout.FindViewById(Resource.Id.listView1);
			ArrayAdapter<String> adapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleListItem1, rpcListStringArary);
			rpcListView.Adapter = adapter;

			rpcListView.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) =>
			 {

				 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("ActivateAppResponse") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("DialNumberResponse") ||
					 rpcListView.GetItemAtPosition(e.Position).ToString().Equals("PolicyUpdateResponse") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("SystemRequestResponse") ||
					 rpcListView.GetItemAtPosition(e.Position).ToString().Equals("UpdateAppListResponse") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("UpdateDeviceListResponse") ||
					 rpcListView.GetItemAtPosition(e.Position).ToString().Equals("onExitAllApplicationsNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnSystemInfoChangedNotification") ||
					rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AlertManeuverResponse"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.genericspinner, null);
					 rpcAlertDialog.SetView(rpcView);

					 TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spinner);
					 Spinner spnGeneric = (Spinner)rpcView.FindViewById(Resource.Id.genericspinner_Spinner);

					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });

					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("ActivateAppResponse"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "ResultCode";

						 rpcAlertDialog.SetTitle("ActivateApp");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {
							 // spnGeneric.SelectedItem.ToString();
							 //AppInstanceManager.Instance.sendActivateAppRequest(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition);
							 //AppInstanceManager.Instance.sendActivateAppRequest(Java.Lang.Integer.ParseInt(editTextAppId.Text));

						 });

					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("DialNumberResponse"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "ResultCode";

						 rpcAlertDialog.SetTitle("DialNumber");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {
							 //AppInstanceManager.Instance.send(BuildRpc.getNextId(), (HmiApiLib.Common.Enums.Result)spnGeneric.SelectedItemPosition);
						 });

					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("PolicyUpdateResponse"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "ResultCode";

						 rpcAlertDialog.SetTitle("PolicyUpdate");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });

					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("SystemRequestResponse"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "ResultCode";

						 rpcAlertDialog.SetTitle("SystemRequest");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });
					 }


					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("UpdateAppListResponse"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "ResultCode";

						 rpcAlertDialog.SetTitle("UpdateAppList");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });
					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("UpdateDeviceListResponse"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "ResultCode";

						 rpcAlertDialog.SetTitle("UpdateDeviceList");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });
					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AlertManeuverResponse"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "ResultCode";

						 rpcAlertDialog.SetTitle("AlertManeuver");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });
					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("onExitAllApplicationsNotification"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appsCloseReason);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "App Close Reason";

						 rpcAlertDialog.SetTitle("onExitAllApplications");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });
					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnSystemInfoChangedNotification"))
					 {
						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, language);
						 spnGeneric.Adapter = adapter1;

						 rsltCode.Text = "Language";

						 rpcAlertDialog.SetTitle("OnSystemInfoChanged");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });
					 }


					 rpcAlertDialog.Show();


				 }


				 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AllowDeviceToConnectResponse") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("MixingAudioSupportedResponse")
					|| rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnEventChangedNotification"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.allow_device_to_Connect, null);
					 rpcAlertDialog.SetView(rpcView);

					 CheckBox checkBoxAllow = (CheckBox)rpcView.FindViewById(Resource.Id.allow);

					 TextView rsltCode = (TextView)rpcView.FindViewById(Resource.Id.result_code_spn);
					 Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.result_Code);

					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });


					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("AllowDeviceToConnectResponse"))
					 {
						 checkBoxAllow.Text = ("Allow");

						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
						 spnResultCode.Adapter = adapter1;

						 rsltCode.Text = "Result Code";

						 rpcAlertDialog.SetTitle("AllowDeviceToConnect");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
					 {

					 });

					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("MixingAudioSupportedResponse"))
					 {
						 checkBoxAllow.Text = ("AttenuatedSupported");

						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
						 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
						 spnResultCode.Adapter = adapter1;

						 rsltCode.Text = "Result Code";

						 rpcAlertDialog.SetTitle("MixingAudioSupported");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
					 {

					 });

					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnEventChangedNotification"))
					 {
						 checkBoxAllow.Text = ("IsActive");

						 var adapter1 = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, eventTypes);
						 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
						 spnResultCode.Adapter = adapter1;

						 rsltCode.Text = "Event Types";

						 rpcAlertDialog.SetTitle("OnEventChanged");
						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
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

					 getSystemInfoRpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 getSystemInfoRpcAlertDialog.Dispose();
					 });

					 getSystemInfoRpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
					  {

					  });

					 getSystemInfoRpcAlertDialog.Show();

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
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnResultCode.Adapter = resultCodeAdapter;
				 }


				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAppActivatedNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAppDeactivatedNotification"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_app_activated, null);
					 rpcAlertDialog.SetView(rpcView);

					 TextView textView = (TextView)rpcView.FindViewById(Resource.Id.app_id_tv);
					 EditText editTextAppId = (EditText)rpcView.FindViewById(Resource.Id.app_id);


					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });


					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAppActivatedNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnAppActivated");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

							  AppInstanceManager.Instance.sendOnAppActivatedNotification(Java.Lang.Integer.ParseInt(editTextAppId.Text));

						  });
					 }


					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAppDeactivatedNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnAppDeactivated");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {
							  AppInstanceManager.Instance.sendOnAppDeActivatedNotification(Java.Lang.Integer.ParseInt(editTextAppId.Text));
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


					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });


					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnDeactivateHMINotification"))
					 {
						 rpcAlertDialog.SetTitle("OnDeactivateHMI");
						 checkBoxIsDeactivated.Text = "Deactivated";

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

						  });
					 }

					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnEmergencyEventNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnEmergencyEvent");
						 checkBoxIsDeactivated.Text = "Eabled";

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

						  });
					 }

					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnPhoneCallNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnPhoneCall");
						 checkBoxIsDeactivated.Text = "Active";

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

						  });
					 }

					 rpcAlertDialog.Show();
				 }

				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("GetCapabilitiesResponse"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.get_capabilities_response, null);
					 rpcAlertDialog.SetView(rpcView);
					 rpcAlertDialog.SetTitle("GetCapabilities");

					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });

					 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
					  {

					  });

					 rpcAlertDialog.Show();


					 TextView textViewLanguage = (TextView)rpcView.FindViewById(Resource.Id.name_tv);
					 Spinner spnName = (Spinner)rpcView.FindViewById(Resource.Id.name_spn);

					 CheckBox checkBoxShortPressAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.short_press_available_cb);

					 CheckBox checkBoxLongPressAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.long_press_available_cb);

					 CheckBox checkBoxUpDownAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.up_down_available_cb);

					 CheckBox checkBoxOnScreenPresetsAvailable = (CheckBox)rpcView.FindViewById(Resource.Id.on_screen_presets_available);


					 TextView textViewResultCode = (TextView)rpcView.FindViewById(Resource.Id.get_capabilities_result_code_tv);
					 Spinner spnResultCode = (Spinner)rpcView.FindViewById(Resource.Id.get_capabilities_result_code_spn);

					 var nameAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonNames);
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnName.Adapter = nameAdapter;

					 var resultCodeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, resultCode);
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnResultCode.Adapter = resultCodeAdapter;

				 }



				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnDeviceChosenNotification"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_device_chosen, null);
					 rpcAlertDialog.SetView(rpcView);
					 rpcAlertDialog.SetTitle("OnDeviceChosen");

					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });

					 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
					  {

					  });

					 rpcAlertDialog.Show();

					 TextView textViewDeviceName = (TextView)rpcView.FindViewById(Resource.Id.device_name_tv);
					 EditText editFieldDeviceName = (EditText)rpcView.FindViewById(Resource.Id.device_name);

					 TextView textViewDeviceId = (TextView)rpcView.FindViewById(Resource.Id.device_id_tv);
					 EditText editFieldDeviceId = (EditText)rpcView.FindViewById(Resource.Id.device_id);

					 TextView textViewTransportType = (TextView)rpcView.FindViewById(Resource.Id.transport_type_tv);
					 Spinner spnTransportType = (Spinner)rpcView.FindViewById(Resource.Id.transport_type);

					 CheckBox checkBoxIsSDLAllowed = (CheckBox)rpcView.FindViewById(Resource.Id.is_sdl_allowed);

					 var transportTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, transportType);
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnTransportType.Adapter = transportTypeAdapter;

				 }

				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnExitApplicationNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnFindApplicationsNotification"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_exit_application, null);
					 rpcAlertDialog.SetView(rpcView);

					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });


					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnExitApplicationNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnExitApplication");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });

					 }

					 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnFindApplicationsNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnFindApplication");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });

					 }


					 rpcAlertDialog.Show();


					 EditText editTextdAppplicationId = (EditText)rpcView.FindViewById(Resource.Id.appplication_id);

					 Spinner spnAppExitReason = (Spinner)rpcView.FindViewById(Resource.Id.app_exit_reason);

					 var appExitReasonAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, appsExitReason);
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnAppExitReason.Adapter = appExitReasonAdapter;

				 }


				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnSystemRequestNotification"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View getSystemInfoRpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_system_request, null);
					 rpcAlertDialog.SetView(getSystemInfoRpcView);
					 rpcAlertDialog.SetTitle("GetSystemInfo");

					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });

					 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
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
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnRequestType.Adapter = requsetTypeAdapter;

					 var fileTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, fileType);
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnFileType.Adapter = fileTypeAdapter;
				 }


				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnDeviceChosenNotification"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_device_chosen, null);
					 rpcAlertDialog.SetView(rpcView);
					 rpcAlertDialog.SetTitle("OnDeviceChosen");

					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });

					 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
					  {

					  });

					 rpcAlertDialog.Show();

					 TextView textViewDeviceName = (TextView)rpcView.FindViewById(Resource.Id.device_name_tv);
					 EditText editFieldDeviceName = (EditText)rpcView.FindViewById(Resource.Id.device_name);

					 TextView textViewDeviceId = (TextView)rpcView.FindViewById(Resource.Id.device_id_tv);
					 EditText editFieldDeviceId = (EditText)rpcView.FindViewById(Resource.Id.device_id);

					 TextView textViewTransportType = (TextView)rpcView.FindViewById(Resource.Id.transport_type_tv);
					 Spinner spnTransportType = (Spinner)rpcView.FindViewById(Resource.Id.transport_type);

					 CheckBox checkBoxIsSDLAllowed = (CheckBox)rpcView.FindViewById(Resource.Id.is_sdl_allowed);

					 var transportTypeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, transportType);
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnTransportType.Adapter = transportTypeAdapter;

				 }


				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnButtonEventNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnButtonPressNotification"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.on_button, null);
					 rpcAlertDialog.SetView(rpcView);


					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
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

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						 {

						 });

					 }

					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnButtonPressNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnButtonPress");
						 textViewButtonMode.Text = "ButtonPressMode";


						 var buttonPressModeAdapter = new ArrayAdapter<String>(this.Context, Android.Resource.Layout.SimpleSpinnerDropDownItem, buttonPressMode);
						 spnButtonMode.Adapter = buttonPressModeAdapter;

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
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

					 getSystemInfoRpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 getSystemInfoRpcAlertDialog.Dispose();
					 });

					 getSystemInfoRpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
					  {

					  });

					 getSystemInfoRpcAlertDialog.Show();

					 TextView textViewAppID = (TextView)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_app_id_tv);
					 EditText editTextAppID = (EditText)getSystemInfoRpcView.FindViewById(Resource.Id.get_way_points_app_id_et);

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
					 // adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
					 spnImageType.Adapter = imageTypeAdapter;

				 }



				 //:::::::::::::::::::::::::::::::::::::::::::For Empty UI::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

				 else if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAwakeSDLNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnIngitionCycleOverNotification")
						  || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnReadyNotification") || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnStartDeviceDiscoveryNotification")
						 || rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnUpdateDeviceListNotification"))
				 {
					 AlertDialog.Builder rpcAlertDialog = new AlertDialog.Builder(this.Context);
					 View rpcView = (View)layoutIinflater.Inflate(Resource.Layout.ui_with_only_send_request, null);
					 rpcAlertDialog.SetView(rpcView);


					 //TextView textViewAppId = (TextView)rpcView.FindViewById(Resource.Id.app_id_tv);
					 //textViewAppId.Visibility = ViewStates.Invisible;

					 //EditText editTextAppId = (EditText)rpcView.FindViewById(Resource.Id.app_id);
					 //editTextAppId.Visibility = ViewStates.Invisible;
					 rpcAlertDialog.SetNegativeButton("Cancel", (senderAlert, args) =>
					 {
						 rpcAlertDialog.Dispose();
					 });


					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnAwakeSDLNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnAwakeSDL");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

						  });
					 }


					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnIngitionCycleOverNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnIngitionCycleOver");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

						  });
					 }

					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnReadyNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnReady");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

						  });
					 }


					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnStartDeviceDiscoveryNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnStartDeviceDiscovery");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

						  });
					 }

					 if (rpcListView.GetItemAtPosition(e.Position).ToString().Equals("OnUpdateDeviceListNotification"))
					 {
						 rpcAlertDialog.SetTitle("OnUpdateDeviceList");

						 rpcAlertDialog.SetPositiveButton("Ok", (senderAlert, args) =>
						  {

						  });
					 }
					 rpcAlertDialog.Show();
				 }
			 };
			rpcListAlertDialog.Show();
        }
    }
}