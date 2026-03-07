using AfigoBackend.Aplication.DTO;
using AfigoBackend.Domain.Inventario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IInventarioInterface
    {
        Task<List<Inventario>> GetAllAsync();
        Task<List<InventarioDTO>> GetInventariosParaExcel();
    }
}
