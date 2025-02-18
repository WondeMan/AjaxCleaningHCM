using Microsoft.AspNetCore.Mvc;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using System.Linq;
using AjaxCleaningHCM.Core.UserManagment.Identity;
using AjaxCleaningHCM.Infrastructure.Data;

namespace AjaxCleaningHCM.Web.Areas.Account.Controllers
{
    [Area("AccountManagement")]
    [AjaxCleaningHCMAuthorizationFilter]
    public class AccessLogController : Controller
    {
        ApplicationDbContext context;

        public AccessLogController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public ActionResult Index()
        {
            //var accessLogs = context.AccessLogs.ToList();

            return View();
        }
    }
}