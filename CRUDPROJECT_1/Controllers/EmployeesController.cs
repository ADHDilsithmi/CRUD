using CRUDPROJECT_1.Data;
using CRUDPROJECT_1.Models;
using CRUDPROJECT_1.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDPROJECT_1.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly Database database;

        public EmployeesController(Database database) 
        {
            this.database = database;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await database.Employees.ToListAsync();


            return View(employees) ;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("AddView");
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeModel addEmployeeRequest)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Id = Guid.NewGuid(),
                    Name = addEmployeeRequest.Name,
                    Email = addEmployeeRequest.Email,
                    Salary = addEmployeeRequest.Salary,
                    DateOfBirth = addEmployeeRequest.DateOfBirth,
                    Department = addEmployeeRequest.Department

                };

                await database.Employees.AddAsync(employee);
                await database.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Add");
            

        }

         [HttpGet]
         public async Task<IActionResult> View(Guid id)
        {
            var employee = await database.Employees.FirstOrDefaultAsync(x => x.Id  == id);

            if(employee != null)
            {
                var viewModel = new UpdateEmployeeModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    DateOfBirth = employee.DateOfBirth,
                    Department = employee.Department
                };
                return await Task.Run(()=> View("View", viewModel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = await database.Employees.FindAsync(model.Id);
                if (employee != null)
                {
                    employee.Name = model.Name;
                    employee.Email = model.Email;
                    employee.Salary = model.Salary;
                    employee.DateOfBirth = model.DateOfBirth;
                    employee.Department = model.Department;

                    await database.SaveChangesAsync();

                    return RedirectToAction("Index");

                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeModel model)
        {
            var employee = await database.Employees.FindAsync(model.Id);

            if(employee != null)
            {
                database.Employees.Remove(employee);
                await database.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
