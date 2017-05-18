using System;
using Android.Content;
using Android.Support.V7.Preferences;

namespace SharpHmiAndroid
{
	public class AppSetting
	{
		private String sIPAddress = null;
		private int iTcpPort = 8087;
		private static Context appContext = null;
		private ISharedPreferences prefs = null;

		public AppSetting(Context appCon)
		{
			appContext = appCon;
			prefs = PreferenceManager.GetDefaultSharedPreferences(appContext);
		}

		public String getIPAddress()
		{
			return sIPAddress;
		}

		public void setIPAddress(String sVal)
		{
			sIPAddress = sVal;
		}

		public int getTcpPort()
		{
			return iTcpPort;
		}

		public void setTcpPort(int iVal)
		{
			iTcpPort = iVal;
		}
	}
}
