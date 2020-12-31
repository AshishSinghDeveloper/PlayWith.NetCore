using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        public ViewResult Details(int? id)
        {
            //implement ViewData
            //Employee model = _employeeRepository.GetEmployeeById(1);
            //ViewData["Employee"] = model;
            //ViewData["Title"] = "Employee Details";
            //return View(model);

            //implement ViewBag
            //ViewBag.PageTitle = 
            //Employee model = _employeeRepository.GetEmployeeById(1);"Employee Details";
            //ViewBag.Employee = model;
            //return View(model);

            //implement strongly type model
            //Employee model = _employeeRepository.GetEmployeeById(1);
            //return View(model);

            //implement View Model: We create a "View Model" when a Model object does not contain all the data a view needs
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployeeById(id??1), //id??! means if id is not null then use id = 1
                PageTitle = "Employee Details",
            };
            return View(homeDetailsViewModel);

            //return View(); // Since it has not parameter it look for Details.cstml in Home controller
            //return View("Test"); // This will look for Test.cshtml in View/Home folder.
            //return View("../../NewFolder/newview.cshtml") // This is relative path. It was in the home folder. first it came out from home folder, then view folder which means it come to root folder. From root folder it went to New folder and then looked for newview.cshtml file.
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public RedirectToActionResult Create(Employee employee)
        {
            Employee newEmployee = _employeeRepository.AddEmployee(employee);
            return RedirectToAction("details", new { id = newEmployee.Id });
        }
    }
}
