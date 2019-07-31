using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAppTry3.DBEntities;
using WebAppTry3.Models;

namespace WebAppTry3.Controllers
{
    [Authorize]
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
            var track = await applicationContext.ToListAsync();

            return View(track);     
        }

        // GET: Track/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.DBUsers, "Id", "UserName");

            return View();
        }

        // POST: Track/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackID,Album,UserId,TrackUrl,ArtistName,TrackName")] Track track)
        {
            if (ModelState.IsValid)
            {
                track.TrackID = Guid.NewGuid();
                var trackOwn = _context.DBUsers.FirstOrDefault(to => to.UserName == User.Identity.Name);
                track.UserId = trackOwn.Id;

                _context.Add(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            ViewData["UserId"] = new SelectList(_context.DBUsers, "Id", track.UserId);

            return View(track);
        }

        // POST: Track/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TrackID,UserId,TrackUrl,ArtistName,TrackName,Album")] Track track)
        {
            if (id != track.TrackID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var trackOwn = _context.DBUsers.FirstOrDefault(to => to.UserName == User.Identity.Name);
                    track.UserId = trackOwn.Id;
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
            };

            return View(track);
        }

        public async Task<IActionResult> HistoryInListToPlay()
        {
            foreach(var item in _context.Tracks.Where(i => i.PlayerState == PlayerState.History).ToList())
            {
                item.PlayerState = PlayerState.ListToPlay;
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Index", "Track");
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
