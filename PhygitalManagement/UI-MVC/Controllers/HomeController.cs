using System.Diagnostics;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PM.BL.Domain.contact;
using PM.DAL.EF;
using PM.UI.Web.MVC.Models;

namespace PM.UI.Web.MVC.Controllers;

public class HomeController : Controller
{
    private readonly UnitOfWork _uow;
    private readonly PhygitalDbContext _dbContext;
    
    public HomeController(UnitOfWork uow, PhygitalDbContext dbContext)
    {
        _uow = uow;
        _dbContext = dbContext;
    }
    
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult Contact()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Contact(ContactViewModel contactVm)
    {
        if (ModelState.IsValid)
        {
            _uow.BeginTransaction();
            var contact = new Contact
            {
                Name = contactVm.Name,
                Email = contactVm.Email,
                Phone = contactVm.Phone,
                Subject = contactVm.Subject,
                Message = contactVm.Message
            };
            
            _dbContext.Contacts.Add(contact);
            _uow.Commit();
            TempData["SuccessMessage"] = "Your contact details have been successfully saved!";
            return RedirectToAction("Contact");
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult SetLanguage(string culture, string returnUrl)
    {
        _uow.BeginTransaction();
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
        _uow.Commit();
        return LocalRedirect(returnUrl);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    
}