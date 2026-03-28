using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Repositories;

namespace MyMvcApp.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _repository;

        public EmployeeController(EmployeeRepository repository)
             {
                 _repository = repository;
             }

        // 🔹 Get All
        public async Task<IActionResult> Index()
        {
            var data = await _repository.GetEmployees();
            return View(data);
        }

        // 🔹 Search by Name
       [HttpPost]
       public async Task<IActionResult> Search(string Name)
       {
           if(string.IsNullOrEmpty(Name))
               Console.WriteLine("Search parameter is empty!");
           else
               Console.WriteLine($"Searching for: {Name}");
       
           var data = await _repository.GetEmployeesByName(Name);
           return View("Index", data);
       }

       // 🔹 AJAX: Get employee by ID
[HttpGet]
public async Task<IActionResult> GetById(int id)
{
    var emp = await _repository.GetEmployeeByID(id);
    if (emp == null) return NotFound();
    return Json(emp);
}

// 🔹 AJAX: Save (insert or update)
[HttpPost]
public async Task<IActionResult> Save([FromBody] MyMvcApp.Models.Employee emp)
{
    var result = await _repository.SaveEmployee(emp);
    // If result is numeric → success; else it's an error message
    if (int.TryParse(result, out int newId))
        return Json(new { success = true, empID = newId });
    else
        return Json(new { success = false, message = result });
}
    }
}