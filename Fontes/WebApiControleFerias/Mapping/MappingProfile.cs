using AutoMapper;
using ControleFerias.API.DTO;
using ControleFerias.DTO;
using ControleFerias.Models;

namespace ControleFerias.AutoMapper
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // colaborador -> colaboradorconsultadto
            CreateMap<Colaborador, ColaboradorConsultarDTO>();
            CreateMap<Colaborador, ColaboradorIncluirDTO>();
            CreateMap<Colaborador, ColaboradorAlterarDTO>();
            CreateMap<Colaborador, ColaboradorConsultarFeriasDTO>();

            // dto -> entidade (incluir colaborador)
            CreateMap<ColaboradorIncluirDTO, Colaborador>();

            // ferias -> feriasdto (com datas formatadas)
            CreateMap<Ferias, FeriasFormatadaDTO>()
                .ForMember(dest => dest.dDataInicio,
                    opt => opt.MapFrom(src => src.dDataInicio.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.dDataFinal,
                    opt => opt.MapFrom(src => src.dDataFinal.ToString("dd/MM/yyyy")));

            CreateMap<Ferias, FeriasCriarDTO>()
                .ForMember(dest => dest.dDataInicio,
                    opt => opt.MapFrom(src => src.dDataInicio.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.dDataFinal,
                    opt => opt.MapFrom(src => src.dDataFinal.ToString("dd/MM/yyyy")));

            //equipe -> equipedto
            CreateMap<Equipe, EquipeIncluirDTO>().ReverseMap();
            CreateMap<Equipe, EquipeAlterarDTO>().ReverseMap();
        }
    }
}