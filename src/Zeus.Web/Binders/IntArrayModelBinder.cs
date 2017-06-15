using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace Zeus.Web.Binders
{
    public class IntArrayModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(int[]))
            {
                return false;
            }

            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var valueText = value?.RawValue as string;

            if (valueText == null)
                return false;

            var ids = new List<int>();
            foreach (var idText in valueText.Split(','))
            {
                int id;
                if (int.TryParse(idText, out id))
                {
                    ids.Add(id);
                }
                else
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Cannot convert value");
                    return false;
                }
            }

            bindingContext.Model = ids.ToArray();
            return true;
        }
    }
}