using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPG.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RPG.Controllers
{
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            ViewBag.Location = _db.Locations.ToList();
            return View();
        }
        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult Create(Item item, string locationId)
        {
            item.CharacterId = 0;
            item.LocationId = Int32.Parse(locationId);
            _db.Items.Add(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

       
        public IActionResult Index()
        {
            return View();
        }
    }
}
