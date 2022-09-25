
using Application.Features.Location.Queries.GetMeteoInfo;

namespace Application.Contracts.Services
{
    public interface ILocationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns>null if not found</returns>
        Task<GetMeteoInfoDto?> GetMeteoInfoAsync(string location);
    }
}
