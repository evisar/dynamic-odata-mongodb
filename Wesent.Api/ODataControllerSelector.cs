using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.OData;
using System.Web.Http.OData.Builder;
using Wesent.Api.Controllers;

namespace Wesent.Api
{

    public static class ODataControllerExtension
    {
        public static void ApplyODataDynamicRoutes(this HttpConfiguration config, IDictionary<string,Type> types)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();

            config.Services.Replace(typeof(IHttpControllerSelector), new ODataControllerSelector(config, types));

            foreach (var kv in types)
            {
                var mi = typeof(ODataConventionModelBuilder).GetMethod("EntitySet").MakeGenericMethod(kv.Value); ;
                mi.Invoke(builder, new[] { kv.Key });
            }
            config.Routes.MapODataRoute("odata", "", builder.GetEdmModel());
        }
    }
    public class ODataControllerSelector : DefaultHttpControllerSelector
    {
        readonly HttpConfiguration _configuration;
        readonly IDictionary<string, Type> _types;
        
 
        public ODataControllerSelector(HttpConfiguration configuration, IDictionary<string,Type> types )
            : base(configuration)
        {
            _configuration = configuration;
            _types = types;
        }
 
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        { 
            var controllerName = base.GetControllerName(request);

            if (controllerName == "ODataMetadata")
            {
                return new HttpControllerDescriptor(_configuration, controllerName, typeof(ODataMetadataController));
            }
            else
            {                                
                var valueType = _types[controllerName];
                Type keyType = typeof(int);
                var controllerType = typeof(ODataController<,>).MakeGenericType(keyType, valueType);
                return new HttpControllerDescriptor(_configuration, controllerName, controllerType);
            }
        }
    }
}