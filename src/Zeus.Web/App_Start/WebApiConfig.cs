using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using Zeus.Common;
using Zeus.Web.Binders;

namespace Zeus.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            AddModelBinderProvider<int[]>(config, new IntArrayModelBinder());
            AddModelBinderProvider<TrackedEventType>(config, new EnumModelBinder<TrackedEventType>());
        }

        private static void AddModelBinderProvider<T>(HttpConfiguration config, IModelBinder binder)
        {
            var provider = new SimpleModelBinderProvider(typeof(T), binder);
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);
        }
    }
}
