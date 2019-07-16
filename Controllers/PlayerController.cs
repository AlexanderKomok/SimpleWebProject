using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppTry3.DBEntities;
using WebAppTry3.Models;

namespace WebAppTry3.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ApplicationContext _context;
        public PlayerController(ApplicationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var Tracklist = _context.Tracks.ToList();
            //var Albumlist = _context.Tracks.ToList();
            //ViewData["TrackUrl"] = list;

            return View(Tracklist);
        }
        
        //GET
        public IActionResult CreateFromPlayer()
        {
            ViewData["AlbumName"] = new SelectList(_context.Albums, "AlbumID");
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> CreateFromPlayer(Track track)
        {
            if (ModelState.IsValid)
            {
                track.TrackID = Guid.NewGuid();
                _context.Add(track);
                await _context.SaveChangesAsync();
            }
            ViewData["AlbumName"] = new SelectList(_context.Albums, "AlbumID", track.AlbumName);
            return RedirectToAction("Index", "Player");
        }

    }
}