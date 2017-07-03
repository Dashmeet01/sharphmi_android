using System;
using Android.Graphics;

namespace SharpHmiAndroid
{
    public class AppItem
    {
        private string AppName;
        private int AppID;
		private Bitmap appIcon;

		public void setAppIcon(Bitmap appIcon)
		{
			this.appIcon = appIcon;
		}

		public Bitmap getAppIcon()
		{
			return appIcon;
		}

        public string getAppName()
        {
            return AppName;
        }

        public int getAppID()
        {
            return AppID;
        }

        public AppItem(string appName, int appID)
        {
            AppName = appName;
            AppID = appID;
        }
    }
}