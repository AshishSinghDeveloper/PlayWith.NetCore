﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry, the page could not be found";
                    logger.LogWarning($"404 Error Occured. Path {statusCodeResult.OriginalPath} and Query String {statusCodeResult.OriginalQueryString}");
                    break;
            }
            return View("NotFound");
        }
        
        [Route("/Error")]
        [AllowAnonymous]
        public IActionResult ErrorHandler()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            //ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            //ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            //ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;

            logger.LogError($"The path {exceptionHandlerPathFeature.Path} threw an exception {exceptionHandlerPathFeature.Error}");

            return View("Error");
        }
    }
}
