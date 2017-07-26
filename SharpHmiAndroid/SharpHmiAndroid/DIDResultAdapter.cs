using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using HmiApiLib.Common.Structs;

namespace SharpHmiAndroid
{
    public class DIDResultAdapter : BaseAdapter<DIDResult>
    {
        List<DIDResult> didResultList;
        Activity context;

        public DIDResultAdapter(Activity act, List<DIDResult> list) : base()
        {
            didResultList = list;
            context = act;
        }

        public override DIDResult this[int position] => didResultList[position];

        public override int Count => didResultList.Count;

        public override long GetItemId(int position)
        {
            return didResultList[position].didLocation;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
			var view = convertView ?? context.LayoutInflater.Inflate(
				Resource.Layout.touch_event_item_adapter, parent, false);

			var text = view.FindViewById<TextView>(Resource.Id.touch_event_count);
            text.Text = didResultList[position].getDidLocation().ToString();

			return view;
        }
    }
}
