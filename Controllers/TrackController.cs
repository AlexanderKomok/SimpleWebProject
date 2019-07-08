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
    public class SomeModel
    {
        public IEnumerable<Track> Tracks { get; set; }
        public IEnumerable<Album> Albums { get; set; }
    }

    public class TrackController : Controller
    {
        private readonly ApplicationContext _context;

        public TrackController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Track
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Tracks.Include(t => t.User);
            var tracks = await applicationContext.ToListAsync();
            var albums = await _context.Albums.Include(t => t.User).ToListAsync();

            var result = new SomeModel
            {
                Tracks = tracks,
                Albums = albums
            };

            return View(result);
        }

        

        // GET: Track/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TrackID == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // GET: Track/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.DBUsers, "Id", "UserName");
            ViewData["AlbumName"] = new SelectList(_context.Albums, "AlbumID", "AlbumName");
            return View();
        }

        // POST: Track/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackID,AlbumName,UserId,TrackUrl,ArtistName,TrackName,Grade")] Track track)
        {
            if (ModelState.IsValid)
            {
                track.TrackID = Guid.NewGuid();
                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.DBUsers, "Id", "userName", track.UserId);
            ViewData["AlbumName"] = new SelectList(_context.Albums, "AlbumID", "AlbumName", track.AlbumName);
            return View(track);
        }

        // GET: Track/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks.FindAsync(id);
            if (track == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.DBUsers, "Id", "userName", track.UserId);
            ViewData["AlbumName"] = new SelectList(_context.Albums, "AlbumID", "AlbumName", track.AlbumName);
            return View(track);
        }

        // POST: Track/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TrackID,UserId,AlbumName,TrackUrl,ArtistName,TrackName,Grade")] Track track)
        {
            if (id != track.TrackID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(track);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackExists(track.TrackID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.DBUsers, "Id", "Id", track.UserId);
            ViewData["AlbumName"] = new SelectList(_context.Albums, "AlbumID", "AlbumName", track.AlbumName);
            return View(track);
        }

        // GET: Track/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var track = await _context.Tracks
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TrackID == id);
            if (track == null)
            {
                return NotFound();
            }

            return View(track);
        }

        // POST: Track/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var track = await _context.Tracks.FindAsync(id);
            _context.Tracks.Remove(track);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackExists(Guid id)
        {
            return _context.Tracks.Any(e => e.TrackID == id);
        }
    }
}
