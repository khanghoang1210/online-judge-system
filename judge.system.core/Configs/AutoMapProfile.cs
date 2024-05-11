
using AutoMapper;
using judge.system.core.DTOs.Requests.Account;
using judge.system.core.Models;

namespace judge.system.core.Configs
{
    public class AutoMapProfile : Profile
    {
        public AutoMapProfile()
        {
            CreateMap<Account, CreateAccountReq>().ReverseMap();

        }
    }
}
