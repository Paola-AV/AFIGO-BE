using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IExcelExporter
    {
        byte[] Create<T>(IEnumerable<T> data, string sheetName = "Reporte");
    }
}
