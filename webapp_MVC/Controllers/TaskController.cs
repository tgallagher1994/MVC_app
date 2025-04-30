using Microsoft.AspNetCore.Mvc;
using webapp_MVC.Models;
using System.Collections.Generic;

namespace webapp_MVC.Controllers
{
    public class TaskController : Controller
    {
        private static List<TaskItem> tasks = new List<TaskItem>();

        public IActionResult Index()
        {
            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskItem task)
        {
            if (ModelState.IsValid)
            {
                task.Id = tasks.Count + 1;
                tasks.Add(task);
                return RedirectToAction(nameof(Index));


            }

            return View(task);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                tasks.Remove(task);


            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult MarkDone(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                task.IsComplete = true;
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var task = tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(TaskItem updatedTask)
        {
            var task = tasks.FirstOrDefault(t => t.Id == updatedTask.Id);
            if (task != null && ModelState.IsValid)
            {
                task.Title = updatedTask.Title;
                task.Description = updatedTask.Description;
                task.IsComplete = updatedTask.IsComplete;
                return RedirectToAction(nameof(Index));
            }

            return View(updatedTask);
        }

    }
}
    

