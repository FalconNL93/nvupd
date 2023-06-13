using System.Threading.Tasks;
using AutoMapper;
using Nvupd.Core.Helpers;
using Nvupd.Core.Models;
using Nvupd.Core.Services;
using NvUpd.UI.Models;

namespace NvUpd.UI.Services;

public class GpuService
{
    private readonly IMapper _mapper;

    public GpuService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public static GpuInformation GetGpuInformation()
    {
        return GpuHelper.GetGpuInformation();
    }

    public async Task<NvidiaUpdate> GetLatestUpdates()
    {
        return _mapper.Map<NvidiaUpdate>(await NvidiaUpdateService.GetUpdateData());
    }
}