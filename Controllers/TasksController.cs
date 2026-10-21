using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoListApp.Data;
using TodoListApp.Models;

namespace TodoListApp.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Список задач
        public async Task<IActionResult> Index(string? status, string? sortOrder)
        {
            var tasks = _context.TaskItems.AsQueryable();

            if (status == "completed")
            {
                tasks = tasks.Where(t => t.IsCompleted);
            }
            else if (status == "active")
            {
                tasks = tasks.Where(t => !t.IsCompleted);
            }

            tasks = sortOrder == "oldest"
                ? tasks.OrderBy(t => t.CreatedAt)
                : tasks.OrderByDescending(t => t.CreatedAt);

            ViewBag.Status = status;
            ViewBag.SortOrder = sortOrder;

            ViewBag.UncompletedCount =
                await _context.TaskItems.CountAsync(t => !t.IsCompleted);

            return View(await tasks.ToListAsync());
        }

        // Форма добавления
        public IActionResult Create()
        {
            return View();
        }

        // Сохранение новой задачи
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem taskItem)
        {
            if (ModelState.IsValid)
            {
                taskItem.Id = 0;
                taskItem.CreatedAt = DateTime.Now;

                _context.TaskItems.Add(taskItem);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(taskItem);
        }

        // Форма редактирования
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // Сохранение изменений
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Title,Description,IsCompleted")] TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingTask = await _context.TaskItems.FindAsync(id);

                if (existingTask == null)
                {
                    return NotFound();
                }

                existingTask.Title = taskItem.Title;
                existingTask.Description = taskItem.Description;
                existingTask.IsCompleted = taskItem.IsCompleted;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(taskItem);
        }

        // Подтверждение удаления
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id);

            if (taskItem == null)
            {
                return NotFound();
            }

            return View(taskItem);
        }

        // Удаление задачи
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Отметка о выполнении
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);

            if (taskItem == null)
            {
                return NotFound();
            }

            taskItem.IsCompleted = !taskItem.IsCompleted;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}