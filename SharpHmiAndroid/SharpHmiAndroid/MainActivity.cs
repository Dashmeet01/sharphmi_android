using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Views;
using System.Timers;
using Android.Content;
using Android.Widget;
using Android.Text;
using Android.Support.V7.Preferences;
using System;
using Android;
using Android.Support.V4.App;
using Android.Content.PM;
using HmiApiLib.Controllers.UI.IncomingRequests;
using HmiApiLib.Common.Enums;
using HmiApiLib.Controllers.Buttons.IncomingNotifications;
using Android.Speech.Tts;
using HmiApiLib.Controllers.TTS.IncomingRequests;
using Android.Runtime;
using System.Collections.Generic;
using HmiApiLib.Common.Structs;

namespace SharpHmiAndroid
{
    [Activity(Label = "SharpHmiAndroid", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, AppUiCallback, ActivityCompat.IOnRequestPermissionsResultCallback, TextToSpeech.IOnInitListener
    {
        NavigationView navigationView;
        DrawerLayout drawer;
        private AppSetting appSetting;
        private static string CONSOLE_FRAGMENT_TAG = "console_frag";
        private static string MAIN_FRAGMENT_TAG = "main_frag";
        static string HMI_FULL_FRAGMENT_TAG = "hmi_full_frag";
        static string HMI_OPTIONS_MENU_FRAGMENT_TAG = "hmi_options_menu_frag";
        public const int REQUEST_STORAGE = 10;

		Handler mHandler;
		Action action;
        TextToSpeech textToSpeech;
        List<String> speechList = new List<string>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.splash_screen);

            if (!AppUtils.checkPermission(ApplicationContext, Manifest.Permission.WriteExternalStorage))
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage },
                                                  REQUEST_STORAGE);
            }
            else
            {
                mainActivityInitialization();
            }
        }

        public void mainActivityInitialization()
        {
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);
            //			SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

            Android.Support.V7.App.ActionBarDrawerToggle _actionBarDrawerToggle = new Android.Support.V7.App.ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open,
            Resource.String.navigation_drawer_close);

            drawer.SetDrawerListener(_actionBarDrawerToggle);

            navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.NavigationItemSelected += OnNavigationItemSelected;

            AppInstanceManager theInstance = AppInstanceManager.Instance;

            if ((AppInstanceManager.bRecycled == false) || (theInstance.getAppSetting() == null))
            { //Newly launched application.
                this.appSetting = new AppSetting(this);
                theInstance.setAppSetting(this.appSetting);
            }
            else
            {
                this.appSetting = theInstance.getAppSetting();
            }
            theInstance.setAppUiCallback(this);

            setConsoleFragment();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case REQUEST_STORAGE:
                    {
                        if (grantResults.Length > 0 && (grantResults[0] == (int)Permission.Granted))
                        {

                            mainActivityInitialization();
                        }
                        else
                        {
                            Toast.MakeText(this, "Permission Denied.", ToastLength.Long).Show();
                        }
                    }
                    break;
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            var inflater = MenuInflater;
            inflater.Inflate(Resource.Menu.main, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        private void OnNavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
        {
            var menuItem = e.MenuItem;
            menuItem.SetChecked(!menuItem.IsChecked);
            drawer.CloseDrawers();

            switch (menuItem.ItemId)
            {
                case Resource.Id.consoleLogs:
                    clearAllBackStackFragments();
                    Android.Widget.Toast.MakeText(Application.Context, "Console Logs selected", Android.Widget.ToastLength.Long).Show();
                    break;

                case Resource.Id.findApps:
                    setMainFragment();
                    Android.Widget.Toast.MakeText(Application.Context, "Find Apps selected", Android.Widget.ToastLength.Long).Show();
                    break;

                case Resource.Id.nav_exit:
                    exitApp();
                    Android.Widget.Toast.MakeText(Application.Context, "Exit App selected", Android.Widget.ToastLength.Long).Show();
                    break;

                default:
                    Android.Widget.Toast.MakeText(Application.Context, "Something is Wrong", Android.Widget.ToastLength.Long).Show();
                    break;

            }
        }

        /** Closes the activity and stops the proxy service. */
        private void exitApp()
        {
            Finish();

            var timer = new Timer();
            //What to do when the time elapses
            timer.Elapsed += (sender, args) => ExitAppCallback();
            //How often (5 sec)
            timer.Interval = 50;
            //Start it!
            timer.Enabled = true;
        }

        private void ExitAppCallback()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }

        /*Method for clearing out all backstack entries for fragments.*/
        public void clearAllBackStackFragments()
        {
            Android.App.FragmentManager fm = this.FragmentManager;
            fm.PopBackStackImmediate();
        }

        public void setMainFragment()
        {
            MainFragment mainFragment = getMainFragment();

            Android.App.FragmentManager fragmentManager = this.FragmentManager;

            if (mainFragment == null)
            {
                mainFragment = new MainFragment();
                Android.App.FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();
                fragmentTransaction.Replace(Resource.Id.frame_container, mainFragment, MAIN_FRAGMENT_TAG).AddToBackStack(null).Commit();
                fragmentManager.ExecutePendingTransactions();
                this.SetTitle(Resource.String.app_name);
            }
        }

        public void setConsoleFragment()
        {

            ConsoleFragment consoleFragment = getConsoleFragment();

            Android.App.FragmentManager fragmentManager = this.FragmentManager;

            if (consoleFragment == null)
            {
                consoleFragment = new ConsoleFragment();
                fragmentManager.BeginTransaction()
                               .Add(Resource.Id.frame_container, consoleFragment, CONSOLE_FRAGMENT_TAG).CommitAllowingStateLoss();
                fragmentManager.ExecutePendingTransactions();
                SetTitle(Resource.String.app_name);
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.settings:
                    settingsDialog();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public void settingsDialog()
        {
            Android.Support.V7.App.AlertDialog.Builder builder;
            Android.Support.V7.App.AlertDialog dlg;

            appSetting = AppInstanceManager.Instance.getAppSetting();

            LayoutInflater inflater = (LayoutInflater)GetSystemService(Context.LayoutInflaterService);
            View layout = inflater.Inflate(Resource.Layout.setting_option, null);

            EditText ipAddressEditText = layout
                .FindViewById(Resource.Id.selectprotocol_ipAddr) as EditText;
            EditText tcpPortEditText = (EditText)layout
                .FindViewById(Resource.Id.selectprotocol_tcpPort);

            string ipAddress = appSetting.getIPAddress();
            string tcpPort = appSetting.getTcpPort();


            builder = new Android.Support.V7.App.AlertDialog.Builder(this);

            ipAddressEditText.Text = ipAddress;
            tcpPortEditText.Text = tcpPort;

            string htmlString = "<b><u>SharpHmi Settings</u></b>";
            TextView title = new TextView(this);
            title.Gravity = GravityFlags.Center;
            title.Text = Html.FromHtml(htmlString).ToString();
            title.TextSize = 25;

            builder.SetCustomTitle(title);
            builder.SetPositiveButton("OK", (senderAlert, args) =>
            {

                if (ipAddressEditText.Length() != 0)
                {
                    appSetting.setIPAddress(ipAddressEditText.Text.ToString());
                }

                if (tcpPortEditText.Length() != 0)
                {
                    appSetting.setTcpPort(tcpPortEditText.Text.ToString());
                }

                string tmpIpAddress = appSetting.getIPAddress();
                string tmpTcpPortNumber = appSetting.getTcpPort();

                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

                prefs.Edit().PutString(Const.PREFS_KEY_TRANSPORT_IP, appSetting.getIPAddress())
                            .PutInt(Const.PREFS_KEY_TRANSPORT_PORT, int.Parse(appSetting.getTcpPort())).Commit();

                if ((ipAddress != tmpIpAddress) || (tcpPort != tmpTcpPortNumber))
                {
                    new System.Threading.Thread(new System.Threading.ThreadStart(() =>
                    {
                        AppInstanceManager.Instance.setupConnection(appSetting.getIPAddress(), int.Parse(appSetting.getTcpPort()));
                    })).Start();

                }

                builder.Dispose();
            });

            builder.SetNegativeButton("Cancel", (senderAlert, args) =>
            {
                builder.Dispose();
            });

            builder.SetCancelable(true);
            builder.SetView(layout);
            dlg = builder.Create();
            dlg.Show();
        }

        public ConsoleFragment getConsoleFragment()
        {
            return (ConsoleFragment)FragmentManager.FindFragmentByTag(CONSOLE_FRAGMENT_TAG);
        }

        public MainFragment getMainFragment()
        {
            return (MainFragment)this.FragmentManager.FindFragmentByTag(MAIN_FRAGMENT_TAG);
        }

        public FullHmiFragment getFullHMiFragment()
        {
            return (FullHmiFragment)this.FragmentManager.FindFragmentByTag(HMI_FULL_FRAGMENT_TAG);
        }

        public OptionsMenuFragment getOptionsMenuFragment()
        {
            return (OptionsMenuFragment)this.FragmentManager.FindFragmentByTag(HMI_OPTIONS_MENU_FRAGMENT_TAG);
        }

        public void onBcAppRegisteredNotificationCallback(Boolean isNewAppRegistered)
        {
            if (isNewAppRegistered)
            {
                if (getMainFragment() is MainFragmentCallback)
                {
                    getMainFragment().onRefreshCallback();
                }
            }
            else
            {
                if (getFullHMiFragment() != null && getFullHMiFragment().IsVisible)
                {
                    RunOnUiThread(() => RemoveFullFragment());
                }
                else
                {
                    if (getMainFragment() is MainFragmentCallback)
                    {
                        getMainFragment().onRefreshCallback();
                    }
                }
            }
        }

        public void setHmiFragment(int position)
        {
            int appId = AppInstanceManager.appList[position].getAppID();
            AppInstanceManager.Instance.sendActivateAppRequest(appId);

            FullHmiFragment hmiFragment = getFullHMiFragment();

            Android.App.FragmentManager fragmentManager = FragmentManager;

            if (hmiFragment == null)
            {
                hmiFragment = new FullHmiFragment();

                Bundle bundle = new Bundle();
                bundle.PutInt(FullHmiFragment.sClickedAppID, appId);
                hmiFragment.Arguments = bundle;

                Android.App.FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();
                fragmentTransaction.Replace(Resource.Id.frame_container, hmiFragment, HMI_FULL_FRAGMENT_TAG).AddToBackStack(null).Commit();
                fragmentManager.ExecutePendingTransactions();
                SetTitle(Resource.String.app_name);
            }
        }

        public void setOptionsFragment(int appID)
        {
            OptionsMenuFragment optionsMenuFragment = getOptionsMenuFragment();

            Android.App.FragmentManager fragmentManager = FragmentManager;

            if (optionsMenuFragment == null)
            {
                optionsMenuFragment = new OptionsMenuFragment();

                Bundle bundle = new Bundle();
                bundle.PutInt(FullHmiFragment.sClickedAppID, appID);
                optionsMenuFragment.Arguments = bundle;

                Android.App.FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();
                fragmentTransaction.Replace(Resource.Id.frame_container, optionsMenuFragment, HMI_OPTIONS_MENU_FRAGMENT_TAG).AddToBackStack(null).Commit();
                fragmentManager.ExecutePendingTransactions();
                SetTitle(Resource.String.app_name);
            }
        }

        private void RemoveFullFragment()
        {
            Android.App.FragmentManager fragmentManager = FragmentManager;
            fragmentManager.PopBackStack();
        }

        public void refreshOptionsMenu()
        {
            if ((getOptionsMenuFragment() != null) && (getOptionsMenuFragment().IsVisible))
            {
                getOptionsMenuFragment().onRefreshOptionsMenu();
            }
        }

        public void setDownloadedAppIcon()
        {
            if ((getMainFragment() != null) && (getMainFragment().IsVisible))
            {
                getMainFragment().onRefreshCallback();
            }
        }

        public override void OnBackPressed()
        {
            if ((getOptionsMenuFragment() != null) && (getOptionsMenuFragment().IsVisible))
            {
                if (getOptionsMenuFragment().OnBackPressed())
                {
                    base.OnBackPressed();
                }
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public void onUiShowRequestCallback(Show msg)
        {
            if ((getFullHMiFragment() != null) && (getFullHMiFragment().IsVisible))
            {
                getFullHMiFragment().onUiShowRequestCallback(msg);
            }
        }

        public void onUiAlertRequestCallback(Alert msg)
        {
            RunOnUiThread(() => UpdateAlertUI(msg));
        }

        private void UpdateAlertUI(Alert msg)
        {
            Android.Support.V7.App.AlertDialog.Builder alertBuilder = new Android.Support.V7.App.AlertDialog.Builder(this);
            LayoutInflater inflater = LayoutInflater;
            View view = inflater.Inflate(Resource.Layout.custom_alert_dialog, null);
            alertBuilder.SetView(view);

            TextView alertText1 = (TextView)view.FindViewById(Resource.Id.alert_text_1);
            TextView alertText2 = (TextView)view.FindViewById(Resource.Id.alert_text_2);
            TextView alertText3 = (TextView)view.FindViewById(Resource.Id.alert_text_3);

            Button softButton1 = (Button)view.FindViewById(Resource.Id.alert_soft_btn_1);
            Button softButton2 = (Button)view.FindViewById(Resource.Id.alert_soft_btn_2);
            Button softButton3 = (Button)view.FindViewById(Resource.Id.alert_soft_btn_3);
            Button softButton4 = (Button)view.FindViewById(Resource.Id.alert_soft_btn_4);

            if ((msg.getAlertStrings() != null) && (msg.getAlertStrings().Count > 0))
            {
                for (int i = 0; i < msg.getAlertStrings().Count; i++)
                {
                    switch (msg.getAlertStrings()[i].fieldName)
                    {
                        case TextFieldName.alertText1:
                            alertText1.Text = msg.getAlertStrings()[i].fieldText;
                            break;
                        case TextFieldName.alertText2:
                            alertText2.Text = msg.getAlertStrings()[i].fieldText;
                            break;
                        case TextFieldName.alertText3:
                            alertText3.Text = msg.getAlertStrings()[i].fieldText;
                            break;
                        default:
                            break;
                    }
                }
            }

            if ((msg.getSoftButtons() != null) && (msg.getSoftButtons().Count > 0))
            {
                for (int i = 0; i < msg.getSoftButtons().Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            softButton1.Text = msg.getSoftButtons()[i].getText();
                            softButton1.Visibility = ViewStates.Visible;
                            break;
                        case 1:
                            softButton2.Text = msg.getSoftButtons()[i].getText();
                            softButton2.Visibility = ViewStates.Visible;
                            break;
                        case 2:
                            softButton3.Text = msg.getSoftButtons()[i].getText();
                            softButton3.Visibility = ViewStates.Visible;
                            break;
                        case 3:
                            softButton4.Text = msg.getSoftButtons()[i].getText();
                            softButton4.Visibility = ViewStates.Visible;
                            break;
                        default:
                            break;
                    }
                }
            }

            alertBuilder.SetPositiveButton("ok", (senderAlert, args) =>
            {
                alertBuilder.Dispose();
            });
            Android.Support.V7.App.AlertDialog ad = alertBuilder.Create();
            ad.Show();

			int? totalDuration = (int)msg.getDuration();
            if (totalDuration != null)
            {
				mHandler = new Handler(Looper.MainLooper);
				action = delegate
				{
					ad.Cancel();
				};
				if (null != mHandler)
					mHandler.PostDelayed(action, (long)totalDuration);
            }
        }

        public void onUiScrollableMessageRequestCallback(ScrollableMessage msg)
        {
            if ((getFullHMiFragment() != null) && (getFullHMiFragment().IsVisible))
            {
                getFullHMiFragment().onUiScrollableMessageRequestCallback(msg);
            }
        }

        public void onUiPerformInteractionRequestCallback(PerformInteraction msg)
        {
            if ((getFullHMiFragment() != null) && (getFullHMiFragment().IsVisible))
            {
                getFullHMiFragment().onUiPerformInteractionRequestCallback(msg);
            }
        }

        public void onUiSetMediaClockTimerRequestCallback(SetMediaClockTimer msg)
        {
            if ((getFullHMiFragment() != null) && (getFullHMiFragment().IsVisible))
            {
                getFullHMiFragment().onUiSetMediaClockTimerRequestCallback(msg);
            }
        }

        public void OnButtonSubscriptionNotificationCallback(OnButtonSubscription msg)
        {
			if ((getFullHMiFragment() != null) && (getFullHMiFragment().IsVisible))
			{
				getFullHMiFragment().OnButtonSubscriptionNotificationCallback(msg);
			}
        }

        public void onTtsSpeakRequestCallback(Speak msg)
        {
            if ((msg.getTtsChunkList() != null) && (msg.getTtsChunkList().Count > 0))
            {
                foreach(TTSChunk item in msg.getTtsChunkList())
                {
                    if (item.type == SpeechCapabilities.TEXT)
                    {
                        speechList.Add(item.text);
                    }
                }
            }
            RunOnUiThread(() => HandleSpeakRequest());
        }

        private void HandleSpeakRequest()
        {
			textToSpeech = new TextToSpeech(this, this);
        }

        public void OnInit([GeneratedEnum] OperationResult status)
        {
            if (status != OperationResult.Error)
            {
                textToSpeech.SetLanguage(Java.Util.Locale.Us);
                for (int i = 0; i < speechList.Count; i++)
                {
                    textToSpeech.Speak(speechList[i], QueueMode.Add, null, null);
                }
                speechList.Clear();
            }
        }

        public void onUiSliderRequestCallback(Slider msg)
        {
            if ((getFullHMiFragment() != null) && (getFullHMiFragment().IsVisible))
            {
                getFullHMiFragment().onUiSliderRequestCallback(msg);
            }
        }
    }
}