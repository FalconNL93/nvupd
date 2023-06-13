using AutoMapper;
using Nvupd.Core.Models;
using NvUpd.UI.Models;

namespace NvUpd.UI.Configurations;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<UpdateData, NvidiaUpdate>();
    }
}