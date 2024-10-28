using Microsoft.EntityFrameworkCore;
using GestionONG.Models;

namespace GestionONG.Helpers
{
    public static class ProyectoHelper
    {
        public static async Task<string> GenerarCodigoProyecto(GestionOngContext context)
        {
            // Busca el último código de proyecto en la base de datos
            var ultimoProyecto = await context.Proyectos
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            // Si no hay proyectos previos, comienza con "P-0001"
            if (ultimoProyecto == null || string.IsNullOrWhiteSpace(ultimoProyecto.Codigo))
            {
                return "P-0001";
            }

            // Extrae el número del último código (ejemplo: "P-0001" -> 1)
            var numeroUltimoCodigo = int.Parse(ultimoProyecto.Codigo.Substring(2));

            // Genera el nuevo número incrementado
            var nuevoNumero = numeroUltimoCodigo + 1;

            // Formatea el nuevo código como "P-000X" donde X es el número correlativo
            return $"P-{nuevoNumero:D4}";
        }
    }
}
