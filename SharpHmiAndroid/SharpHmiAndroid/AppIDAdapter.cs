using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;

namespace SharpHmiAndroid
{
    public class AppIDAdapter : BaseAdapter<AppItem>
    {
        List<AppItem> appIDList;
        Activity context;

        public AppIDAdapter(Activity act, List<AppItem> list) : base()
        {
            appIDList = list;
            context = act;
        }

        public override AppItem this[int position] => appIDList[position];

        public override int Count => appIDList.Count;

        public override long GetItemId(int position)
        {
            return appIDList[position].getAppID();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? context.LayoutInflater.Inflate(
                Resource.Layout.touch_event_item_adapter, parent, false);

            var text = view.FindViewById<TextView>(Resource.Id.touch_event_count);
            View separator = view.FindViewById(Resource.Id.sepatator_view);

            text.Text = appIDList[position].getAppName() + "\n"
                + appIDList[position].getAppID();
            
            if ((Count > 0) && (position == Count - 1))
            {
                separator.Visibility = ViewStates.Gone;
            }
            else
            {
                separator.Visibility = ViewStates.Visible;
            }

            return view;
        }
    }
}