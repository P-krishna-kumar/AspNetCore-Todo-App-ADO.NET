using Microsoft.AspNetCore.Mvc;  // This is required
using TodoApp.ADO.NET_Repository; 
using TodoApp.Models;

namespace TodoApp.Controllers
{
    public class TodoController : Controller  // <- Must inherit from Controller
    {
        private readonly TodoRepository _repository;

        public TodoController(TodoRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var todos = _repository.GetAll();
            return View(todos);  // <- This works only if you inherit Controller
        }

        [HttpPost]
        public IActionResult Add(string task)
        {
            if (!string.IsNullOrWhiteSpace(task))
            {
                _repository.Add(new Todo { Task = task, IsCompleted = false });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateStatus(int id, bool isCompleted)
        {
            _repository.UpdateStatus(id, isCompleted);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
