using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Uno.Entities;
using Uno.Models.Identity;

namespace Uno
{
    public class AutoMapping :Profile
    {
        //https://www.codementor.io/@zedotech/how-to-using-automapper-on-asp-net-core-3-0-via-dependencyinjection-zq497lzsq
        public AutoMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ForMember(d => d.Cognome, opt => opt.MapFrom(src => src.Cognome))
             .ForMember(d => d.Nome, opt => opt.MapFrom(src => src.Nome));
            CreateMap<ApplicationUser, ApplicationUserModDTO>()
               .ForMember(d => d.Cognome, opt => opt.MapFrom(src => src.Cognome))
            .ForMember(d => d.Nome, opt => opt.MapFrom(src => src.Nome));
            CreateMap<IdentityRole, ApplicationRoleDTO>();
              
            //CreateMap<List<ApplicationUser>, List<ApplicationUserDTO>>();

            //var config = new MapperConfiguration(cfg => {
            //    cfg.CreateMap<List<ApplicationUser>, List<ApplicationUserDTO>>();

            //    IMapper mapper = config.CreateMapper();

            //LstAuditDto = mapper.Map<List<Audit>, List<AuditDTO>>(Audits);
        }
    }
}
