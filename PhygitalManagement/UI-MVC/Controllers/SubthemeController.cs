using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using PM.UI.Web.MVC.Models;
using PM.BL.subthemeManager;
using PM.DAL.EF;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace PM.UI.Web.MVC.Controllers;

public class SubthemeController : Controller
{
    private readonly ISubthemeManager _subthemeManager;
    private readonly UnitOfWork _uow;

    public SubthemeController(ISubthemeManager subthemeManager, UnitOfWork uow)
    {
        _subthemeManager = subthemeManager;
        _uow = uow;
    }

    [HttpGet]
    [Authorize]
    public IActionResult AddSubtheme(int flowId)
    {
        var model = new SubthemeViewModel
        {
            FlowId = flowId
        };
        return View(model);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddSubtheme(SubthemeViewModel subthemeViewModel)
    {
        _uow.BeginTransaction();
        if (ModelState.IsValid)
        {
            var files = HttpContext.Request.Form.Files;

            foreach (var image in files)
            {
                if (image != null && image.Length > 0)
                {
                    var uploads = Path.Combine("UI-MVC", "wwwroot", "images");
                    if (string.IsNullOrEmpty(uploads))
                    {
                        throw new InvalidOperationException("Upload path could not be determined.");
                    }

                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"');
                    var filePath = Path.Combine(uploads, fileName);

                    using (var memoryStream = new MemoryStream())
                    {
                        await image.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;

                        using (var img = await Image.LoadAsync(memoryStream))
                        {
                            img.Mutate(x => x.Resize(800, 600));
                            await img.SaveAsync(filePath);
                            subthemeViewModel.Image = fileName;
                        }
                    }
                }
            }

            var subtheme = _subthemeManager.AddSubtheme(subthemeViewModel.Name, subthemeViewModel.Description, subthemeViewModel.Image, subthemeViewModel.FlowId);
            int subthemeId = subtheme.Id;

            _uow.Commit();

            return RedirectToAction("AddQuestions", new { name = subthemeViewModel.Name, description = subthemeViewModel.Description, subthemeId, flowId = subthemeViewModel.FlowId });
        }

        return View(subthemeViewModel);
    }

    [HttpPost]
    [Authorize]
    public IActionResult AddQuestions(QuestionsViewModel questionsViewModel)
    {
        if (!ModelState.IsValid)
        {
            _uow.BeginTransaction();
            return View(questionsViewModel);
        }

        _uow.Commit();
        return RedirectToAction("Index", "Flow");
    }

    [HttpGet]
    public IActionResult AddQuestions(string name, string description, int subthemeId, int flowId)
    {
        var model = new QuestionsViewModel
        {
            SubthemeName = name,
            SubthemeDescription = description,
            SubthemeId = subthemeId,
            FlowId = flowId
        };
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var subtheme = _subthemeManager.GetSubthemeById(id);
        return View(subtheme);
    }
    
    [HttpPost]
    [Authorize]
    public IActionResult ToggleActivation(int subthemeId)
    {
        var subtheme = _subthemeManager.GetSubthemeById(subthemeId);
        if (subtheme != null)
        {
            _uow.BeginTransaction();
            subtheme.IsActivated = !subtheme.IsActivated;
            _subthemeManager.UpdateSubtheme(subtheme);
            _uow.Commit();
        }
        return RedirectToAction("Index", "Flow", new { flowId = subtheme.FlowId });
    }
}
