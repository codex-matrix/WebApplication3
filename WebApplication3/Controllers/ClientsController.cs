using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;


namespace WebApplication3.Controllers
{
    public class ClientsController : Controller
    {
        private readonly AppDbContext _db;
        public ClientsController(AppDbContext db) { _db = db; }

        public async Task<IActionResult> Index()
        {
            var clients = await _db.Clients.OrderBy(c => c.FirstName).ToListAsync();
            return View(clients);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = await _db.Clients.FindAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }
    }
}