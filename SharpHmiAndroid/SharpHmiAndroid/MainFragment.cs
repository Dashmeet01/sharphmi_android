using System;
using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using HmiApiLib;
using HmiApiLib.Manager;

namespace SharpHmiAndroid
{
	public class MainFragment : Fragment, MainFragmentCallback
	{
		RecyclerView mRecyclerView;
		RecyclerView.LayoutManager mLayoutManager;
		AppListAdapter mAdapter;
		List<AppItem> appList = new List<AppItem>();

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View rootView = inflater.Inflate(Resource.Layout.main_fragment, container,
				false);

			mRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
			mLayoutManager = new LinearLayoutManager(Application.Context);
			mRecyclerView.SetLayoutManager(mLayoutManager);

			appList.Clear();
			appList.AddRange(AppInstanceManager.appList);

            mAdapter = new AppListAdapter(appList);
            mAdapter.ItemClick += OnItemClick;
			mRecyclerView.SetAdapter(mAdapter);
			return rootView;
		}
        		
		public void onRefreshCallback()
		{
            Activity.RunOnUiThread(() => UpdateAdapter());
		}

        void UpdateAdapter()
        {
			appList.Clear();
			appList.AddRange(AppInstanceManager.appList);
			mAdapter.NotifyDataSetChanged();
        }

		void OnItemClick(object sender, int position)
		{
            int appId = AppInstanceManager.appList[position].getAppID();
            ((MainActivity)Activity).setHmiFragment(position);
		}
	}
}