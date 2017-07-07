using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using HmiApiLib.Base;
using HmiApiLib.Controllers.UI.IncomingRequests;

namespace SharpHmiAndroid
{
    public class OptionsMenuFragment : Fragment, HmiOptionsFragmentCallback
    {
        RecyclerView mRecyclerView;
        RecyclerView.LayoutManager mLayoutManager;
        OptionsMenuAdapter mAdapter;
        List<RpcRequest> appList = new List<RpcRequest>();
        int appID;
        bool isMainScreenDisplayed;
        int selectedSubmenuID;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.main_fragment, container,
                false);

            appID = Arguments.GetInt(FullHmiFragment.sClickedAppID);

            mRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            mLayoutManager = new LinearLayoutManager(Application.Context);
            mRecyclerView.SetLayoutManager(mLayoutManager);

            appList.Clear();

            mAdapter = new OptionsMenuAdapter(appList);

            mAdapter.ItemClick += OnItemClick;
            mRecyclerView.SetAdapter(mAdapter);

            UpdateAdapter();
            return rootView;
        }

        void OnItemClick(object sender, int position)
        {
            RpcRequest request = AppInstanceManager.menuOptionListUi[appID][position];
            if (request is AddSubMenu)
            {
                selectedSubmenuID = (int)((AddSubMenu)request).getMenuID();
                appList.Clear();

                foreach (RpcRequest req in AppInstanceManager.menuOptionListUi[appID])
                {
                    if (req is AddCommand)
                    {
                        int parentID = ((AddCommand)req).getMenuParams().parentID;
                        if (parentID == selectedSubmenuID)
                        {
                            appList.Add(req);
                        }
                    }
                }
                mAdapter.NotifyDataSetChanged();
                isMainScreenDisplayed = false;
            }
        }

        private void UpdateAdapter()
        {
            appList.Clear();
            foreach (RpcRequest request in AppInstanceManager.menuOptionListUi[appID])
            {
                if (request is AddCommand)
                {
                    int parentID = ((AddCommand)request).getMenuParams().parentID;
                    if (parentID == 0)
                    {
                        appList.Add(request);
                    }
                }
                if (request is AddSubMenu)
                {
                    appList.Add(request);
                }
            }
            isMainScreenDisplayed = true;
            mAdapter.NotifyDataSetChanged();
        }

        public void onRefreshOptionsMenu()
        {
            Activity.RunOnUiThread(() => UpdateAdapter());
        }

        internal bool OnBackPressed()
        {
            if (!isMainScreenDisplayed)
            {
                UpdateAdapter();
                return false;
            }
            return true;
        }
    }
}
