using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Metadata.Providers;
using System.Web.Http.ModelBinding;
using System.Web.Http.ValueProviders;
using NSubstitute;

namespace Zeus.Web.Tests
{
    public class ModelBinderTestBase
    {
        protected ModelBindingContext CreateBindingContext<T>(string value, string modelName = "name")
        {
            var data = new DataAnnotationsModelMetadataProvider();
            var modelMetadata = data.GetMetadataForType(null, typeof(T));
            var valueProvider = Substitute.For<IValueProvider>();

            if (value != null)
            {
                valueProvider
                    .GetValue(Arg.Any<string>())
                    .Returns(new ValueProviderResult(value, "", CultureInfo.InvariantCulture));
            }
            else
            {
                valueProvider.GetValue(Arg.Any<string>()).Returns(default(ValueProviderResult));
            }

            return new ModelBindingContext
            {
                ModelName = modelName,
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };
        }
    }
}
