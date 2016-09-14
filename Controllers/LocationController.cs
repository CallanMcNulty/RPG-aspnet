using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RPG.Controllers
{
    public class LocationController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public LocationController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, Authorize(Roles = "admin")]
        public IActionResult Create(Location location)
        {
            _db.Locations.Add(location);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            List<Location> locations = _db.Locations.ToList();
            return View(locations);
        }

        private async Task<Location> GetLocationInDirection(int direction)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            Character character = _db.Characters.FirstOrDefault(c => c.UserId == currentUser.Id);
            Location current = _db.Locations.FirstOrDefault(l => l.Id == character.LocationId);
            int X = current.XCoord;
            int Y = current.YCoord;
            if (direction == 0)
            {
                Y++;
            }
            else if (direction == 1)
            {
                X++;
            }
            else if (direction == 2)
            {
                Y--;
            }
            else if (direction == 3)
            {
                X--;
            }
            return _db.Locations.FirstOrDefault(l => l.XCoord == X && l.YCoord == Y);
        }

        public async Task<IActionResult> Current(int id)
        {
            Location current = _db.Locations.Include(l => l.Items).FirstOrDefault(l => l.Id == id);
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            Character character = _db.Characters.FirstOrDefault(c => c.UserId == currentUser.Id);
            ViewBag.CanGoNorth = true;
            ViewBag.CanGoSouth = true;
            ViewBag.CanGoEast = true;
            ViewBag.CanGoWest = true;
            ViewBag.IsCurrentLocation = false;

            if (character.LocationId == id)
            {
                ViewBag.IsCurrentLocation = true;
            }
            if( await GetLocationInDirection(0) == null)
            {
                ViewBag.CanGoNorth = false;
            }
            if (await GetLocationInDirection(1) == null)
            {
                ViewBag.CanGoEast = false;
            }
            if (await GetLocationInDirection(2) == null)
            {
                ViewBag.CanGoSouth = false;
            }
            if (await GetLocationInDirection(3) == null)
            {
                ViewBag.CanGoWest = false;
            }
            return View(current);
        }

        public async Task<IActionResult> Go(int direction)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            Character character = _db.Characters.FirstOrDefault(c => c.UserId == currentUser.Id);
            Location current = _db.Locations.FirstOrDefault(l => l.Id == character.LocationId);
            Location NewLocation = await GetLocationInDirection(direction);
            if (NewLocation == null)
            {
                return RedirectToAction("Current", new { id = current.Id});
            }
            character.LocationId = NewLocation.Id;
            _db.Entry(character).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Current", new { id = NewLocation.Id });
        }

        public async Task<IActionResult> Loot(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            Character character = _db.Characters.FirstOrDefault(c => c.UserId == currentUser.Id);
            Item lootedItem = _db.Items.FirstOrDefault(i => i.Id == id);
            lootedItem.CharacterId = character.Id;
            lootedItem.LocationId = 0;
            _db.Entry(lootedItem).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Current", new { id = character.LocationId});
        }

        public async Task<IActionResult> Drop(int id)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            Character character = _db.Characters.FirstOrDefault(c => c.UserId == currentUser.Id);
            Item droppedItem = _db.Items.FirstOrDefault(i => i.Id == id);
            droppedItem.CharacterId = 0;
            droppedItem.LocationId = character.LocationId;
            _db.Entry(droppedItem).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Current", new { id = character.LocationId });
        }
    }
}
