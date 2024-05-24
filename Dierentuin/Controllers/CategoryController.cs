using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace Dierentuin.Controllers;

public class CategoryController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}