using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notification_Service.Models.MongoEntities;
using Notification_Service.Models.Responses;
using Notification_Service.Models.Services;
using static Notification_Service.Services.Queries.GetDriverUsersTokensHandler;

namespace Notification_Service.Services.Queries
{
    public class GetDriverUserTokens : IRequest<List<DriverUserTokensResponse>>
    {
    }

    public class GetDriverUsersTokensHandler : IRequestHandler<GetDriverUserTokens, List<DriverUserTokensResponse>>, IGetDriverUsersTokenHandler
    {
        private readonly IMongoCollection<DriverUser> _DriverUserCollection;
        private readonly IMapper _mapper;

        public GetDriverUsersTokensHandler(
        IOptions<DriverUserDatabaseSettings> DriverStoreDatabaseSettings)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DriverUser, DriverUserTokensResponse>().ReverseMap());

            var mongoClient = new MongoClient(
                DriverStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DriverStoreDatabaseSettings.Value.DatabaseName);

            _DriverUserCollection = mongoDatabase.GetCollection<DriverUser>(
                DriverStoreDatabaseSettings.Value.CollectionName);

            _mapper = config.CreateMapper();
        }

        public async Task<List<DriverUserTokensResponse>> Handle(GetDriverUserTokens response, CancellationToken cancellationToken)
        {
            try
            {
                var filter = Builders<DriverUser>.Filter.Ne(c => c.RegistrationToken, null);
                var drivers = await _DriverUserCollection.Find(filter).ToListAsync();

                var result = _mapper.Map<List<DriverUserTokensResponse>>(drivers);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error obteniendo los usuarios driver: {ex.Message}");
            }
        }

        public interface IGetDriverUsersTokenHandler
        {
            Task<List<DriverUserTokensResponse>> Handle(GetDriverUserTokens request, CancellationToken cancellationToken);
        }
    }
}