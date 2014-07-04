using System;
using System.Collections.Generic;
using System.IO;
namespace Phoenix.Core
{
	class ConfigurationData
	{
		public Dictionary<string, string> data;
		public ConfigurationData(string filePath)
		{
			this.data = new Dictionary<string, string>();
			if (!File.Exists(filePath))
			{
				throw new Exception("Unable to locate configuration file at '" + filePath + "'.");
			}
			try
			{
				using (StreamReader streamReader = new StreamReader(filePath))
				{
					string str;
					while ((str = streamReader.ReadLine()) != null)
					{
						if (str.Length >= 1 && !str.StartsWith("#"))
						{
							int num = str.IndexOf('=');
							if (num != -1)
							{
								string key = str.Substring(0, num);
								string value = str.Substring(num + 1);
								data.Add(key, value);
							}
						}
					}
					streamReader.Close();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Could not process configuration file: " + ex.Message);
			}
		}
	}
}
