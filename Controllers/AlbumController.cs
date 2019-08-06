using System;
using System.Collections;
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

        public class OutputGetTracks
        {
            public List<Track> AllTrackList { get; set; }
            public List<Track> AlbumTrackList { get; set; }
        }


        public async Task<IActionResult> GetTracks(Guid albumId)
        {
            var OutputGetTraksCurr = new OutputGetTracks();
            //All Tracks
            var allTracks = _context.Tracks.Where(t => t.TrackName != null).Select(t => t);
            ViewBag.albumId = albumId;
            TempData["AlbumId"] = albumId;

            OutputGetTraksCurr.AllTrackList = await allTracks.ToListAsync();

            //AlbumTracks
            List<Track> trackList = new List<Track>();

            var output = _context.Track_Albums.Where(ta => ta.AlbumId == albumId);

            if (ModelState.IsValid)
            {
                foreach (var item in output)
                {
                    var currTrack = _context.Tracks.Where(t => t.TrackID == item.TrackId).FirstOrDefault();
                    trackList.Add(currTrack);

                }
            }
            OutputGetTraksCurr.AlbumTrackList = trackList;

            return View(OutputGetTraksCurr);
        }

        //POST
        //Save in album
        public async Task<IActionResult> InAlbum(Guid albumId, Guid trackId, Track_Album track_Album)
        {
            track_Album.Track_AlbumId = Guid.NewGuid();
            //var nowAlbumId= (Guid)TempData["AlbumId"];
            track_Album.AlbumId = albumId;
            //track_Album.AlbumId = ViewBag.albumId;
            track_Album.TrackId = trackId;
            var album = _context.Albums.FirstOrDefault(a => a.AlbumId == albumId);
            var track = _context.Tracks.FirstOrDefault(t => t.TrackID == trackId);
            track_Album.Album = album;
            track_Album.Track = track;
            _context.Add(track_Album);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetTracks");
        }

        //Delete from album
        public async Task<IActionResult> OutAlbum(Guid albumId, Guid trackId)
        {
            var track_Album = _context.Track_Albums.Where(ta => ta.AlbumId == albumId).Where(ta => ta.TrackId == trackId).FirstOrDefault();
            _context.Track_Albums.Remove(track_Album);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetTracks");
        }

        //Delete album
        public IActionResult DeleteAlbum(Guid albumId)
        {
            var trackInAlbum = _context.Track_Albums.Where(ta => ta.AlbumId == albumId);
            foreach (var item in trackInAlbum)
            {
                if (item != null)
                {
                    _context.Track_Albums.Remove(item);
                    _context.SaveChangesAsync();
                }
            }
            var album = _context.Albums.FirstOrDefault(ta => ta.AlbumId == albumId);
            _context.Albums.Remove(album);
            _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        //public async Task<IActionResult> DeleteAlbum(Guid albumId)
        //{

        //}
        //public async Task<IActionResult> DisplayAllTrack()
        //{
        //    var allTracks = _context.Tracks.Where(t => t.TrackName != null).Select(t => t);
        //    var partialViewHtml = await this.RenderViewAsync("Index", allTracks);
        //    return Ok(partialViewHtml);
        //}

        public async Task<IActionResult> DisplayAllAlbum(Guid AlbumId)
        {
            var allTracks = _context.Track_Albums.Where(ta => ta.AlbumId == AlbumId);
            var model = new OutputGetTracks
            {
                AlbumTrackList = allTracks.Select(x => x.Track).ToList(),
                //AllTrackList = _context.Track_Albums.Select(x => x.Track).ToList()
                AllTrackList = _context.Tracks.Where(t => t.TrackName != null).ToList()
            };
            var partialViewHtml = await this.RenderViewAsync("GetTracks", model);
            return Ok(partialViewHtml);
        }

        public IActionResult OutputAllTrackFromAlbum(Guid? albumId)
        {
            List<Track> trackList = new List<Track>();

            var output = _context.Track_Albums.Where(ta => ta.AlbumId == albumId);

            if (ModelState.IsValid)
            {
                foreach (var item in output)
                {
                    var currTrack = _context.Tracks.Where(t => t.TrackID == item.TrackId).FirstOrDefault();
                    trackList.Add(currTrack);
                }
            }

            //var trackId = output.TrackId;
            //var AlbumAndTrackFull = new AlbumAndTrack();
            //AlbumAndTrackFull.ListAlbum = _context.Albums
            return View(trackList);
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

        // POST: Track/Edit
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

        //--------------------------------< Album player code >--------------------------------


        public IActionResult PlayAlbum(Guid albumId)
        {
            //Get current album
            var currAlb = _context.Track_Albums.Where(ta => ta.AlbumId == albumId);
            var FullSortTrackList = new FullTrackList();
            foreach (var item in currAlb)
            {
                //If Album have track in play fill in FullSortTrackList.ListPlay
                var currTrack = _context.Tracks.Where(t => t.TrackID == item.TrackId).Where(t => t.PlayerState == PlayerState.Play);
                if (currTrack.FirstOrDefault() != null)
                {
                    FullSortTrackList.ListPlay = currTrack.ToList();
                }
            }

            //Rebuild logic or create other 
            //if (FullSortTrackList.ListPlay == null)
            //{
            //    //Current track from play to ListToPlay
            //    var playTrack = _context.Tracks.FirstOrDefault(t => t.PlayerState == PlayerState.Play);
            //    playTrack.PlayerState = PlayerState.ListToPlay;
            //    _context.SaveChanges();
            //    //Track from Album to play
                
            //    var trackFromAlbum = _context.Tracks.FirstOrDefault(t => t.TrackID == currTrack.TrackId);
            //    trackFromAlbum.PlayerState = PlayerState.Play;
            //    _context.SaveChanges();
            //    FullSortTrackList.ListPlay = _context.Tracks.Where(t => t.PlayerState == PlayerState.Play).ToList();
            //}

            //Add ListToPlay
            List<Track> ListToPlayTracks = new List<Track>();
            foreach (var item in currAlb)
            {
                var track = _context.Tracks.Where(t => t.PlayerState == PlayerState.ListToPlay).Where(t => t.TrackID == item.TrackId).FirstOrDefault();
                if (track != null)
                {
                    ListToPlayTracks.Add(track);
                }
            }
            FullSortTrackList.ListToPlay = ListToPlayTracks;

            //Add History
            List<Track> ListHistory = new List<Track>();
            foreach (var item in currAlb)
            {
                var track = _context.Tracks.Where(t => t.PlayerState == PlayerState.History).Where(t => t.TrackID == item.TrackId).FirstOrDefault();
                if (track != null)
                {
                    ListHistory.Add(track);
                }
            }
            FullSortTrackList.ListHistory = ListHistory;

            return View(FullSortTrackList);
        }

        //
        public async Task<IActionResult> ChangePlayerStateFromAlbum(string songUrl, Guid albumId)
        {
            var currAlb = _context.Track_Albums.Where(ta => ta.AlbumId == albumId).FirstOrDefault();

            //Get what's playing now and send to history
            var PlayNow = _context.Tracks.FirstOrDefault(t => t.PlayerState == PlayerState.Play);
            PlayNow.PlayerState = PlayerState.History;

            //Get a song from the playlist and move to playable
            var track = _context.Tracks.Where(t => t.TrackUrl == songUrl).FirstOrDefault();
            track.PlayerState = PlayerState.Play;

            await _context.SaveChangesAsync();

            return RedirectToAction("PlayAlbum");
        }

        //
        public async Task<IActionResult> DisplayPlayAlbumPartial(Guid albumId)
        {
            var currAlb = _context.Track_Albums.Where(ta => ta.AlbumId == albumId);
            var FullSortTrackList = new FullTrackList();

            //Add Play
            List<Track> PlayTrack = new List<Track>();
            var currTrack = _context.Tracks.Where(t => t.PlayerState == PlayerState.Play).FirstOrDefault();
            PlayTrack.Add(currTrack);
            FullSortTrackList.ListPlay = PlayTrack;
            

            //Add ListToPlay
            List<Track> ListToPlayTracks = new List<Track>();
            foreach (var item in currAlb)
            {
                var track = _context.Tracks.Where(t => t.PlayerState == PlayerState.ListToPlay).Where(t => t.TrackID == item.TrackId).FirstOrDefault();
                if (track != null)
                {
                    ListToPlayTracks.Add(track);
                }
            }
            FullSortTrackList.ListToPlay = ListToPlayTracks;

            //Add History
            List<Track> ListHistory = new List<Track>();
            foreach (var item in currAlb)
            {
                var track = _context.Tracks.Where(t => t.PlayerState == PlayerState.History).Where(t => t.TrackID == item.TrackId).FirstOrDefault();
                if (track != null)
                {
                    ListHistory.Add(track);
                }
            }
            FullSortTrackList.ListHistory = ListHistory;

            var partialViewHtml = await this.RenderViewAsync("PlayAlbum", FullSortTrackList);
            return Ok(partialViewHtml);

        }


    }
}