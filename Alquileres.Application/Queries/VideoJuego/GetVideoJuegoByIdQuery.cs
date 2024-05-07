using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Queries.VideoJuego;

public record GetVideoJuegoByIdQuery(int Id) : IQuery<VideoJuegoFormDTO>;
internal class GetVideoJuegoByIdQueryHandler : IQueryHandler<GetVideoJuegoByIdQuery, VideoJuegoFormDTO>
{
    private readonly IRepositoryBase<Domain.Entities.VideoJuego> _repository;

    private readonly IMapper _mapper;

    private readonly IAzureStorageService _azureStorageService;

    public GetVideoJuegoByIdQueryHandler(IRepositoryBase<Domain.Entities.VideoJuego> repository, IMapper mapper, IAzureStorageService azureStorageService)
    {
        _repository = repository;

        _mapper = mapper;

        _azureStorageService = azureStorageService;
    }

    public async Task<VideoJuegoFormDTO> Handle(GetVideoJuegoByIdQuery req, CancellationToken cancellationToken)
    {
        var resultQuery = await _repository.GetByIdAsync(req.Id, cancellationToken);

        var result = _mapper.Map<VideoJuegoFormDTO>(resultQuery);

        if (resultQuery != null && !string.IsNullOrEmpty(resultQuery.Imagen))
        {
            var blob = await _azureStorageService.DownloadFileByteArray($"{result.Id}_{result.Imagen}", "caratulas");

            if (blob != null)
            {
                result.Fichero = Convert.ToBase64String(blob);
            }
        }

        return result;
    }
}
