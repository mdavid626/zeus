using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Trackers
{
    public class SqlTracker : ITracker
    {
        public ITrackedEventContextProvider DbContextProvider { get; }

        public SqlTracker(ITrackedEventContextProvider dbContextProvider)
        {
            DbContextProvider = dbContextProvider;
        }

        public void Track(TrackedEvent trackedEvent)
        {
            using (var ctx = DbContextProvider.Provide())
            {
                foreach (var id in trackedEvent.Ids)
                {
                    var dto = new TrackedEventDto();
                    dto.ProductId = id;
                    dto.EventType = (byte) trackedEvent.Type;
                    dto.EventDate = trackedEvent.CreationDate;

                    ctx.TrackedEvents.Add(dto);
                }

                ctx.SaveChanges();
            }
        }

        public IEnumerable<ProductStatistics> GetStatistics(DateTime upperBound)
        {
            var upperDate = upperBound.Date;
            var lowerBound1Day = upperDate.AddDays(-1);
            var lowerBound7Days = upperDate.AddDays(-7);
            var lowerBound14Days = upperDate.AddDays(-14);

            using (var ctx = DbContextProvider.Provide())
            {
                var stats = ctx
                 .TrackedEvents
                 .Where(e => e.EventDate >= lowerBound14Days && e.EventDate < upperDate)
                 .GroupBy(e => e.ProductId)
                 .Select(e => e.Key)
                 .Select(pId => 
                    new
                    {
                        ProductId = pId,
                        ListImpressions = ctx.TrackedEvents
                           .Count(e => e.ProductId == pId && e.EventType == (byte) TrackedEventType.List &&
                                       e.EventDate >= lowerBound1Day && e.EventDate < upperDate),
                        DetailsViews = ctx.TrackedEvents
                            .Count(e => e.ProductId == pId && e.EventType == (byte)TrackedEventType.Details &&
                                        e.EventDate >= lowerBound1Day && e.EventDate < upperDate),
                        Conversions = ctx.TrackedEvents
                            .Count(e => e.ProductId == pId && e.EventType == (byte)TrackedEventType.Conversion &&
                                        e.EventDate >= lowerBound1Day && e.EventDate < upperDate),
                        ListViews7Days = ctx.TrackedEvents
                            .Count(e => e.ProductId == pId && e.EventType == (byte)TrackedEventType.List &&
                                        e.EventDate >= lowerBound7Days && e.EventDate < upperDate),
                        DetailViews7Days = ctx.TrackedEvents
                            .Count(e => e.ProductId == pId && e.EventType == (byte)TrackedEventType.Details &&
                                        e.EventDate >= lowerBound7Days && e.EventDate < upperDate),
                        Conversions7Days = ctx.TrackedEvents
                            .Count(e => e.ProductId == pId && e.EventType == (byte)TrackedEventType.Conversion &&
                                        e.EventDate >= lowerBound7Days && e.EventDate < upperDate),
                        DetailViews14Days = ctx.TrackedEvents
                            .Count(e => e.ProductId == pId && e.EventType == (byte)TrackedEventType.Details &&
                                        e.EventDate >= lowerBound14Days && e.EventDate < upperDate),
                        Conversions14Days = ctx.TrackedEvents
                            .Count(e => e.ProductId == pId && e.EventType == (byte)TrackedEventType.Conversion &&
                                        e.EventDate >= lowerBound14Days && e.EventDate < upperDate),
                    }).ToList();

                return stats.Select(s =>
                {
                    var clickRate7Days = s.ListViews7Days != 0
                        ? Math.Min(Math.Round(100.0 * s.DetailViews7Days / s.ListViews7Days, 2), 100)
                        : 0;
                    var conversionRate7Days = s.DetailViews7Days != 0
                        ? Math.Min(Math.Round(100.0 * s.Conversions7Days / s.DetailViews7Days, 2), 100)
                        : 0;
                    var conversionRate14Days = s.DetailViews14Days != 0
                        ? Math.Min(Math.Round(100.0 * s.Conversions14Days / s.DetailViews14Days, 2), 100)
                        : 0;

                    return new ProductStatistics()
                    {
                        Id = s.ProductId,
                        ListImpressions = s.ListImpressions,
                        DetailsViews = s.DetailsViews,
                        Conversions = s.Conversions,
                        ClickRate7Days = clickRate7Days,
                        ConversionRate7Days = conversionRate7Days,
                        ConversionRate14Days = conversionRate14Days
                    };
                }).ToList();
            }
        }
    }
}
