using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication3.Domain;
using WebApplication3.Domain.Security;
using WebApplication3.Facades.ValueResolvers;
using WebApplication3.Models;
using WebApplication3.Models.Fatturazione;

namespace WebApplication3.Facades
{
    public class AutoMapperConfigurationProfile : Profile
    {
        public AutoMapperConfigurationProfile()
        {

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Mail, opt => opt.MapFrom(source => source.UserName))
                .ForMember(dest => dest.Active, opt => opt.MapFrom(source => source.IsActive))
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest=> dest.Role, opt => opt.ResolveUsing<RoleResolver>());


            CreateMap<ClienteDto, Cliente>()
                .ForMember(dest => dest.RagioneSociale, opt => opt.MapFrom(source => source.Anag.Denom))
                .ForMember(dest => dest.PartitaIva, opt => opt.MapFrom(source => source.Anag.Piva))
                .ForMember(dest => dest.CodiceFiscale, opt => opt.MapFrom(source => source.Anag.Cf))
                .ForMember(dest => dest.Cee, opt => opt.MapFrom(source => "I"))
                .ForMember(dest => dest.Nazione, opt => opt.MapFrom(source => source.Anag.DomFisc.Naz))
                .ForMember(dest => dest.Provincia, opt => opt.MapFrom(source => source.Anag.DomFisc.Prov))
                .ForMember(dest => dest.Comune, opt => opt.MapFrom(source => source.Anag.DomFisc.Com))

                .ForMember(dest => dest.Indirizzo, opt => opt.MapFrom(source => source.Anag.DomFisc.Ind))
                .ForMember(dest => dest.Cap, opt => opt.MapFrom(source => source.Anag.DomFisc.Cap))
                .ForMember(dest => dest.Pec, opt => opt.MapFrom(source => source.Sdi.Pec))
                .ForMember(dest => dest.CodiceSdi, opt => opt.MapFrom(source => source.Sdi.Cod))
                .ForMember(dest => dest.CodiceCliente, opt => opt.Ignore());




        }
    }
}
