using Alquileres.Application.Commands.Cliente;
using Alquileres.Application.Commands.FormaPago;
using Alquileres.Application.Commands.Genero;
using Alquileres.Application.Commands.LineaAlquiler;
using Alquileres.Application.Commands.LineaGenero;
using Alquileres.Application.Commands.Plataforma;
using Alquileres.Application.Commands.PrecioVideoJuego;
using Alquileres.Application.Commands.VideoJuego;
using Alquileres.Application.Models;
using Alquileres.Application.Models.Commands;
using Alquileres.Application.Models.Queries;
using Alquileres.Domain.Entities;
using AutoMapper;

namespace Alquileres.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AspNetLanguages, AspNetLanguagesDto>().ReverseMap();

            CreateMap<Sequences, SequencesDTO>().ReverseMap();

            CreateMap<CreateClienteCommand, Cliente>().ReverseMap();
            CreateMap<UpdateClienteCommand, Cliente>().ReverseMap();
            CreateMap<ClienteFormDTO, Cliente>().ReverseMap();


            CreateMap<Genero, GeneroListDTO>().ReverseMap();
            CreateMap<Genero, GeneroFormDTO>().ReverseMap();
            CreateMap<UpdateGeneroCommand, Genero>().ReverseMap();
            CreateMap<CreateGeneroCommand, Genero>().ReverseMap();

            CreateMap<Plataforma, PlataformaListDTO>().ReverseMap();
            CreateMap<Plataforma, PlataformaFormDTO>().ReverseMap();
            CreateMap<UpdatePlataformaCommand, Plataforma>().ReverseMap();
            CreateMap<CreatePlataformaCommand, Plataforma>().ReverseMap();

            CreateMap<VideoJuego, VideoJuegoListDTO>().ReverseMap();
            CreateMap<VideoJuego, VideoJuegoFormDTO>().ReverseMap();
            CreateMap<UpdateVideoJuegoCommand, VideoJuego>().ReverseMap();
            CreateMap<CreateVideoJuegoCommand, VideoJuego>().ReverseMap();

            CreateMap<LineasGenero, LineaGeneroDTO>().ReverseMap().ReverseMap();
            CreateMap<CreateLineaGeneroCommand, LineasGenero>().ReverseMap();
            CreateMap<UpdateLineaGeneroCommand, LineasGenero>().ReverseMap();

            CreateMap<PrecioVideoJuego, PrecioVideoJuegoFormDTO>().ReverseMap();
            CreateMap<UpdatePrecioVideoJuegoCommand, PrecioVideoJuego>().ReverseMap();
            CreateMap<CreatePrecioVideoJuegoCommand, PrecioVideoJuego>().ReverseMap();

            CreateMap<FormaPago, FormaPagoListDTO>().ReverseMap();
            CreateMap<FormaPagoFormDTO, FormaPago>().ReverseMap();
            CreateMap<CreateFormaPagoCommand, FormaPago>().ReverseMap();
            CreateMap<UpdateFormaPagoCommand, FormaPago>().ReverseMap();


            CreateMap<AlquilerFormDTO, Alquiler>().ReverseMap();
            CreateMap<AlquilerListDTO, Alquiler>().ReverseMap();
            CreateMap<CreateClienteCommand, Alquiler>().ReverseMap();
            CreateMap<UpdateClienteCommand, Alquiler>().ReverseMap();

            CreateMap<LineaAlquilerFormDTO, LineasAlquiler>().ReverseMap();
            CreateMap<CreateLineaAlquilerCommand, LineasAlquiler>().ReverseMap();
            CreateMap<UpdateLineaAlquilerCommand, LineasAlquiler>().ReverseMap();

        }
    }
}