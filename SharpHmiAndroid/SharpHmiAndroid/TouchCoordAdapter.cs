using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using HmiApiLib.Common.Structs;

namespace SharpHmiAndroid
{
    public class TouchCoordAdapter : BaseAdapter<TouchCoord>
    {
        List<TouchCoord> touchCoordList;
        Activity context;

        public TouchCoordAdapter(Activity act, List<TouchCoord> list) : base()
        {
            touchCoordList = list;
            context = act;
        }

        public override TouchCoord this[int position] => touchCoordList[position];

        public override int Count => touchCoordList.Count;

        public override long GetItemId(int position)
        {
            return touchCoordList[position].getX();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
			var view = convertView ?? context.LayoutInflater.Inflate(
				Resource.Layout.touch_event_item_adapter, parent, false);

			var text = view.FindViewById<TextView>(Resource.Id.touch_event_count);
            text.Text = "X = " + touchCoordList[position].getX() + " ,Y = " + touchCoordList[position].getY();

			return view;
        }
    }
}
