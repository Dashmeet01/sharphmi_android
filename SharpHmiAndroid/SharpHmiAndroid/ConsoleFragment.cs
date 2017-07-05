using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using HmiApiLib;
using HmiApiLib.Base;
using Newtonsoft.Json;

namespace SharpHmiAndroid
{
	public class ConsoleFragment : Fragment
	{
		private ListView _listview = null;
		public MessageAdapter _msgAdapter = null;
		private List<LogMessage> _logMessages = new List<LogMessage>();
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			if (AppInstanceManager.Instance.getMsgAdapter() == null)
			{
				_msgAdapter = new MessageAdapter(this.Activity, _logMessages);
				AppInstanceManager.Instance.setMsgAdapter(_msgAdapter);
			}
			else
			{
				_msgAdapter = AppInstanceManager.Instance.getMsgAdapter();
				_msgAdapter.updateActivity(this.Activity);
			}

			if (SdlService.instance == null)
			{
				var intent = new Intent((MainActivity)this.Activity, typeof(SdlService));
				((MainActivity)this.Activity).StartService(intent);
			}
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			View rootView = inflater.Inflate(Resource.Layout.console_fragment, container,
											 false);

			_listview = (ListView)rootView.FindViewById(Resource.Id.messageList);
			_listview.Clickable = true;

			_listview.Adapter = _msgAdapter;
			_listview.TranscriptMode = TranscriptMode.AlwaysScroll;

			_listview.ItemClick += listView_ItemClick;

			if (_listview.Adapter.Count > 10)
			{
				_listview.StackFromBottom = true;
			}
			return rootView;
		}

		private void showDialogWithBack(string sTitle, string sBody, Boolean isRpcResendAllowed, AlertDialog.Builder builder, View jsonLayout)
		{
			AlertDialog.Builder dialogNew = new AlertDialog.Builder(this.Activity);
			dialogNew.SetMessage(sBody);
			dialogNew.SetTitle("Show Getter Methods");
			dialogNew.SetPositiveButton("Back", (senderAlert, args) =>
				{

					if (jsonLayout != null)
					{
						ViewGroup parent = (ViewGroup)jsonLayout.Parent;
						if (parent != null)
						{
							parent.RemoveView(jsonLayout);
						}
					}
					AlertDialog alertDlg = builder.Create();
					alertDlg.Show();

					Button resendRpcAllowed = alertDlg.GetButton((int)DialogButtonType.Neutral);
					if (isRpcResendAllowed)
					{
						resendRpcAllowed.Enabled = true;
					}
					else
					{
						resendRpcAllowed.Enabled = false;
					}

				});
			AlertDialog ad = dialogNew.Create();
			ad.Show();
		}

		void listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
		{
			Object listObj = _msgAdapter[e.Position];

			if (listObj is RpcLogMessage)
			{
				LayoutInflater requestJSON = (LayoutInflater)this.Activity.GetSystemService(Context.LayoutInflaterService);
				View jsonLayout = requestJSON.Inflate(Resource.Layout.consolelogpreview, null);
				EditText jsonText = (EditText)jsonLayout.FindViewById(Resource.Id.consoleLogPreview_jsonContent);

				RpcMessage message = ((RpcLogMessage)listObj).getMessage();
				AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

				string rawJSON = "";
				int corrId = -1;
				string methodName = "";

				jsonText.Focusable = false;

				if (message is RpcRequest)
				{
					corrId = ((RpcRequest)message).getId();
					methodName = ((RpcRequest)message).method;
				}
				else if (message is RpcResponse)
				{
					corrId = ((RpcResponse)message).getId();
					methodName = ((HmiApiLib.Base.Result)((RpcResponse)message).result).method;
				}
				else if (message is RpcNotification)
				{
					methodName = ((RpcNotification)message).method;
				}
				else if (message is RequestNotifyMessage)
				{
					methodName = ((RequestNotifyMessage)message).method;
				}

				try
				{
					rawJSON = JsonConvert.SerializeObject(message, Formatting.Indented);
					builder.SetTitle("Raw JSON" + (corrId != -1 ? " (Corr ID " + corrId + ")" : ""));
				}
				catch (Exception ex)
				{
					try
					{
						rawJSON = methodName +
							" (" + message.getRpcMessageFlow().ToString().ToLower() + " " + message.getRpcMessageType().ToString().ToLower() + ")";
					}
					catch (Exception e1)
					{
						rawJSON = "Undefined";
					}
				}

				string finalJSON = rawJSON;

				jsonText.Text = finalJSON;

				builder.SetView(jsonLayout);

				builder.SetPositiveButton("Getters", (senderAlert, args) =>
				{
					string sInfo = RpcMessageGetterInfo.viewDetails(message, false, 0);
					Boolean isRpcResendAllowed = false;

					if (message.rpcMessageFlow == HmiApiLib.Common.Enums.RpcMessageFlow.OUTGOING)
					{
						isRpcResendAllowed = true;
					}

					showDialogWithBack("GetterInfo", sInfo, isRpcResendAllowed, builder, jsonLayout);
				});

				builder.SetNeutralButton("Resend", (senderAlert, args) =>
				{
					AppInstanceManager.Instance.sendRpc(message);
				});

				builder.SetNegativeButton("Cancel", (senderAlert, args) =>
				{
					builder.Dispose();
				});

				AlertDialog ad = builder.Create();
				ad.Show();

				Button resendRpc = ad.GetButton((int)DialogButtonType.Neutral);
				if (message.rpcMessageFlow == HmiApiLib.Common.Enums.RpcMessageFlow.OUTGOING) {
					resendRpc.Enabled = true;
				} else {
					resendRpc.Enabled = false;
				}

			}
			else if (listObj is StringLogMessage)
			{
				AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);
				string sMessageText = ((StringLogMessage)listObj).getData();
				if (sMessageText == "")
				{
					sMessageText = ((StringLogMessage)listObj).getMessage();
				}
				builder.SetMessage(sMessageText);
				builder.SetPositiveButton("OK", (senderAlert, args) =>
				{
					builder.Dispose();
				});
				AlertDialog ad = builder.Create();
				ad.Show();
			}
		}
	}
}
