using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Exporter
{
    public class FileStreamProvider : IStreamProvider
    {
        public TextWriter Create(DateTime upperBound)
        {
            return File.CreateText($"events_{upperBound:yyyy-MM-dd}.csv");
        }
    }
}
