﻿using System;
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
        }
    }
}
