using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using System.Net;

namespace CSI.IBTA.Administrator.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly HttpClient _httpClient;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            var authServiceApiUrl = configuration.GetValue<string>("AuthServiceApiUrl");
            if (string.IsNullOrEmpty(authServiceApiUrl))
            {
                _logger.LogError("AuthServiceApiUrl is missing in appsettings.json");
                throw new InvalidOperationException("AuthServiceApiUrl is missing in appsettings.json");
            }
            else
            {
                Console.WriteLine("skdksd");
            }
            _httpClient.BaseAddress = new Uri(authServiceApiUrl);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            var apiEndpoint = "v1/Auth";
            var dto = new LoginRequest(model.Username, model.Password);
            var jsonBody = JsonConvert.SerializeObject(dto);

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiEndpoint, content);
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true
                };
                Response.Cookies.Append("jwtToken", token, cookieOptions);

                return RedirectToAction("Index", "Home");
            }
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
            {
                ModelState.AddModelError("", "Server error");
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
            }
            return View("Index", model);
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwtToken");
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
