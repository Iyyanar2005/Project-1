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
    }
}