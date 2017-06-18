using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.ModelBinding.Binders;
using Microsoft.Practices.Unity;
using Zeus.Trackers;
using Zeus.Web.Binders;

namespace Zeus.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.AddUnity();

            config.MapHttpAttributeRoutes();

            config.AddModelBinderProvider<int[]>(new IntArrayModelBinder());
            config.AddModelBinderProvider<TrackedEventType>(new EnumModelBinder<TrackedEventType>());
        }

        private static void AddUnity(this HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<ITrackedEventContextProvider, TrackedEventContextProvider>(new HierarchicalLifetimeManager());
            container.RegisterType<ITracker, SqlTracker>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);
        }

        private static void AddModelBinderProvider<T>(this HttpConfiguration config, IModelBinder binder)
        {
            var provider = new SimpleModelBinderProvider(typeof(T), binder);
            config.Services.Insert(typeof(ModelBinderProvider), 0, provider);
        }
    }
}
