using Microsoft.Isam.Esent.Collections.Generic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Wesent.Common;

namespace Wesent.Api.Controllers
{
    public class ODataController<TKey, TEntity> : ODataController
        where TKey: IComparable<TKey>
        where TEntity: IEntity
    {

        readonly IRepository<TKey, TEntity> _repository;
        public ODataController()
        {
            Type type = Type.GetType(ConfigurationManager.AppSettings["provider"]);
            type = type.MakeGenericType(typeof(TKey), typeof(TEntity));
            string url = ConfigurationManager.AppSettings["url"];
            _repository = Activator.CreateInstance(type, url) as IRepository<TKey, TEntity>;
        }
        public IQueryable<TEntity> Get(ODataQueryOptions<TEntity> options)
        {
            return options.ApplyTo(_repository.Query()).OfType<TEntity>();
        }
        public TEntity Get([FromODataUri]TKey key)
        {
            return _repository.Get(key);
        }

        public IHttpActionResult Post(TEntity obj)
        {            
            _repository.Update(obj);
            return Created(obj);
        }

        public IHttpActionResult Delete([FromODataUri]TKey key)
        {            
            _repository.Delete(key);
            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
