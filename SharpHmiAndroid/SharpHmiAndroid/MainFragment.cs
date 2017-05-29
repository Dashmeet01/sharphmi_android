
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SharpHmiAndroid
{
	public class MainFragment : Fragment
	{
		AppSetting appSetting = null;

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if(SdlService.instance == null) {
				var intent = new Intent((MainActivity)this.Activity, typeof(SdlService));
				((MainActivity) this.Activity).StartService(intent);
			}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View rootView = inflater.Inflate(Resource.Layout.main_fragment, container,
				false);
			return rootView;
			//return base.OnCreateView(inflater, container, savedInstanceState);
		}
	}
}
