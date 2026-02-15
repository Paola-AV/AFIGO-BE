using AfigoBackend.Domain.Gasto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IGastoInterface
    {
        Task<List<Gasto>> GetAllAsync();
    }
}
