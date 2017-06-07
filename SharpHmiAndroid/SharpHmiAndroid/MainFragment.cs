using Android.App;
using Android.OS;
using Android.Views;

namespace SharpHmiAndroid
{
	public class MainFragment : Fragment
	{

		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View rootView = inflater.Inflate(Resource.Layout.main_fragment, container,
				false);
			return rootView;
		}
	}
}
