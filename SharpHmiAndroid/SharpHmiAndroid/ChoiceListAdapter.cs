using System;
using System.Collections.Generic;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using HmiApiLib.Base;
using HmiApiLib.Common.Structs;
using HmiApiLib.Controllers.UI.IncomingRequests;

namespace SharpHmiAndroid
{
    public class ChoiceListAdapter : RecyclerView.Adapter
    {
        List<Choice> requestList;
		public event EventHandler<int> ItemClick;

        public ChoiceListAdapter(List<Choice> list)
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
			ItemClick?.Invoke(this, position);
		}

		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			AppViewHolder appViewHolder = holder as AppViewHolder;
            Choice choice = requestList[position];
            if (choice.getImage() != null)
            {
                appViewHolder.Image.Visibility = ViewStates.Visible;
                Bitmap image = BitmapFactory.DecodeStream(AppInstanceManager.Instance.getPutfile(choice.getImage().getValue()));
                appViewHolder.Image.SetImageBitmap(image);
            }
            else
            {
                appViewHolder.Image.Visibility = ViewStates.Invisible;
            }
            appViewHolder.Caption.Text = choice.getMenuName();
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
