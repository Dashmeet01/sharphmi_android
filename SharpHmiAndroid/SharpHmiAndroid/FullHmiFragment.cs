using System;
using Android.App;
using Android.OS;
using Android.Views;

namespace SharpHmiAndroid
{
    public class FullHmiFragment : Fragment
    {
        int appID;
        public static readonly String sClickedAppID = "APP_ID";

        public FullHmiFragment()
        {
            SetHasOptionsMenu(true);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.full_hmi_fragment, container,
                false);

            appID = Arguments.GetInt(sClickedAppID);

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
    }
}