using MyMvcApp.Repositories;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly EmployeeRepository _repository;

    public HomeController(EmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Employee()
    {
        var data = await _repository.GetEmployees(); // 🔥 CALL SP
        return View(data);
    }
}