using System;
using System.Collections.Generic;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace SharpHmiAndroid
{
	public class AppListAdapter : RecyclerView.Adapter
	{
		private List<AppItem> AppList;
        public event EventHandler<int> ItemClick;

        public AppListAdapter(List<AppItem> appList)
		{
			AppList = appList;
		}

		public override int ItemCount
		{
			get
			{
				return AppList.Count;
			}
		}

		void OnClick(int position)
		{
			ItemClick?.Invoke(this, position);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			AppViewHolder appViewHolder = holder as AppViewHolder;
			appViewHolder.Caption.Text = AppList[position].getAppName();

			if (AppList[position].getAppIcon() != null)
			{
				appViewHolder.Image.SetImageBitmap(AppList[position].getAppIcon());
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
			public ImageView Image { get; private set; }
			public TextView Caption { get; private set; }

			public AppViewHolder(View itemView, Action<int> listener) : base(itemView)
			{
				Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);
				Caption = itemView.FindViewById<TextView>(Resource.Id.textView);

                itemView.Click += (sender, e) => listener(base.Position);
			}
		}
	}
}