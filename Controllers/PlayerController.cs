using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppTry3.DBEntities;
using WebAppTry3.Models;

namespace WebAppTry3.Controllers
{
    
    [Authorize]
    public class PlayerController : Controller
    {
        private readonly ApplicationContext _context;
        public PlayerController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            bool IsAdmin = HttpContext.User.IsInRole("admin");

            if (IsAdmin == true)
            {
                ViewData["SomeMessage"] = "My congratulations you are admin";
            }
            var Tracklist = _context.Tracks.ToList();
            //var Albumlist = _context.Tracks.ToList();
            //ViewData["TrackUrl"] = list;

            return View(Tracklist);
        }
        
        //GET
        public IActionResult CreateFromPlayer()
        {
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> CreateFromPlayer(string url)
        {
            if (ModelState.IsValid)
            {
                _context.Tracks.Add(new Track { TrackID = Guid.NewGuid(), AlbumName = "", TrackUrl = url });
                //track.TrackID = Guid.NewGuid();
                //_context.Add(track);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Player");
        }

    }
}