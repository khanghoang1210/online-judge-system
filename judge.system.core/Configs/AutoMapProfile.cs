
using AutoMapper;
using judge.system.core.DTOs.Requests.Account;
using judge.system.core.DTOs.Responses.Post;
using judge.system.core.DTOs.Responses.Problem;
using judge.system.core.Models;

namespace judge.system.core.Configs
{
    public class AutoMapProfile : Profile
    {
        public AutoMapProfile()
        {
            CreateMap<Account, CreateAccountReq>().ReverseMap();
            CreateMap<Problem, GetProblemRes>().ReverseMap();
            CreateMap<Post, GetPostRes>().ReverseMap();
        }
    }
}
