using InfilonMVCWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace InfilonMVCWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;

        List<User> user = new List<User>();

        public HomeController(
            ILogger<HomeController> logger, 
            IHttpClientFactory httpClientFactory, 
            ApplicationDbContext db,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _db = db;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/users/1/todos");

            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            user = JsonSerializer.Deserialize<List<User>>(responseString, options);
            List<User> userData = _db.tblUsers.ToList();
            if (userData.Count == 0)
            {

                foreach (User data in user)
                {
                    _db.tblUsers.Add(data);
                }
                await _db.SaveChangesAsync();
                return View(user);
            }
            else
            {
                return View(userData);
            }
        }

        public async Task<IActionResult> Edit(int Id)
        {
            try
            {
                var userData = await Task.FromResult(_db.tblUsers.Find(Id));
                return View(userData);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User uData)
        {
            try
            {
                User userData = await Task.FromResult(_db.tblUsers.Find(uData.Id));
                if(userData != null)
                {
                    
                    userData.UserId = uData.UserId;
                    userData.Title = uData.Title;
                    userData.Completed = uData.Completed;
                    _db.tblUsers.Update(userData);
                   await _db.SaveChangesAsync();
                }
                return View(userData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> PostData()
        {
            /*  try
               {

                 var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/users/1/todos");

                 var httpClient = _httpClientFactory.CreateClient();

                 var response = await httpClient.SendAsync(request);

                 if (!response.IsSuccessStatusCode)
                 {
                     return NotFound();
                 }

                 var responseString = await response.Content.ReadAsStringAsync();
                 var options = new JsonSerializerOptions
                 {
                     PropertyNameCaseInsensitive = true
                 };

                 List<User> userData = JsonSerializer.Deserialize<List<User>>(responseString, options);
                 foreach (User data in userData)
                   { 
                       _db.tblUsers.Add(data);
                   }
                 await _db.SaveChangesAsync();
                 return Ok();
             }
               catch (Exception ex)
               {
                   throw ex;
               }
            */
            return Ok();
        }
    }
}