using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        if (isLogged()) return RedirectToAction("Index", "Login");
        return View();
    }

    private bool isLogged()
    {
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("User"));
    }


    public IActionResult Privacy()
    {
        if (isLogged()) return RedirectToAction("Index", "Login");
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
