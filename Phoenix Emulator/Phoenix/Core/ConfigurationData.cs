using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Core
{
    class ConfigurationData
    {
        internal Dictionary<string, string> data = new Dictionary<string, string>();
        internal bool fileHasBeenRead = false;

        internal ConfigurationData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException("Unable to locate configuration file at '" + filePath + "'.");
            }
            else
            {
                fileHasBeenRead = true;
                try
                {
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        string str;
                        while ((str = reader.ReadLine()) != null)
                        {
                            if ((str.Length >= 1) && !str.StartsWith("#"))
                            {
                                int i = str.IndexOf('=');
                                if (i != -1)
                                {
                                    string key = str.Substring(0, i);
                                    string str2 = str.Substring(i + 1);
                                    data.Add(key, str2);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Could not process configuration file: " + ex.Message);
                }
            }
        }
    }
}
