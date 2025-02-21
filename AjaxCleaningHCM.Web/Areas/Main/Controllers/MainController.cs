using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Infrastructure.Data;
using AjaxCleaningHCM.Core.Utils;
using AjaxCleaningHCM.Core.UserManagment.Identity;

namespace AjaxCleaningHCM.Web.Areas.Main.Controllers
{
    [Area("Main")]
    public class MainController : Controller
    {
        ApplicationDbContext context;
        IConfiguration configuration { get; }
        LogWriter logWriter = new LogWriter();
        EmailSender emailSender = new EmailSender();
        private Keys keys { get; set; }
        private readonly IHostEnvironment env;
        public UserManager<User> userManager { get; private set; }
        public MainController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            IHostEnvironment env,
            IConfiguration configuration)
        {
            this.env = env;
            this.keys = new Keys(configuration, env);
            this.configuration = configuration;
            this.userManager = userManager;
            this.context = context;
        }

        public IActionResult Index() {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        [AjaxCleaningHCMAuthorizationFilter]
        public IActionResult Modules()
        {
            return View();
        }

        [HttpGet]
        public new ActionResult Unauthorized()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult SubmissionResult()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}
