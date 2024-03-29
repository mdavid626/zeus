﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Exporter
{
    public class ExporterContextFromArgs : IExporterContext
    {
        public string[] Arguments { get; }

        private DateTime? upperBound;

        public DateTime UpperBound
        {
            get
            {
                if (upperBound == null)
                {
                    upperBound = ParseUpperBound();
                }
                return upperBound.Value;
            }
        }

        public ExporterContextFromArgs(string[] args)
        {
            Arguments = args;
        }

        private DateTime ParseUpperBound()
        {
            var dateText = Arguments?.FirstOrDefault();

            if (String.IsNullOrEmpty(dateText))
            {
                return DateTime.Now.Date;
            }

            if (DateTime.TryParseExact(dateText, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }

            throw new FormatException($"The input string ({dateText}) is in wrong format. The supported format is: yyyy-MM-dd");
        }
    }
}
