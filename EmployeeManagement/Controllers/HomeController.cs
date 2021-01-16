using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment webHostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment  webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.webHostingEnvironment = webHostEnvironment;
        }
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        public ViewResult Details(int? id)
        {
            #region Example of ViewData, ViewBag, and Strongly type Model
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
            #endregion
            var employee = _employeeRepository.GetEmployeeById(id.Value);
            if(employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = employee,
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
        public IActionResult Create(HomeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        private string ProcessUploadedFile(HomeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null && model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    //webHostingEnvironment.WebRootPath give the exact path of wwwroot folder
                    string uploadsFolder = Path.Combine(webHostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    using(var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        photo.CopyTo(fileStream);
                    }
                }
            }
            return uniqueFileName;
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployeeById(id);
            HomeEditViewModel homeEditViewModel = new HomeEditViewModel
            {
                Id = employee.Id,
                Department = employee.Department,
                Email = employee.Email,
                Name = employee.Name,
                ExistingPhotoPath = employee.PhotoPath,
            };
            return View(homeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(HomeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployeeById(model.Id);
                employee.Department = model.Department;
                employee.Email = model.Email;
                employee.Name = model.Name;
                if(model.Photos != null)
                {
                    if(model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(webHostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    employee.PhotoPath = ProcessUploadedFile(model);
                }
                
                _employeeRepository.Update(employee);
                return RedirectToAction("Index");
            }
            return View();
        }

        
    }
}
