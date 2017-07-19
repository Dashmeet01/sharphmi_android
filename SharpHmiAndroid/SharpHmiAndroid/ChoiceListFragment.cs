using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using HmiApiLib.Common.Structs;
using HmiApiLib.Controllers.UI.IncomingRequests;

namespace SharpHmiAndroid
{
    public class ChoiceListFragment : Fragment
    {
        int appID;
		RecyclerView mRecyclerView;
		RecyclerView.LayoutManager mLayoutManager;
		ChoiceListAdapter mAdapter;
        PerformInteraction msg;
        List<Choice> choiceList = new List<Choice>();
        TextView initialText;

        public ChoiceListFragment()
        {
        }

        public ChoiceListFragment(PerformInteraction rpc)
        {
            this.msg = rpc;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
            View rootView = inflater.Inflate(Resource.Layout.choice_set_layout, container, false);

            initialText = rootView.FindViewById<TextView>(Resource.Id.perform_interaction_initial_text);
			mRecyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.recycler_view_choice_set);
			mLayoutManager = new LinearLayoutManager(Application.Context);
			mRecyclerView.SetLayoutManager(mLayoutManager);

            initialText.Text = msg.getInitialText().fieldText;

			choiceList.Clear();
            choiceList = msg.getChoiceSet();

			mAdapter = new ChoiceListAdapter(choiceList);

			mAdapter.ItemClick += OnItemClick;
			mRecyclerView.SetAdapter(mAdapter);

			return rootView;
		}

        void OnItemClick(object sender, int position)
        {
			
        }

        private void UpdateAdapter()
        {
            initialText.Text = msg.getInitialText().fieldText;
			choiceList.Clear();
			choiceList = msg.getChoiceSet();
			mAdapter.NotifyDataSetChanged();
        }

        internal void onUiPerformInteractionRequestCallback(PerformInteraction rpc)
        {
            Activity.RunOnUiThread(() => UpdatePerformInteractionUI(rpc));
        }

        private void UpdatePerformInteractionUI(PerformInteraction rpc)
        {
			this.msg = rpc;
			UpdateAdapter();
        }
    }
}
