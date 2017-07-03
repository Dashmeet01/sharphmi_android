using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using HmiApiLib.Base;
using HmiApiLib.Controllers.UI.IncomingRequests;

namespace SharpHmiAndroid
{
    public class OptionsMenuAdapter : RecyclerView.Adapter
    {
        private List<RpcRequest> requestList;
		public event EventHandler<int> ItemClick;

        public OptionsMenuAdapter(List<RpcRequest> list)
		{
			requestList = list;
		}

		public override int ItemCount
		{
			get
			{
				return requestList.Count;
			}
		}

		void OnClick(int position)
		{
			if (ItemClick != null)
				ItemClick(this, position);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			AppViewHolder appViewHolder = holder as AppViewHolder;
            RpcRequest req = requestList[position];
            if ((req is AddCommand) && (((AddCommand)req).getMenuParams().getParentID() == 0))
            {
                appViewHolder.SubmenuIcon.Visibility = ViewStates.Gone;
                appViewHolder.CommandText.Text = ((AddCommand)req).getMenuParams().menuName;
            }
            else if (req is AddSubMenu)
            {
                appViewHolder.SubmenuIcon.Visibility = ViewStates.Visible;
                appViewHolder.CommandText.Text = ((AddSubMenu)req).getMenuParams().menuName;
            }
		}

		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
		{
			View itemView = LayoutInflater.From(parent.Context).
										  Inflate(Resource.Layout.AppListItem, parent, false);
			AppViewHolder vh = new AppViewHolder(itemView, OnClick);
			return vh;
		}


		public class AppViewHolder : RecyclerView.ViewHolder
		{
			public ImageView CommandIcon { get; private set; }
			public TextView CommandText { get; private set; }
            public ImageView SubmenuIcon { get; private set; }

			public AppViewHolder(View itemView, Action<int> listener) : base(itemView)
			{
				CommandIcon = itemView.FindViewById<ImageView>(Resource.Id.imageView);
				CommandText = itemView.FindViewById<TextView>(Resource.Id.textView);
                SubmenuIcon = itemView.FindViewById<ImageView>(Resource.Id.options_menu_icon);

				itemView.Click += (sender, e) => listener(base.Position);
			}
		}
    }
}
