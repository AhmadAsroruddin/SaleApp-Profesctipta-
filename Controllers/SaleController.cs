using Microsoft.AspNetCore.Mvc;

namespace SaleApp.Controllers;

[Route("[controller]")]
public class OrderController :Controller
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

    [HttpPost]
    [Route("SaveOrder")]
    public IActionResult SaveOrder()
    {

        return Json(new { });
    }

    [Route("Edit")]
    public IActionResult Edit()
    {
        return View();
    }
}