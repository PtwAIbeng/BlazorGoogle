namespace BlazorAppAuth.Utils
{
    public class SessionService(IHttpContextAccessor httpContextAccessor)
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string GetSessionValue(string key)
        {
            return _httpContextAccessor.HttpContext.Session.GetString(key);
        }

        public void SetSessionValue(string key, string value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, value);
        }
    }
}
