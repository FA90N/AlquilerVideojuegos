using Alquileres.Application.Configuration.CQRS;
using Alquileres.Application.Interfaces.Application;
using Alquileres.Application.Interfaces.Infrastructure.Repositories;
using Alquileres.Application.Models.Commands;
using AutoMapper;

namespace Alquileres.Application.Queries.Cliente;

public record GetClienteByIdQuery(int Id) : IQuery<ClienteFormDTO>;

public class GetVideoClienteByIdHandler : IQueryHandler<GetClienteByIdQuery, ClienteFormDTO>
{
    private readonly IRepositoryBase<Domain.Entities.Cliente> _repository;

    private readonly IMapper _mapper;

    private readonly IAzureStorageService _azureStorageService;
    public GetVideoClienteByIdHandler(IRepositoryBase<Domain.Entities.Cliente> repository, IMapper mapper, IAzureStorageService azureStorageService)
    {
        _repository = repository;

        _mapper = mapper;

        _azureStorageService = azureStorageService;
    }

    public async Task<ClienteFormDTO> Handle(GetClienteByIdQuery request, CancellationToken cancellationToken)
    {
        var resultQuery = await _repository.GetByIdAsync(request.Id, cancellationToken);

        var result = _mapper.Map<ClienteFormDTO>(resultQuery);

        if (resultQuery != null && !string.IsNullOrEmpty(resultQuery.Documento))
        {
            var blob = await _azureStorageService.DownloadFileByteArray($"{result.Id}_{result.Documento}", "usuarios");

            if (blob != null)
            {
                result.Fichero = Convert.ToBase64String(blob);
            }
        }

        return result;
    }
}
