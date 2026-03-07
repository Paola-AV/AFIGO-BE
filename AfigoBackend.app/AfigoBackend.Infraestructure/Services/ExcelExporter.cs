using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfigoBackend.Aplication.Abstractions.Interfaces;
using ClosedXML.Excel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;


namespace AfigoBackend.Infraestructure.Services
{
    public class ExcelExporter: IExcelExporter
    {

        public byte[] Create<T>(IEnumerable<T> data, string sheetName = "Reporte")
        {
            using var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(string.IsNullOrWhiteSpace(sheetName) ? "Reporte" : sheetName);

            var type = typeof(T);
            var props = GetExportableProps(type).ToArray();

            // ===== Encabezados =====
            for (int i = 0; i < props.Length; i++)
            {
                var header = GetHeaderText(props[i]);
                var cell = ws.Cell(1, i + 1);
                cell.Value = header;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
            }

            // ===== Cuerpo =====

            int rowIdx = 2;
            foreach (var item in data ?? Enumerable.Empty<T>())
            {
                for (int col = 0; col < props.Length; col++)
                {
                    var prop = props[col];
                    var value = prop.GetValue(item);
                    var cell = ws.Cell(rowIdx, col + 1);

                    SetCellValue(cell, value);
                }
                rowIdx++;
            }


            // ===== Formatos por atributo =====
            for (int col = 0; col < props.Length; col++)
            {
                var fmt = props[col].GetCustomAttribute<DisplayFormatAttribute>();
                if (!string.IsNullOrWhiteSpace(fmt?.DataFormatString))
                {
                    // Si viene con {0:...}, quítalo
                    var format = fmt.DataFormatString!.Replace("{0:", "").Replace("}", "");
                    ws.Column(col + 1).Style.NumberFormat.Format = format;
                }
            }

            // ===== Estética =====
            ws.SheetView.FreezeRows(1);         // Fija cabecera
            ws.Columns().AdjustToContents();     // Ajusta anchos

            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        // Obtiene propiedades exportables ordenadas:
        // - Públicas, legibles, no indizadoras
        // - Excluye [NotMapped], [ScaffoldColumn(false)], [Display(AutoGenerateField=false)]
        // - Ordena por [Display(Order=...)] si existe
        private static IEnumerable<PropertyInfo> GetExportableProps(Type type)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanRead && p.GetIndexParameters().Length == 0);

            props = props.Where(p =>
            {
                if (p.GetCustomAttribute<NotMappedAttribute>() != null)
                    return false;

                var scaffold = p.GetCustomAttribute<ScaffoldColumnAttribute>();
                if (scaffold != null && scaffold.Scaffold == false)
                    return false;

                var display = p.GetCustomAttribute<DisplayAttribute>();
                if (display != null && display.GetAutoGenerateField() == false)
                    return false;

                return true;
            });

            // Orden por [Display(Order=...)] si está definido
            return props.OrderBy(p =>
            {
                var display = p.GetCustomAttribute<DisplayAttribute>();
                // Si no hay orden, usa int.MaxValue para mantener el original al final
                return display?.GetOrder() ?? int.MaxValue;
            });
        }

        // Encabezado = Display(Name) | DisplayName | Nombre de la propiedad
        private static string GetHeaderText(PropertyInfo prop)
        {
            var display = prop.GetCustomAttribute<DisplayAttribute>();
            if (!string.IsNullOrWhiteSpace(display?.GetName()))
                return display!.GetName()!;

            var displayName = prop.GetCustomAttribute<DisplayNameAttribute>();
            if (!string.IsNullOrWhiteSpace(displayName?.DisplayName))
                return displayName!.DisplayName;

            return prop.Name;
        }


        private static void SetCellValue(IXLCell cell, object? value)
        {
            if (value is null)
            {
                // Limpia o deja vacío
                cell.Clear();
                return;
            }

            // Normaliza tipos que Excel no reconoce nativamente
            if (value is DateOnly d)
                value = new DateTime(d.Year, d.Month, d.Day);

            else if (value is TimeOnly t)
                value = new TimeSpan(t.Hour, t.Minute, t.Second);

            // Si quieres que los enums salgan como texto y no como número:
            else if (value is Enum e)
                value = e.ToString();

            // Crea el XLCellValue a partir del objeto ya normalizado
            var xlValue = XLCellValue.FromObject(value);
            cell.SetValue(xlValue);
        }


    }
}

//Si tu DTO o clase trae [Display(Name = "ID Cuenta")], el encabezado será “ID Cuenta”.
//Si agregas [Display(Order = 1)], se respetará ese orden.
//Si agregas [DisplayFormat(DataFormatString = "#,##0.00")], se aplicará ese formato a la columna completa.
//Puedes ocultar columnas con [Display(AutoGenerateField = false)], [ScaffoldColumn(false)] o[NotMapped].