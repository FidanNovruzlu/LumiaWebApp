using LumiaWebApp.DAL;
using LumiaWebApp.Models;
using LumiaWebApp.ViewModels.TestimonialsVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LumiaWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles ="Admin")]
public class TestimonialsController : Controller
{
    private readonly LumiaDbContext _lumiaDbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public TestimonialsController(LumiaDbContext lumiaDbContext,IWebHostEnvironment webHostEnvironment)
    {
        _lumiaDbContext = lumiaDbContext;
        _webHostEnvironment = webHostEnvironment;
    }
    public async  Task<IActionResult> Index()
    {
        List<Testimonials> testimonials = await _lumiaDbContext.Testimonials.Include(t=>t.Job).ToListAsync();
        return View(testimonials);
    }
    public async Task< IActionResult> Create()
    { 
        CreateTestimonialsVM createTestimonialsVM = new CreateTestimonialsVM()
        {
            Jobs =  _lumiaDbContext.Jobs.ToList(),
        };
        return View(createTestimonialsVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTestimonialsVM createVM)
    {
        if (!ModelState.IsValid)
        {
            createVM.Jobs = _lumiaDbContext.Jobs.ToList();
            return View(createVM);
        }
        Testimonials testimonials = new Testimonials()
        {
            Name=createVM.Name,
            Surname=createVM.Surname,
            Bio=createVM.Bio,
            JobId=createVM.JobId
        };

        if(!createVM.ProfileImage.ContentType.Contains("image/") && createVM.ProfileImage.Length /1024 > 2048)
        {
            ModelState.AddModelError("", "Incorrect image type or size!");
            createVM.Jobs = _lumiaDbContext.Jobs.ToList();
            return View(createVM);
        }
      
        string newFilename = Guid.NewGuid().ToString() + createVM.ProfileImage.FileName;
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "testimonials", newFilename);

        using(FileStream fileStream = new FileStream(path, FileMode.CreateNew))
        {
            await createVM.ProfileImage.CopyToAsync(fileStream);
        }
        testimonials.ProfileImageName = newFilename;

        await _lumiaDbContext.Testimonials.AddAsync(testimonials);
        await _lumiaDbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(int id)
    {
        Testimonials? testimonials = await _lumiaDbContext.Testimonials.FindAsync(id);
        if(testimonials==null) return NotFound();

        UpdatetestimonialsVM updateVM = new UpdatetestimonialsVM()
        {
            Jobs = _lumiaDbContext.Jobs.ToList(),
            Name= testimonials.Name,
            Surname= testimonials.Surname,
            Bio= testimonials.Bio,
            ProfileImageNmae=testimonials.ProfileImageName,
            JobId=testimonials.JobId
        };
        return View(updateVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdatetestimonialsVM updateVM)
    {
        if (!ModelState.IsValid)
        {
            updateVM.Jobs = _lumiaDbContext.Jobs.ToList();
            return View(updateVM);
        }

        Testimonials? testimonials = await _lumiaDbContext.Testimonials.FindAsync(id);
        if (testimonials == null) return NotFound();

        if (!updateVM.ProfileImage.ContentType.Contains("image/") && updateVM.ProfileImage.Length / 1024 > 2048)
        {
            ModelState.AddModelError("", "Incorrect image type or size!");
            updateVM.Jobs = _lumiaDbContext.Jobs.ToList();
            return View(updateVM);
        }

        if (updateVM.ProfileImage != null)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "testimonials", testimonials.ProfileImageName);

            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                await updateVM.ProfileImage.CopyToAsync(fileStream);
            }
        }
      
        testimonials.Surname = updateVM.Surname;
        testimonials.Name = updateVM.Name;
        testimonials.JobId = updateVM.JobId;
        testimonials.Bio = updateVM.Bio;

        await _lumiaDbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        Testimonials? testimonials = await _lumiaDbContext.Testimonials.FindAsync(id);
        if (testimonials == null) return NotFound();

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "testimonials", testimonials.ProfileImageName);

        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }

        _lumiaDbContext.Testimonials.Remove(testimonials);

        await _lumiaDbContext.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> Read(int id)
    {

        Testimonials? testimonials = await _lumiaDbContext.Testimonials.Include(t=>t.Job).FirstOrDefaultAsync(t=>t.Id==id);
        if (testimonials == null) return NotFound();

        return View(testimonials);

    }
}
