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
    public class PlayerController : Controller
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
            //
            protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
            {
                var identity = await base.GenerateClaimsAsync(user);
                identity.AddClaim(new Claim("UserName", user.UserName ?? ""));
                identity.AddClaim(new Claim("Email", user.Email ?? ""));
                return identity;
            }
        }

        public IActionResult Index()
        {

            //var user = HttpContext.User.Identity.Name;
            //var UserEntity = _context.DBUsers.FirstOrDefault(o => o.UserName == user);
            //string Email = UserEntity.Email;
            var EmailContext = _httpContextAccessor.HttpContext.User.FindFirst("Email").Value;
            
            bool IsAdmin = HttpContext.User.IsInRole("admin");        
            if (IsAdmin == true)
            {
                ViewData["SomeMessage"] = "My congratulations you are admin";
                //ViewData["EmailMessege"] = Email;
            }
            var Tracklist = _context.Tracks.ToList();
            //var Albumlist = _context.Tracks.ToList();
            //ViewData["TrackUrl"] = list;

            return View(Tracklist);
        }

        public IActionResult GetPlay()
        {
            var PlaySong = _context.Tracks.Where(alb => alb.Album.Value == Album.Play).ToList();
            return Json(PlaySong);

        }

        public IActionResult GetListToPlay()
        {
            var PlaySong = _context.Tracks.Where(alb => alb.Album.Value == Album.ListToPlay).ToList();
            return Json(PlaySong);

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
        public async Task<IActionResult> CreateFromPlayer(string url)
        {
            if (ModelState.IsValid)
            {
                //Album id = Album.ListToPlay;
                _context.Tracks.Add(new Track { TrackID = Guid.NewGuid(), Album = Album.ListToPlay, TrackUrl = url });
                //track.TrackID = Guid.NewGuid();
                //_context.Add(track);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Player");
        }

    }
}