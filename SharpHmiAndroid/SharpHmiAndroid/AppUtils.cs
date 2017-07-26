﻿﻿using System;
using Android.Content;
using Android.Content.PM;
using Android.Preferences;
using Android.Support.V4.Content;
using HmiApiLib.Base;

namespace SharpHmiAndroid
{
	public class AppUtils
	{
        private static string separator = "_";

        public static bool checkPermission(Context context, string permission)
		{
			if (ContextCompat.CheckSelfPermission(context, permission) == (int)Permission.Granted)
			{
				return true;
			}
			return false;
		}

		public static RpcMessage getSavedPreferenceValueForRpc<T>(Context ctx, String key, int appId)
		{
			if ((ctx == null) || (key == null))
				return null;

			String json = PreferenceManager.GetDefaultSharedPreferences(ctx).GetString(appId + separator + key, null);

			if (json == null)
				return null;

			object msg = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);

			return (RpcMessage)Convert.ChangeType(msg, typeof(T));
		}

		public static void savePreferenceValueForRpc(Context ctx, String key, RpcMessage rpcMessage, int appId)
		{
			if ((rpcMessage == null) || (key == null) || (ctx == null))
				return;

			string json = Newtonsoft.Json.JsonConvert.SerializeObject(rpcMessage);
			PreferenceManager.GetDefaultSharedPreferences(ctx).Edit().PutString(appId + separator + key, json).Commit();
		}

		public static void removeSavedPreferenceValueForRpc(Context ctx, String key, int appId)
		{
			if ((ctx == null) || (key == null))
				return;

			PreferenceManager.GetDefaultSharedPreferences(ctx).Edit().Remove(appId + separator + key).Commit();
		}
	}
}