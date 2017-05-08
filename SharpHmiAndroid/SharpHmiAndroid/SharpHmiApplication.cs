using System;
using Android.App;

namespace SharpHmiAndroid
{
	public class SharpHmiApplication : Application
	{
		private static SharpHmiApplication mInstance;
		public SharpHmiApplication()
		{
		}

		public override void OnCreate()
		{
			mInstance = this;
		}
	}
}
