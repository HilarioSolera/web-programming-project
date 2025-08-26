using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Modelos;



namespace Proyecto1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}