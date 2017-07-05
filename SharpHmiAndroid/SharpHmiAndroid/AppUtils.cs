using System;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.Content;

namespace SharpHmiAndroid
{
	public class AppUtils
	{
		public static bool checkPermission(Context context, string permission)
		{
			if (ContextCompat.CheckSelfPermission(context, permission) == (int)Permission.Granted)
			{
				return true;
			}
			return false;
		}
	}
}