using System;
namespace SharpHmiAndroid
{
	public class AppSetting
	{
		private String sIPAddress = null;
		private int iTcpPort = 8087;

		public AppSetting()
		{
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
