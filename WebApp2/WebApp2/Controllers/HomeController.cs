using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp2.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp2.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HelloWorldDBContext _context;
        public HomeController(HelloWorldDBContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [AllowAnonymous]
        public IActionResult Index()
        {
            var model = new HomePageViewModel();

            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            model.Employees = sqlData.GetAll();

            return View(model);
        }

        public ViewResult Detail(int id)
        {
            var model = new HomePageViewModel();
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);

            Employee employee = sqlData.Get(id);

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = new HomePageViewModel();
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);

            Employee employee = sqlData.Get(id);

            if (employee == null)
                return RedirectToAction("Index");

            return View(employee);

        }

        [HttpPost]
        public IActionResult Edit(int id,EmployeeEditViewModel input)
        {
            SQLEmployeeData sqlData = new SQLEmployeeData(_context);
            var employee = sqlData.Get(id);

            if (employee != null && ModelState.IsValid)
            {
                employee.Name = input.Name;
                _context.SaveChanges();
            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee();
                employee.Name = model.Name;

                SQLEmployeeData sqlData = new SQLEmployeeData(_context);
                sqlData.Add(employee);
                return RedirectToAction("Detail", new { id = employee.ID });
            }
            return View();
        }

    }

    public class SQLEmployeeData
    {
        private HelloWorldDBContext _context { get; set; }

        public SQLEmployeeData(HelloWorldDBContext context)
        {
            _context = context;
        }

        public void Add(Employee emp)
        {
            _context.Add(emp);
            _context.SaveChanges();
        }

        public Employee Get(int ID)
        {
            return _context.Employees.FirstOrDefault(e => e.ID == ID);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList<Employee>();
        }
    }

    public class HomePageViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }
    }

    public class EmployeeEditViewModel
    {
        [Required,MaxLength(80)]
        public string Name { get; set; }
    }
}
