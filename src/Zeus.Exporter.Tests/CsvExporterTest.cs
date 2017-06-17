using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Zeus.Trackers;

namespace Zeus.Exporter.Tests
{
    [TestClass]
    public class CsvExporterTest
    {
        [TestMethod]
        public void TestExport()
        {
            // arrange
            var date = DateTime.Now;
            var tracker = Substitute.For<ITracker>();

            tracker.GetStatistics(date).Returns(t => new []
            {
                new ProductStatistics()
                {
                    Id = 1,
                    ListImpressions = 10,
                    DetailsViews = 20,
                    Conversions = 30,
                    ClickRate7Days = 12.5f,
                    ConversionRate7Days = 45f,
                    ConversionRate14Days = 67f
                },
                new ProductStatistics()
                {
                    Id = 2,
                    ListImpressions = 100,
                    DetailsViews = 200,
                    Conversions = 300,
                    ClickRate7Days = 14.5f,
                    ConversionRate7Days = 75f,
                    ConversionRate14Days = 87f
                },
            });

            var streamProvider = Substitute.For<IStreamProvider>();
            var sb = new StringBuilder();
            streamProvider.Create(date).Returns(t => new StringWriter(sb));

            var context = Substitute.For<IExporterContext>();
            context.UpperBound.Returns(t => date);

            var exporter = new CsvExporter(tracker, streamProvider);

            // act
            exporter.Export(context);

            // assert
            var text = sb.ToString();
            var lines = text.Split(new [] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var header = lines.FirstOrDefault();
            var product1 = lines.ElementAtOrDefault(1);
            var product2 = lines.ElementAtOrDefault(2);

            Assert.AreEqual(header, "Id,ListImpressions,DetailsViews,Conversions,ClickRate7Days,ConversionRate7Days,ConversionRate14Days");
            Assert.AreEqual(product1, "1,10,20,30,12.5,45,67");
            Assert.AreEqual(product2, "2,100,200,300,14.5,75,87");
        }
    }
}
