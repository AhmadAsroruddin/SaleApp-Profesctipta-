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

    [Route("Create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [Route("Edit")]
    public IActionResult Edit()
    {
        return View();
    }
}