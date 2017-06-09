using System;
using Android.Content;
using Android.Support.V7.Preferences;

namespace SharpHmiAndroid
{
	public class AppSetting
	{
		private String sIPAddress = null;
		private String sTcpPort = null;
		private static Context appContext = null;
		private ISharedPreferences prefs = null;

		public AppSetting(Context appCon)
		{
			appContext = appCon;
			prefs = PreferenceManager.GetDefaultSharedPreferences(appContext);
		}

		public String getIPAddress()
		{
			if (prefs != null)
				if (sIPAddress == null)
					sIPAddress = prefs.GetString(Const.PREFS_KEY_TRANSPORT_IP,Const.PREFS_DEFAULT_TRANSPORT_IP);

			return sIPAddress;
		}

		public void setIPAddress(String sVal)
		{
			sIPAddress = sVal;
		}

		public string getTcpPort()
		{
			if (prefs != null)
			{
				if (sTcpPort == null)
				{
					int iTcpPort = prefs.GetInt(Const.PREFS_KEY_TRANSPORT_PORT, Const.PREFS_DEFAULT_TRANSPORT_PORT);
					sTcpPort = iTcpPort.ToString();
				}
			}

			return sTcpPort;
		}

		public void setTcpPort(string sVal)
		{
			sTcpPort = sVal;
		}
	}
}
