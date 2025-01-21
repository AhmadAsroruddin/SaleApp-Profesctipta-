using Microsoft.AspNetCore.Mvc;

namespace SaleApp.Controllers;

[Route("[controller]")]
public class SaleController :Controller
{
    [Route("Index")]
    public IActionResult Index()
    {
        return View();
    }
}