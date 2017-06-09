using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using System.Timers;
using Android.Content;
using Android.Widget;
using Android.Text;
using Android.Content.Res;
using Android.Support.V7.Preferences;

namespace SharpHmiAndroid
{
	[Activity(Label = "SharpHmiAndroid", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		NavigationView navigationView;
		DrawerLayout drawer;
		private AppSetting appSetting;
		private static string CONSOLE_FRAGMENT_TAG = "console_frag";
		private static string MAIN_FRAGMENT_TAG = "main_frag";

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
//			SupportActionBar.SetHomeButtonEnabled(true);
			SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

			SupportActionBar.SetDisplayHomeAsUpEnabled(true);

			drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

			ActionBarDrawerToggle _actionBarDrawerToggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open,
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

			setConsoleFragment();
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

			}		}

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
			FragmentManager fm = this.FragmentManager;
			fm.PopBackStackImmediate();
		}

		public void setMainFragment()
		{
			MainFragment mainFragment = getMainFragment();

			FragmentManager fragmentManager = this.FragmentManager;

			if (mainFragment == null)
			{
				mainFragment = new MainFragment();
				FragmentTransaction fragmentTransaction = fragmentManager.BeginTransaction();
				fragmentTransaction.Replace(Resource.Id.frame_container, mainFragment, MAIN_FRAGMENT_TAG).AddToBackStack(null).Commit();
				fragmentManager.ExecutePendingTransactions();
				this.SetTitle(Resource.String.app_name);
			}
		}

		public void setConsoleFragment()
		{

			ConsoleFragment consoleFragment = getConsoleFragment();

			FragmentManager fragmentManager = this.FragmentManager;

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
			builder.SetPositiveButton("OK", (senderAlert, args) => { 

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

			builder.SetNegativeButton("Cancel", (senderAlert, args) => {
				builder.Dispose();
			});

			builder.SetCancelable(true);
			builder.SetView(layout);
			dlg = builder.Create();
			dlg.Show();
		}

		public ConsoleFragment getConsoleFragment()
		{
			return (ConsoleFragment)this.FragmentManager.FindFragmentByTag(CONSOLE_FRAGMENT_TAG);
		}

		public MainFragment getMainFragment()
		{
			return (MainFragment)this.FragmentManager.FindFragmentByTag(MAIN_FRAGMENT_TAG);
		}
	}
}

