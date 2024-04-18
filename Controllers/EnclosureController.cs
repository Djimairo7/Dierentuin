using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Dierentuin.Controllers;

public class EnclosureController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}