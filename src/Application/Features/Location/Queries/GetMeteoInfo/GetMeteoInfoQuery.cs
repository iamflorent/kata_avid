using Application.Common.Exceptions;
using Application.Contracts.Services;
using MediatR;

namespace Application.Features.Location.Queries.GetMeteoInfo;

public record GetMeteoInfoQuery(string location) : IRequest<GetMeteoInfoDto>;   

public class GetGeocodingInfoQueryHandler : IRequestHandler<GetMeteoInfoQuery, GetMeteoInfoDto>
{
    public GetGeocodingInfoQueryHandler(ILocationService locationService)
    {
        LocationService = locationService;
    }
    

    ILocationService LocationService { get; }

    public async Task<GetMeteoInfoDto> Handle(GetMeteoInfoQuery request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var meteoInfo = await LocationService.GetMeteoInfoAsync(request.location);

        if (meteoInfo == null)
        {
            throw new NotFoundException($"Location {request.location}");
        }

        return meteoInfo;
    }
}
