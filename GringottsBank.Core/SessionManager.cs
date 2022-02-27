using System.Threading.Tasks;
using GringottsBank.DataContracts;
using GringottsBank.Internal.Contracts;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Core
{
    public class SessionManager : ISessionManager
    {
        public SessionManager(IDataStore<DataStore.Session> sessionDataStore, ICustomerAuthorizationService customerAuthorizationService)
        {
            _sessionDataStore = sessionDataStore;
            _customerAuthorizationService = customerAuthorizationService;
        }

        private readonly IDataStore<DataStore.Session> _sessionDataStore;
        private readonly ICustomerAuthorizationService _customerAuthorizationService;
        public async Task<bool> IsSessionActive(string token)
        {
            var sessionData = await _sessionDataStore.Read(new DataStore.Session { Token = token });
            return sessionData?.CustomerId != null;
        }

        public async Task<string> NewSession(LoginRequest loginRequest)
        {
            var session = Translator.TranslateToDataSource(loginRequest);
            var sessionInDb = await _sessionDataStore.Read(session);
            if(sessionInDb != null)
            {

            }
            var isAuthorized = _customerAuthorizationService.IsAuthorizedCustomer(loginRequest.CustomerId, loginRequest.Password);
            sessionInDb = await _sessionDataStore.Create(session);
            return sessionInDb.Token;
        }

        public async Task<string> GetCustomerSessionData(string token)
        {
            bool isSessionActive = await IsSessionActive(token);
            if (isSessionActive == false)
            {
                return null;
            }

            var sessionData = await _sessionDataStore.Read(new DataStore.Session { Token = token });

            return sessionData.CustomerId;
        }
    }
}
