using BlazorAppAuth.Utils;
using BlazorGoogle.Development.Core.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorAppAuth.Web.Components.Base
{
    [Authorize]
    public class SecuredPage : ComponentBase
    {
        [Inject]
        public AuthenticatedUserInfo UserInfo { get; set; }

        public Account GetAccount()
        {
            return UserInfo.GetAccount();
        }
    }
}
