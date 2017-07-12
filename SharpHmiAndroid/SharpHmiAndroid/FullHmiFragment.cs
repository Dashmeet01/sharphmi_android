using System;
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
        LinearLayout mLinearLayout;
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

        public FullHmiFragment()
        {
            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.full_hmi_fragment, container,
                false);

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

            mLinearLayout = (LinearLayout)rootView.FindViewById(Resource.Id.full_hmi_linear_layout);

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
            mLinearLayout.RemoveViews(0, mLinearLayout.ChildCount);
        }

        internal void onUiPerformInteractionRequestCallback(PerformInteraction msg)
        {
            Activity.RunOnUiThread(() => UpdatePerformInteractionUI(msg));
        }

        private void UpdatePerformInteractionUI(PerformInteraction msg)
        {
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
                    mLinearLayout.AddView(button);
                }
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
    }
}