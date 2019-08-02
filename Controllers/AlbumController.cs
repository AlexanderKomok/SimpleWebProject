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
    public class AlbumController : Controller
    {
        private readonly ApplicationContext _context;

        public AlbumController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: Album
        public async Task<IActionResult> Index()
        {
            var applicationContext = _context.Albums.Include(a => a.User).Include(b => b.ConnectEntities);

            //ViewData["hi"] = "hello";
            var album = await applicationContext.ToListAsync();

            return View(album);
        }

        // GET: Album/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Album/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Album album)
        {
            if (ModelState.IsValid)
            {
                album.AlbumId = Guid.NewGuid();
                var AlbumOwn = _context.DBUsers.FirstOrDefault(to => to.UserName == User.Identity.Name);
                album.UserId = AlbumOwn.Id;

                _context.Add(album);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(album);
        }

        public async Task<IActionResult> GetTracks(Guid albumId)
        {

            //var album = _context.Albums.Include(t => t.ConnectEntities).FirstOrDefault(a => a.AlbumId == albumId);
            //var track1 = album.ConnectEntities.Select(t => t.Track);

            ViewBag.albumId = albumId;
            var applicationContext = _context.Tracks.Include(t => t.User);
            var track = await applicationContext.ToListAsync();

            return View(track);
        }

        //POST
        public async Task<IActionResult> InAlbum(Guid albumId, Guid trackId, Track_Album track_Album)
        {
            track_Album.Track_AlbumId = Guid.NewGuid();
            track_Album.AlbumId = albumId;
            track_Album.TrackId = trackId;
            var album = _context.Albums.FirstOrDefault(a => a.AlbumId == albumId);
            var track = _context.Tracks.FirstOrDefault(t => t.TrackID == trackId);
            track_Album.Album = album;
            track_Album.Track = track;
            _context.Add(track_Album);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetTracks");
        }

        //public class AlbumAndTrack
        //{
        //    public List<Track> ListTrack { get; set; }
        //    public List<Album> ListAlbum { get; set; }
        //}

        public IActionResult OutputAllTrackFromAlbum(Guid? albumId)
        {
            var output = _context.Track_Albums.Where(ta => ta.AlbumId == albumId);
            List<Guid> trackIdList = new List<Guid>();
            foreach(var item in output)
            {
                trackIdList = item.TrackId;
                
            }
            
            //var trackId = output.TrackId;
            //var AlbumAndTrackFull = new AlbumAndTrack();
            //AlbumAndTrackFull.ListAlbum = _context.Albums
            return View();
        }


        // GET: Album/Edit
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Track/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AlbumName")] Album album)
        {
            if (id != album.AlbumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var AlbumOwn = _context.DBUsers.FirstOrDefault(to => to.UserName == User.Identity.Name);
                    album.UserId = AlbumOwn.Id;
                    _context.Update(album);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            };

            return View(album);
        }
        private bool AlbumExists(Guid id)
        {
            return _context.Albums.Any(a => a.AlbumId == id);
        }


    }
}