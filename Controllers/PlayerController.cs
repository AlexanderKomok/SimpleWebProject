using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppTry3.DBEntities;
using WebAppTry3.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;


namespace WebAppTry3.Controllers
{
    
    [Authorize]
    public class PlayerController : Microsoft.AspNetCore.Mvc.Controller
    {

        private readonly ApplicationContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PlayerController(ApplicationContext context, IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;

        }

        


        //My own implementation of IUserClaimsPrincipalFactory(just kidding it from the internet)
        public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole>
        {
            public MyUserClaimsPrincipalFactory(
                UserManager<User> userManager,
                RoleManager<IdentityRole> roleManager,
                IOptions<IdentityOptions> optionsAccessor)
                : base(userManager, roleManager, optionsAccessor)
            {
            }
         
            protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
            {
                var identity = await base.GenerateClaimsAsync(user);
                identity.AddClaim(new Claim("UserName", user.UserName ?? ""));
                identity.AddClaim(new Claim("Email", user.Email ?? ""));
                return identity;
            }
        }

        public class FullTrackList
        {
            public List<Track> ListPlay { get; set; }
            public List<Track> ListToPlay { get; set; }
            public List<Track> ListHistory { get; set; }
        }

        

        public IActionResult DisplayPartialView()
        {
            string viewUrl = Url.RouteUrl(new { Controller = "Player", Action = "Index" });

            System.Net.WebClient client = new System.Net.WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            string result = client.DownloadString(new Uri(viewUrl));
            return PartialView(result);
        }

        public IActionResult Index()
        {
            var EmailContext = _httpContextAccessor.HttpContext.User.FindFirst("Email").Value;
            var FullSortTrackList = new FullTrackList();
            FullSortTrackList.ListPlay = _context.Tracks.Where(alb => alb.Album.Value == Album.Play).ToList();
            FullSortTrackList.ListToPlay = _context.Tracks.Where(alb => alb.Album.Value == Album.ListToPlay).ToList();
            FullSortTrackList.ListHistory = _context.Tracks.Where(alb => alb.Album.Value == Album.History).ToList();

            return View(FullSortTrackList);
        }

        public IActionResult GetPlay()
        {
            var PlaySong = _context.Tracks.Where(alb => alb.Album.Value == Album.Play).ToList();
            return Json(PlaySong);

        }

        public IActionResult GetListToPlay()
        {
            var PlaySong = _context.Tracks.Where(alb => alb.Album.Value == Album.ListToPlay).ToList();
            return Ok(PlaySong);

        }

        public IActionResult GetHistory()
        {
            var PlaySong = _context.Tracks.Where(alb => alb.Album.Value == Album.History).ToList();
            return Json(PlaySong);

        }

        //public JsonResult AllSongs()
        //{
        //    var tracks = _context.Tracks;
        //    return new JsonResult(tracks);
        //}

        //GET
        public IActionResult CreateFromPlayer()
        {
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> CreateFromPlayer(string url, string title)
        {
            if (ModelState.IsValid)
            {               
                //Album id = Album.ListToPlay;
                _context.Tracks.Add(new Track { TrackID = Guid.NewGuid(), Album = Album.ListToPlay, TrackUrl = url, TrackName = title});
                //track.TrackID = Guid.NewGuid();
                //_context.Add(track);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Player");
        }

        //GET
        public IActionResult ChangeFromPlayer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeFromPlayer(string url, Track track)
        {
            if (ModelState.IsValid)
            {                
                var PlaySong = _context.Tracks.FirstOrDefault(alb => alb.Album.Value == Album.Play);
                PlaySong.Album = Album.History;
                
                var TrackEntity = _context.Tracks.FirstOrDefault(o => o.TrackUrl == url);
                TrackEntity.Album = Album.Play;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Player");
        }
    }
}