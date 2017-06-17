using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeus.Exporter
{
    public interface IStreamProvider
    {
        TextWriter Create(DateTime date);
    }
}
