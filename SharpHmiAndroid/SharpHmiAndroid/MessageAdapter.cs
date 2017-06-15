using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using HmiApiLib;
using HmiApiLib.Base;
using HmiApiLib.Common.Enums;

namespace SharpHmiAndroid
{
	public class MessageAdapter : BaseAdapter<LogMessage>
	{
		private List<LogMessage> items;
		private LayoutInflater vi;
		private Activity activity;
		public MessageAdapter(Activity context, List<LogMessage> items) : base()
		{
			this.vi = (LayoutInflater)context
				.GetSystemService(Context.LayoutInflaterService);
			this.items = items;
			this.activity = context;
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
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void addMessage(LogMessage m)
		{
			if (this.activity == null) return;

			this.activity.RunOnUiThread(() => addMessageAndNotify(m));
		}

		public void addMessageAndNotify(LogMessage logMessage)
		{
			items.Add(logMessage);
			NotifyDataSetChanged();
		}

		public override int Count
		{
			get
			{
				return this.items.Count;
			}
		}

		public override LogMessage this[int position]
		{
			get
			{
				return items[position];
			}
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
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

			LogMessage rpcObj = items[position];
			if (rpcObj != null)
			{
				if (rpcObj is StringLogMessage)
				{
					StringLogMessage myStringLog = (StringLogMessage)rpcObj;

					lblTop.Text = myStringLog.getMessage();
					lblTime.Text = rpcObj.getDate();
					lblTop.SetTextColor(Color.Black);
				}
				else if (rpcObj is RpcLogMessage)
				{
					RpcMessage func = ((RpcLogMessage)rpcObj).getMessage();

					if (func.getRpcMessageType() == RpcMessageType.REQUEST)
					{
						lblTop.SetTextColor(Color.Blue);
						lblTop.Text = rpcObj.getPrependComment() + ((RpcRequest)func).method + " ("+ func.getRpcMessageFlow().ToString().ToLower() + " " + func.getRpcMessageType().ToString().ToLower() + ")";
					}
					else if (func.getRpcMessageType() == RpcMessageType.RESPONSE)
					{
						lblTop.SetTextColor(Color.GreenYellow);
						lblTop.Text = rpcObj.getPrependComment() + ((HmiApiLib.Base.Result)((RpcResponse)func).result).method + " (" + func.getRpcMessageFlow().ToString().ToLower()	+ " " + func.getRpcMessageType().ToString().ToLower() + ")";
					}
					else if (func.getRpcMessageType() == RpcMessageType.NOTIFICATION)
					{
						lblTop.SetTextColor(Color.DarkCyan);
						lblTop.Text = rpcObj.getPrependComment() + ((RpcNotification)func).method + " (" + func.getRpcMessageFlow().ToString().ToLower() + " " + func.getRpcMessageType().ToString().ToLower() + ")";
					}
					else if (func.getRpcMessageType() == RpcMessageType.REQUEST_NOTIFY)
					{
						lblTop.SetTextColor(Color.Cyan);
						lblTop.Text = rpcObj.getPrependComment() + ((RequestNotifyMessage)func).method + " (" + func.getRpcMessageFlow().ToString().ToLower() + " " + func.getRpcMessageType().ToString().ToLower() + ")";
					}

					lblTime.Text = rpcObj.getDate();
				}
				lblBottom.Visibility = ViewStates.Gone;
			}
			return rowView;
		}

		public override long GetItemId(int position)
		{
			return position;
		}
	}
}
