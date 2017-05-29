﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SharpHmiAndroid
{
	public class ConsoleFragment : Fragment
	{
		private ListView _listview = null;
		public override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Create your fragment here
		}

		public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
		{
			// Use this to return your custom view for this Fragment
			// return inflater.Inflate(Resource.Layout.YourFragment, container, false);

			View rootView = inflater.Inflate(Resource.Layout.console_fragment, container,
											 false);

			_listview = (ListView) rootView.FindViewById(Resource.Id.messageList);
			_listview.Clickable = true;
			_listview.Adapter = AppInstanceManager.Instance._msgAdapter;
			_listview.TranscriptMode = TranscriptMode.AlwaysScroll;

			if (AppInstanceManager.Instance._msgAdapter.Count > 10)
			{
				_listview.StackFromBottom = true;
			}
			return rootView;
			//return base.OnCreateView(inflater, container, savedInstanceState);
		}
	}
}
