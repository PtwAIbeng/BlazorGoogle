using BlazorGoogle.Development.Core.Account;
using Newtonsoft.Json;

namespace BlazorAppAuth.Utils
{
    public class AuthenticatedUserInfo(IHttpContextAccessor httpContextAccessor, SessionService sessionService)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly SessionService _sessionService = sessionService;

        public string Name
        {
            get
            {
                return _httpContextAccessor?.HttpContext?.User?.Identity?.Name;
            }
        }

        public Account GetAccount()
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return null;
            }

            var userSession = _sessionService.GetSessionValue("User");

            if (string.IsNullOrEmpty(userSession))
            {
                var acc = Account.FindByEmail(Name);
                if (acc == null)
                {
                    return null;
                }

                acc.Permissions = AccountPermission.ReadAllPermissionsByAccountId(acc.ID);
                _sessionService.SetSessionValue("User", JsonConvert.SerializeObject(acc, Formatting.Indented));
            }

            return JsonConvert.DeserializeObject<Account>(_sessionService.GetSessionValue("User"));
        }
    }
}
