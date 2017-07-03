using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using HmiApiLib.Base;

namespace SharpHmiAndroid
{
    public class OptionsMenuFragment : Fragment, HmiOptionsFragmentCallback
    {
		RecyclerView mRecyclerView;
		RecyclerView.LayoutManager mLayoutManager;
		OptionsMenuAdapter mAdapter;
		List<RpcRequest> appList = new List<RpcRequest>();
        int appID;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
			View rootView = inflater.Inflate(Resource.Layout.main_fragment, container,
				false);

            appID = Arguments.GetInt(FullHmiFragment.sClickedAppID);

			mRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			mLayoutManager = new LinearLayoutManager(Application.Context);
			mRecyclerView.SetLayoutManager(mLayoutManager);

			appList.Clear();
			appList.AddRange(AppInstanceManager.menuOptionListUi[appID]);

            mAdapter = new OptionsMenuAdapter(appList);

			mAdapter.ItemClick += OnItemClick;
			mRecyclerView.SetAdapter(mAdapter);
			return rootView;
        }

		void OnItemClick(object sender, int position)
		{
			
		}

		private void UpdateAdapter()
		{
			appList.Clear();
			appList.AddRange(AppInstanceManager.menuOptionListUi[appID]);
			mAdapter.NotifyDataSetChanged();
		}

        public void onRefreshOptionsMenu()
        {
            Activity.RunOnUiThread(() => UpdateAdapter());
        }
    }
}
