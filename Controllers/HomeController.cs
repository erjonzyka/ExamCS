using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExamCS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
namespace ExamCS.Controllers;


public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it   
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;
    }

    [SessionCheck]
    public IActionResult Index()
    {
        PartialsDataIndex Pdx = new PartialsDataIndex();
        List<Hobby>? AllHobbies = _context.Hobbies.Include(e => e.Proficencies).ToList();
        Pdx.AllHobbies = AllHobbies;
        List<Hobby>? Novice = _context.Hobbies.Include(h => h.Proficencies).Where(h => h.Proficencies.Any(p => p.Level == "Novice")).OrderByDescending(h => h.Proficencies.Count).ToList();
        List<Hobby>? Intermediate = _context.Hobbies.Include(h => h.Proficencies).Where(h => h.Proficencies.Any(p => p.Level == "Intermediate")).OrderByDescending(h => h.Proficencies.Count).ToList();
        List<Hobby>? Expert = _context.Hobbies.Include(h => h.Proficencies).Where(h => h.Proficencies.Any(p => p.Level == "Expert")).OrderByDescending(h => h.Proficencies.Count).ToList();
        Pdx.Novice = Novice;
        Pdx.Intermediate = Intermediate;
        Pdx.Expert = Expert;
        return View(Pdx);
    }

    [SessionCheck]
    [HttpGet("logout")]
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Login");
    }

    [SessionCheck]
    [HttpGet("hobbies/new")]
    public IActionResult CreateHobby()
    {
        return View();
    }


    [SessionCheck]
    [HttpPost("hobbies/post")]
    public IActionResult AddHobby(Hobby hobby)
    {
        if (_context.Hobbies.Any(e => e.Name == hobby.Name))
        {
            ModelState.AddModelError("Name", "This hobby already exists!");
            return View("CreateHobby", hobby);
        }
        else if (ModelState.IsValid)
        {
            _context.Add(hobby);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        else
        {
            return View("CreateHobby", hobby);
        }
    }

    [SessionCheck]
    [HttpGet("hobbies/{id}")]
    public IActionResult HobbyDetails(int id)
    {
        PartialsDataFirst Pdf = new PartialsDataFirst();
        Hobby? showHobby = _context.Hobbies.Include(e => e.Proficencies).ThenInclude(e => e.Enthusiast).FirstOrDefault(e => e.HobbyId == id);
        Pdf.Hobby = showHobby;
        ViewBag.HobbyId = showHobby.HobbyId;
        return View(Pdf);
    }

    [SessionCheck]
    [HttpPost("enthusiast/add/{id}")]
    public IActionResult AddEnth(Proficency prof, int id)
    {
        if (_context.Proficencies.Any(e => e.EnthusiastId == prof.EnthusiastId && e.HobbyId == id))
        {
            ModelState.AddModelError("Level", "This Enthusiast already exists!");
            PartialsDataFirst Pdf = new PartialsDataFirst();
            Hobby? showHobby = _context.Hobbies.Include(e => e.Proficencies).ThenInclude(e => e.Enthusiast).FirstOrDefault(e => e.HobbyId == id);
            Pdf.Hobby = showHobby;
            ViewBag.HobbyId = showHobby.HobbyId;
            return View("HobbyDetails", Pdf);
        }
        else if (ModelState.IsValid)
        {
            prof.HobbyId = id;
            _context.Add(prof);
            _context.SaveChanges();
            return RedirectToAction("HobbyDetails", new { id = id });
        }
        else
        {
            PartialsDataFirst Pdf = new PartialsDataFirst();
            Hobby? showHobby = _context.Hobbies.Include(e => e.Proficencies).ThenInclude(e => e.Enthusiast).FirstOrDefault(e => e.HobbyId == id);
            Pdf.Hobby = showHobby;
            ViewBag.HobbyId = showHobby.HobbyId;
            return View("HobbyDetails", Pdf);
        }
    }


    [SessionCheck]
    [HttpGet("hobbies/edit/{id}")]
    public IActionResult EditHobby(int id)
    {
        Hobby? hobby = _context.Hobbies.FirstOrDefault(e => e.HobbyId == id);
        return View(hobby);
    }

    [SessionCheck]
    [HttpPost("hobbies/edit/{id}")]
    public IActionResult PostEditHobby(Hobby hobbyy, int id)
{
    if (ModelState.IsValid)
    {
        Hobby hobbyFromDb = _context.Hobbies.FirstOrDefault(e => e.HobbyId == id);
        if (hobbyy.Name != hobbyFromDb.Name)
        {
            if (_context.Hobbies.Any(e => e.Name == hobbyy.Name))
            {
                ModelState.AddModelError("Name", "This hobby already exists");
                Hobby? hobby = _context.Hobbies.FirstOrDefault(e => e.HobbyId == id);
                return View("EditHobby", hobby);
            }
            hobbyFromDb.Name = hobbyy.Name;
        }
        hobbyFromDb.Description = hobbyy.Description;
        _context.SaveChanges();
        return RedirectToAction("HobbyDetails", new { id = id });
    }
    else
    {
        Hobby? hobby = _context.Hobbies.FirstOrDefault(e => e.HobbyId == id);
        return View("EditHobby", hobby);
    }
}


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}



public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId = context.HttpContext.Session.GetInt32("UserId");
        if (userId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }
}