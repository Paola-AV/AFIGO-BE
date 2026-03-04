using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfigoBackend.Aplication.Abstractions.Interfaces
{
    public interface IExternalSyncInterface
    {
        Task SyncAllAsync(CancellationToken ct = default);
        Task SyncCuentas(CancellationToken ct = default);
        Task SyncFacturas(CancellationToken ct = default);
        Task SyncGasto(CancellationToken ct = default);
        Task SyncInventario(CancellationToken ct = default);
        Task SyncProductos(CancellationToken ct = default);
        Task SyncProveedores(CancellationToken ct = default);
        Task SyncVentas(CancellationToken ct = default);
    }
}
