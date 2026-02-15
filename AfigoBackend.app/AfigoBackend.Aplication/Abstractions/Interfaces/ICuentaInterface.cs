using AfigoBackend.Domain.Cuenta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface ICuentaInterface
    {
        Task<List<Cuenta>> GetAllAsync();
    }
}
