using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.OData.Builder;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using Wesent.Common;

namespace Wesent.Api
{
    public class Global : HttpApplication
    {

        public void Application_Start()
        {
            GlobalConfiguration.Configure(config =>
            {

                bool authenticate;
                bool.TryParse(ConfigurationManager.AppSettings["authenticate"], out authenticate);
                if (authenticate)
                {
                    config.ApplyBasicAuthentication();
                }

                var types = LoadControllerTypes();

                config.ApplyODataDynamicRoutes(types);

            });
        }

        private static Dictionary<string, Type> LoadControllerTypes()
        {
            string model = ConfigurationManager.AppSettings["model"];
            var asm = (from a in AppDomain.CurrentDomain.GetAssemblies()
                       where a.FullName.StartsWith(model)
                       select a).FirstOrDefault();
            var types = (from t in asm.GetTypes()
                         where typeof(IEntity).IsAssignableFrom(t)
                         select t).ToDictionary(x => x.Name);
            return types;
        }
    }
}