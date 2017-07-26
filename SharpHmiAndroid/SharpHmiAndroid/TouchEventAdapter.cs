using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using HmiApiLib.Common.Structs;

namespace SharpHmiAndroid
{
    public class TouchEventAdapter : BaseAdapter<TouchEvent>
    {
        List<TouchEvent> touchEventList;
        Activity context;

        public TouchEventAdapter(Activity act, List<TouchEvent> list) : base()
        {
            touchEventList = list;
            context = act;
        }

        public override TouchEvent this[int position] => touchEventList[position];

        public override int Count => touchEventList.Count;

        public override long GetItemId(int position)
        {
            return touchEventList[position].getId();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
			var view = convertView ?? context.LayoutInflater.Inflate(
                Resource.Layout.touch_event_item_adapter, parent, false);

            var text = view.FindViewById<TextView>(Resource.Id.touch_event_count);
            text.Text = touchEventList[position].getId().ToString();

            return view;
        }
    }
}
