using System;
using System.Collections;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using HmiApiLib;

namespace SharpHmiAndroid
{
	public class MessageAdapter : ArrayAdapter<LogMessage>
	{
		private List<LogMessage> items;
		private LayoutInflater vi;
		public MessageAdapter(Context context, int textViewResourceId, List<LogMessage> items) : base(context, textViewResourceId, items)
		{
            this.vi = (LayoutInflater)context
				.GetSystemService(Context.LayoutInflaterService);
			this.items = items;
		}

		public List<LogMessage> getLogMessageList()
		{
			return this.items;
		}

		class ViewHolder : Java.Lang.Object
		{
			public TextView lblTop;
			public TextView lblBottom;
			public TextView lblTime;
		}

		/** Adds the specified message to the items list and notifies of the change. */
		public void addMessage(LogMessage m)
		{
			Add(m);
		}

		public override int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		public View getView(int position, View convertView, ViewGroup parent)
		{
			ViewHolder holder = null;
			TextView lblTop = null;
			TextView lblBottom = null;
			TextView lblTime = null;

			ViewGroup rowView = (ViewGroup)convertView;
			if (rowView == null)
			{
				rowView = (ViewGroup)vi.Inflate(Resource.Layout.row, null);

				lblTop = (TextView)rowView.FindViewById(Resource.Id.toptext);
				lblBottom = (TextView)rowView.FindViewById(Resource.Id.bottomtext);
				lblTime = (TextView)rowView.FindViewById(Resource.Id.text_date_time);

				holder = new ViewHolder();
				holder.lblTop = lblTop;
				holder.lblBottom = lblBottom;
				holder.lblTime = lblTime;
				rowView.Tag = holder;
			}
			else
			{
				holder = rowView.Tag as ViewHolder;
				lblTop = holder.lblTop;
				lblBottom = holder.lblBottom;
				lblTime = holder.lblTime;

				lblBottom.Visibility = ViewStates.Visible;
				lblBottom.Text = "";
				lblTop.SetTextColor(rowView.Context.Resources.GetColor(
					Resource.Color.log_regular_text_color));
				lblTop.Text = "";
			}

			LogMessage logObj = GetItem(position);
			if (logObj != null)
			{
				lblTop.Text = logObj.getMessage();
				lblTime.Text = logObj.getDate();
				lblTop.SetTextColor(Color.Black);
			}

			return rowView;
		}
	}
}
