using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppTry3.DBEntities;
using WebAppTry3.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Collections.Generic;


namespace WebAppTry3.Controllers
{

    public static class ControllerExtensions
    {
        public static async Task<string> RenderViewAsync<TModel>(this Controller controller, string viewName, TModel model, bool isPartial = false)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }

            controller.ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
                ViewEngineResult viewResult = GetViewEngineResult(controller, viewName, isPartial, viewEngine);

                if (viewResult.Success == false)
                {
                    throw new System.Exception($"A view with the name {viewName} could not be found");
                }

                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }

        private static ViewEngineResult GetViewEngineResult(Controller controller, string viewName, bool isPartial, IViewEngine viewEngine)
        {
            if (viewName.StartsWith("~/"))
            {
                var hostingEnv = controller.HttpContext.RequestServices.GetService(typeof(IHostingEnvironment)) as IHostingEnvironment;
                return viewEngine.GetView(hostingEnv.WebRootPath, viewName, !isPartial);
            }
            else
            {
                return viewEngine.FindView(controller.ControllerContext, viewName, !isPartial);

            }
        }
    }


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
                identity.AddClaim(new Claim("Id", user.Id ?? ""));
                return identity;
            }
        }

        //public class FullTrackList
        //{
        //    public List<Track> ListPlay { get; set; }
        //    public List<Track> ListToPlay { get; set; }
        //    public List<Track> ListHistory { get; set; }
        //}

        public async Task<IActionResult> DisplayPartialView()
        {            
            var FullSortTrackList = new FullTrackList();
            FullSortTrackList.ListPlay = _context.Tracks.Where(alb => alb.PlayerState.Value == PlayerState.Play).ToList();
            FullSortTrackList.ListToPlay = _context.Tracks.Where(alb => alb.PlayerState.Value == PlayerState.ListToPlay).ToList();
            FullSortTrackList.ListHistory = _context.Tracks.Where(alb => alb.PlayerState.Value == PlayerState.History).ToList();
            var partialViewHtml = await this.RenderViewAsync("Index", FullSortTrackList);
            return Ok(partialViewHtml);

        }

        public IActionResult Index()
        {
            var EmailContext = _httpContextAccessor.HttpContext.User.FindFirst("Email").Value;
            var FullSortTrackList = new FullTrackList();
            FullSortTrackList.ListPlay = _context.Tracks.Where(alb => alb.PlayerState.Value == PlayerState.Play).ToList();
            FullSortTrackList.ListToPlay = _context.Tracks.Where(alb => alb.PlayerState.Value == PlayerState.ListToPlay).ToList();
            FullSortTrackList.ListHistory = _context.Tracks.Where(alb => alb.PlayerState.Value == PlayerState.History).ToList();

            return View(FullSortTrackList);
        }

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
                var trackOwn = _context.DBUsers.FirstOrDefault(to => to.UserName == User.Identity.Name);
                var UserIdFromLINQ = trackOwn.Id;
                _context.Tracks.Add(new Track { TrackID = Guid.NewGuid(), PlayerState = PlayerState.ListToPlay, TrackUrl = url, TrackName = title, UserId = UserIdFromLINQ});
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
        public async Task<IActionResult> ChangeFromPlayer(string songUrl)
        {
            if (ModelState.IsValid)
            {
                //Get what's playing now and send to history
                var PlaySong = _context.Tracks.FirstOrDefault(alb => alb.PlayerState.Value == PlayerState.Play);
                PlaySong.PlayerState = PlayerState.History;

                //Get a song from the playlist and move to playable
                var TrackEntity = _context.Tracks.FirstOrDefault(o => o.TrackUrl == songUrl);
                TrackEntity.PlayerState = PlayerState.Play;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index", "Player");
        }
    }
}