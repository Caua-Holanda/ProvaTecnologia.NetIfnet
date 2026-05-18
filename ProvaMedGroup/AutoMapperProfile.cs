using AutoMapper;
using ProvaMedGroup.DomainModel.Entities;
using ProvaMedGroup.DomainModel.ValueObjects;
using ProvaMedGroup.ViewModels;

namespace ProvaMedGroup
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Contato, ContatoViewModel>()
                .ForMember(dest => dest.PrimeiroNome, opt => opt.MapFrom(src => src.NomeCompleto.PrimeiroNome))
                .ForMember(dest => dest.Sobrenome, opt => opt.MapFrom(src => src.NomeCompleto.Sobrenome));

            CreateMap<ContatoViewModel, Contato>()
                .ConstructUsing((src, context) =>
                {
                    return Contato.Criar(src.PrimeiroNome, src.Sobrenome, src.DataNascimento, src.Sexo);
                });
        }
    }
}
