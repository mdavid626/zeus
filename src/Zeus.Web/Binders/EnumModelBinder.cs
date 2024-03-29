﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Zeus.Web.Binders
{
    public class EnumModelBinder<T> : IModelBinder where T : struct 
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(T) &&
                bindingContext.ModelType != typeof(T?))
            {
                return false;
            }

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var valueText = value?.RawValue as string;

            if (String.IsNullOrEmpty(valueText))
                return false;

            T eventType;
            if (Enum.TryParse(valueText, true, out eventType))
            {
                bindingContext.Model = eventType;
                return true;
            }

            bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Cannot convert value");
            return false;
        }
    }
}