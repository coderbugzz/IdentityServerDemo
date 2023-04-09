using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Quickstart.Config
{
    public class ConfigController : Controller
    {
        private readonly ConfigurationDbContext _context;

        public ConfigController(ConfigurationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Client client = new Client();
            
            return View(client);
        }
    }
}
