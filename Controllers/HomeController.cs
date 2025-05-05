using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Models;

namespace Project1.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    public readonly ApplicationDbContext _DbContext;
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbcontext)
    {
        _logger = logger;
        _DbContext = dbcontext;
    }

    [HttpGet]
    public async Task<IActionResult> GetImage(int id)
    {

        Image image = await _DbContext.Images.FirstOrDefaultAsync(image => image.PostId == id);
        if (image == null)
        {
            return NotFound("ass hole there was no image to get");
        }

        return File(image.Content, image.ContentType);
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Post> posts = await _DbContext.Posts.ToListAsync();
        return View(posts);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Post post = await _DbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        return View(post);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

