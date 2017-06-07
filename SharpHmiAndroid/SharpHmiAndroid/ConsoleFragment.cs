using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using HmiApiLib;

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

			_listview = (ListView) rootView.FindViewById(Resource.Id.messageList);
			_listview.Clickable = true;


			_listview.Adapter = _msgAdapter;
			_listview.TranscriptMode = TranscriptMode.AlwaysScroll;

			if (_listview.Adapter.Count > 10)
			{
				_listview.StackFromBottom = true;
			}
			return rootView;
		}
	}
}
