using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Notification_Service.Models.Entities.BusinessUser;

using Notification_Service.Models.Responses;
using Notification_Service.Models.Services;
using Notification_Service.Models.Settings;
using static Notification_Service.Services.Queries.GetBusinessUserTokensHandler;
using static Notification_Service.Services.Queries.GetDriverUsersTokensHandler;

namespace Notification_Service.Services.Queries
{
    public class GetBusinessUserTokens : IRequest<List<BusinessUserTokensResponse>>
    {
    }

    public class GetBusinessUserTokensHandler : IRequestHandler<GetBusinessUserTokens, List<BusinessUserTokensResponse>>, IGetBusinessUsersTokenHandler
    {
        private readonly IMongoCollection<BusinessUser> _BusinessUserCollection;
        private readonly IMapper _mapper;

        public GetBusinessUserTokensHandler(
        IOptions<BusinessUserDatabaseSettings> DriverStoreDatabaseSettings)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BusinessUser, BusinessUserTokensResponse>().ReverseMap());

            var mongoClient = new MongoClient(
                DriverStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                DriverStoreDatabaseSettings.Value.DatabaseName);

            _BusinessUserCollection = mongoDatabase.GetCollection<BusinessUser>(
                DriverStoreDatabaseSettings.Value.CollectionName);

            _mapper = config.CreateMapper();
        }

        public async Task<List<BusinessUserTokensResponse>> Handle(GetBusinessUserTokens response, CancellationToken cancellationToken)
        {
            try
            {
                var filter = Builders<BusinessUser>.Filter.Ne(c => c.RegistrationToken, null);
                var drivers = await _BusinessUserCollection.Find(filter).ToListAsync();

                var result = _mapper.Map<List<BusinessUserTokensResponse>>(drivers);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error obteniendo los usuarios driver: {ex.Message}");
            }
        }

        public interface IGetBusinessUsersTokenHandler
        {
            Task<List<BusinessUserTokensResponse>> Handle(GetBusinessUserTokens request, CancellationToken cancellationToken);
        }
    }
}