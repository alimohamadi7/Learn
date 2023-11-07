using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Learn.web.Controllers
{
    
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpstatusCodeHanhler(int statuscode)
        {
            switch (statuscode)
            {
                case 404:

                    return PartialView("_NotFound");
                   
            }
            return PartialView("_NotFound");
        }
    }
}