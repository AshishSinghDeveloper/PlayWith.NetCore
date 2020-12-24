using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
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
        public string Index()
        {
           return  _employeeRepository.GetEmployeeById(1).Name;
        }

        public ViewResult Details()
        {
            Employee model = _employeeRepository.GetEmployeeById(1);
            return View(model);
            return View(); // Since it has not parameter it look for Details.cstml in Home controller
            return View("Test"); // This will look for Test.cshtml in View/Home folder.
            return View("../../NewFolder/newview.cshtml") // This is relative path. It was in the home folder. first it came out from home folder, then view folder which means it come to root folder. From root folder it went to New folder and then looked for newview.cshtml file.
        }
    }
}
