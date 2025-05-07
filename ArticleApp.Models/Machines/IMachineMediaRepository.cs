using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleApp.Models.Machines
{
    /// <summary>
    /// [3] Repository Interface
    /// </summary>
    public interface IMachineMediaRepository
    {
        Task AddMachineMediaAsync(int machineId, int mediaId);
        Task<Media> GetMediasByMachineIdAsync(int machineId);
        Task<Machine> GetMachinesByMediaIdAsync(int mediaId);
        Task DeleteMediaAsync(int machineId, int mediaId);
    }
}
