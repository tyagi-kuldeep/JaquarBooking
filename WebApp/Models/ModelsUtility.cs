using System.Security.Claims;

namespace WebApp.Models
{
    public static class ModelsUtility
    {
        public static IHttpContextAccessor _httpContextAccessor { get; private set; }
        public static IConfiguration _configuration;
        public static IWebHostEnvironment _iHostingEnvironment;
        public static int PgSize { get; private set; }
        public static void Configure(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IWebHostEnvironment iHostingEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration= configuration;
            _iHostingEnvironment= iHostingEnvironment;
            PgSize = ModelsUtility.ConvertToInt(Setting("AppSetting:PgSize"));
        }
        public static string Setting(string key)
        {
            return _configuration.GetSection(key).Value;
        }
        public static int GetUserID()
        {
            return ModelsUtility.ConvertToInt(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        public static int ConvertToInt(dynamic data)
        {
            int.TryParse(data, out int result);
            return result;
        }
        public static string GetWebRootPath()
        {
            return _iHostingEnvironment.WebRootPath;
        }
    }
}
