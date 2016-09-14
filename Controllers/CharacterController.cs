using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RPG.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace RPG.Controllers
{
    public class CharacterController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public CharacterController(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> Create(Character character)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);

            character.LocationId = 0;
            character.UserId = currentUser.Id;
            _db.Characters.Add(character);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Index()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);

            Character character = _db.Characters.FirstOrDefault(c => c.UserId == currentUser.Id);

            return View(character);
        }
        public async Task<IActionResult> Inventory()
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            Character character = _db.Characters.FirstOrDefault(c => c.UserId == currentUser.Id);
            List<Item> inventory = _db.Items.Where(i => i.CharacterId == character.Id).ToList();
            character.Inventory = inventory;
            return View(character);
        }
    }
}
