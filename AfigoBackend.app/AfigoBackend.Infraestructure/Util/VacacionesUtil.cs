using AfigoBackend.Domain.PeticionVacaciones;
using AfigoBackend.Domain.Trabajador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Infraestructure.Util
{
    public static class VacacionesUtil
    {

        public static decimal CalcularDiasVacacionesDisponibles(Trabajador emp,List<PeticionVacaciones> vacaciones)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var inicio = emp.FechaInicio;

            if (inicio > hoy)
                return 0;


            int totalMeses = (hoy.Year - inicio.Year) * 12 + (hoy.Month - inicio.Month);

            if (hoy.Day < inicio.Day)
                totalMeses--;

            if (totalMeses < 0)
                totalMeses = 0;

            var diasTomados = new Dictionary<DateOnly, decimal>();

            foreach (var v in vacaciones.Where(v => v.Estado == "APROBADO"))
            {
                if (v.MedioDia)
                {
                    diasTomados[v.FechaInicio] = 0.5m;
                    continue;
                }

                var d = v.FechaInicio;
                while (d <= v.FechaFin)
                {
                    diasTomados[d] = 1m;   
                    d = d.AddDays(1);
                }
            }

            decimal totalTomados = diasTomados.Values.Sum();

            decimal disponibles = totalMeses - totalTomados;
            return disponibles;
        }

        private static int DiasInclusivos(DateOnly inicio, DateOnly fin)
        {
            if (fin < inicio) return 0;

            var d1 = inicio.ToDateTime(TimeOnly.MinValue);
            var d2 = fin.ToDateTime(TimeOnly.MinValue);

            return (d2 - d1).Days + 1;
        }
    }




}
