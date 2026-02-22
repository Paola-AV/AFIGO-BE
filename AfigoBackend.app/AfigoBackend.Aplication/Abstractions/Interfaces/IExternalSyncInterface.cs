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
    }
}
