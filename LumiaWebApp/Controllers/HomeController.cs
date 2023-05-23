using LumiaWebApp.DAL;
using LumiaWebApp.Models;
using LumiaWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LumiaWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly LumiaDbContext _context;
        public HomeController(LumiaDbContext context)
        {
            _context = context;
        }

        public async Task< IActionResult> Index()
        {
            List<Testimonials> testimonials = await _context.Testimonials.Include(t => t.Job).ToListAsync();
            HomeVM homeVM = new HomeVM()
            {
                Testimonials=testimonials
            };
            return View(homeVM);
        }

    }
}