using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HmiApiLib.Base;
using Java.Util;

namespace SharpHmiAndroid
{
	public class RpcMessageGetterInfo
	{
		public static StringBuilder appendSpaces(string sVal, char cval, int iNumSpaces)
		{
			StringBuilder sb = new StringBuilder(sVal);
			for (int i = 0; i < iNumSpaces; i++)
			{
				sb.Append("     ");
			}
			sb.Append(cval);
			sb.Append(' ');
			return sb;
		}

		public static string viewDetails(Object msg, Boolean bAppendSpace, int iNumSpaces)
		{
			StringBuilder sb = new StringBuilder();
			string sTemp;
			foreach (MethodInfo m in msg.GetType().GetMethods())
			{
				if ((m.Name.StartsWith("get")) &&
				    m.GetParameters().Length == 0)
				{
					if (m.Name.StartsWith("getClass") || m.Name.StartsWith("getBytes") || m.Name.StartsWith("getDeclaringClass")) continue;
					sb = appendSpaces(sb.ToString(), '-', iNumSpaces);
					sb.Append(m.Name);
					sb.Append(": ");
					try
					{
						Object r = m.Invoke(msg,null);

						if (r is RpcMessage)
						{
							sb.Append("\n");
							sb = appendSpaces(sb.ToString(), '+', iNumSpaces + 1);
							sb.Append(r.ToString());
							sb.Append("\n");
							sb.Append(viewDetails(r, (Boolean)true, iNumSpaces + 1));
						}
						else if (r is IList)
						{
							sb.Append("\n");
							sb = appendSpaces(sb.ToString(), '+', iNumSpaces + 1);
							sb.Append(r.ToString());
							sb.Append("\n");
							IList<Object> list = r as IList<Object>;

							foreach (Object key in list)
							{
								if (key is RpcMessage)
								{
									sTemp = viewDetails(key, (Boolean)true, iNumSpaces + 1);
									if (sTemp.Trim() != "")
									{
										sb.Append(sTemp);
										sb.Append("\n");
									}
								}
								else
								{
									sb = appendSpaces(sb.ToString(), '-', iNumSpaces + 1);
									sb.Append(key);
									sb.Append("\n");
								}
							}
						}
						else
						{
							sb.Append(r);
							sb.Append("\n");
						}

					}
					catch (IllegalFormatException e)
					{
						e.PrintStackTrace();
						sb.Append(e.Message);
						sb.Append("\n");
					}
					catch (Exception e)
					{
						sb.Append(e.Message);
						sb.Append("\n");
					}
				}
			}
			return sb.ToString();
		}
	}
}
