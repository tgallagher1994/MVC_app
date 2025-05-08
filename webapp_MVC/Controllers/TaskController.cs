using Microsoft.AspNetCore.Mvc;
using webapp_MVC.Models;
using webapp_MVC.Data;
using Microsoft.AspNetCore.Authorization;

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace webapp_MVC.Controllers
{   
    [Authorize]
    public class TaskController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TaskController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var userId = _userManager.GetUserId(User);

            var userTasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .ToListAsync();
            


            return View(userTasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem task)
        {
            System.Diagnostics.Debug.WriteLine("POST Create hit!");

            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                task.UserId = userId;
                _context.Tasks.Add(task);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            // 🔍 Dump invalid model state errors here
            foreach (var entry in ModelState)
            {
                if (entry.Value.Errors.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Key: {entry.Key}");
                    foreach (var error in entry.Value.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"  Error: {error.ErrorMessage}");
                    }
                }
            }

            return View(task); // 👈 This redisplays the form with validation errors
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
             


            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkDone(int id)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task != null)
            {
                task.IsComplete = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskItem updatedTask)
        {
            if (ModelState.IsValid)
            {
                var task = await _context.Tasks
                    .FirstOrDefaultAsync(t => t.Id == updatedTask.Id && t.UserId == _userManager.GetUserId(User));
                if (task == null)
                {
                    return NotFound();
                }

                task.Title = updatedTask.Title;
                task.Description = updatedTask.Description;
                task.IsComplete = updatedTask.IsComplete;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(updatedTask);
        }

    }
}
    

