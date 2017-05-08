using System;
using System.Runtime.CompilerServices;

namespace SharpHmiAndroid
{
	public class AppInstanceManager
	{
		private static AppInstanceManager instance;
		private static CoreListener coreListener;

		public static Boolean bRecycled = false;

		public static Boolean appResumed = false;

		private AppSetting appSetting = null;

		public AppInstanceManager()
		{
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static AppInstanceManager getInstance()
		{
			if (instance == null)
			{
				instance = new AppInstanceManager();
			}
			else
			{
				bRecycled = true;
			}

			return instance;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void setAppSetting(AppSetting appSetting)
		{
			this.appSetting = appSetting;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public AppSetting getAppSetting()
		{
			return this.appSetting;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void clearInstance()
		{
			removeCoreListener();
			instance = null;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void setCoreListener(CoreListener listener)
		{
			coreListener = listener;
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public void removeCoreListener()
		{
			if (coreListener != null)
			{

				coreListener = null;
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public CoreListener getCoreListener()
		{

			return coreListener;
		}

	}
}
