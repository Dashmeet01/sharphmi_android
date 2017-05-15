using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;

namespace SharpHmiAndroid
{
	[Activity(Label = "SharpHmiAndroid", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		NavigationView navigationView;
		DrawerLayout drawer;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
			SetSupportActionBar(toolbar);
            SupportActionBar.SetHomeButtonEnabled(true);  
			SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.ic_menu);

			SupportActionBar.SetDisplayHomeAsUpEnabled(true);

			drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

			ActionBarDrawerToggle _actionBarDrawerToggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open,
			Resource.String.navigation_drawer_close);

			drawer.SetDrawerListener(_actionBarDrawerToggle);

			navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
			navigationView.NavigationItemSelected += OnNavigationItemSelected;
		}

		private void OnNavigationItemSelected(object sender, NavigationView.NavigationItemSelectedEventArgs e)
		{
			var menuItem = e.MenuItem;
			menuItem.SetChecked(!menuItem.IsChecked);
			drawer.CloseDrawers();

			switch (menuItem.ItemId)
			{
				case Resource.Id.consoleLogs:
					Android.Widget.Toast.MakeText(Application.Context, "Console Logs selected", Android.Widget.ToastLength.Long).Show();
					break;

				case Resource.Id.findApps:
					Android.Widget.Toast.MakeText(Application.Context, "Find New Apps selected", Android.Widget.ToastLength.Long).Show();
					break;

				case Resource.Id.nav_exit:
					Android.Widget.Toast.MakeText(Application.Context, "Exit App selected", Android.Widget.ToastLength.Long).Show();
					break;

				default:
					Android.Widget.Toast.MakeText(Application.Context, "Something Wrong", Android.Widget.ToastLength.Long).Show();
					break;

			}		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case Android.Resource.Id.Home:
					drawer.OpenDrawer(Android.Support.V4.View.GravityCompat.Start);
					return true;
			}
			return base.OnOptionsItemSelected(item);
		}
	}
}

