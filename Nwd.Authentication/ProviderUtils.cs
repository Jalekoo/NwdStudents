using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nwd.Authentication
{
    internal class ProviderUtils
    {
        public static object GetConfigValue( NameValueCollection config, string configKey, object defaultValue )
        {
            object configValue;

            try
            {
                configValue = defaultValue;
                configValue = config[configKey];
                if( configValue != null )
                {
                    configValue = string.IsNullOrEmpty( configValue.ToString() ) ? defaultValue : configValue;
                }
            }
            catch
            {
                configValue = defaultValue;
            }

            return configValue;
        }
    }
}
