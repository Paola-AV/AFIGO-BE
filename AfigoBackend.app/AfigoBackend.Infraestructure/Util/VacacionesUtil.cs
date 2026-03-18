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


        public static int CalcularDiasVacacionesDisponibles(Trabajador emp, List<PeticionVacaciones> vacaciones)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var inicio = emp.FechaInicio; 

            // 0) Si la fecha de inicio es futura, no hay meses ni días acumulados
            if (inicio > hoy)
                return 0;

            // 1) Calcular meses laborados (meses completos)
            int years = hoy.Year - inicio.Year;
            int months = hoy.Month - inicio.Month;
            int totalMeses = years * 12 + months;

            // Ajuste si aún no llegó el "día" de aniversario del mes actual
            // (ej.: inicio 31/01 y hoy 10/02 => aún no cumple el mes)
            if (hoy.Day < inicio.Day)
                totalMeses--;

            if (totalMeses < 0)
                totalMeses = 0;

            // 2) Calcular días tomados sólo de peticiones aprobadas
            // Si Vacacion.FechaInicio/FechaFin son DateOnly, conviértelas o usa un helper
            int diasTomados = vacaciones
                .Where(v => v.Estado == "APROBADA")
                .Sum(v => DiasInclusivos(v.FechaInicio, v.FechaFin));

            // 3) Días disponibles
            int disponibles = totalMeses - diasTomados;
            return disponibles;
        }

        // Cuenta días de forma inclusiva entre dos DateOnly (p. ej. 10–12 => 3 días)
        private static int DiasInclusivos(DateOnly inicio, DateOnly fin)
        {
            if (fin < inicio) return 0;
            // Convertimos a DateTime en medianoche para poder restar
            var d1 = inicio.ToDateTime(TimeOnly.MinValue);
            var d2 = fin.ToDateTime(TimeOnly.MinValue);
            return (d2 - d1).Days + 1;
        }


    }
}
