using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs.OrderDiscountType;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learn.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrdersController : Controller
    {
        private IOrderService _orderService;

        public MyOrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View(_orderService.GetUserOrders(User.Identity.Name));
        }
        public IActionResult ShowOrder(int id, bool finaly = false, string type = "")
        {
            var order = _orderService.GetOrderForUserPanel(User.Identity.Name, id);

            if (order == null)
            {
                return PartialView("_NotFound");
            }

            ViewBag.finaly = finaly;
            ViewBag.typeDiscount = type;
            return View(order);
        }
        
        [AutoValidateAntiforgeryToken]
        public IActionResult RemoveOrder(int id,int orderId )
        {
            bool order = _orderService.RemoveOrder(id);
            if (order)
            {
              return Json(0);
            }
            var order1 = _orderService.GetOrderForUserPanel(User.Identity.Name, orderId);
            return PartialView("ShowOrder", order1);
        }
        public IActionResult FinalyOrder(int id)
        {
            if (_orderService.FinalyOrder(User.Identity.Name, id))
            {
                return Redirect("/UserPanel/MyOrders/ShowOrder/" + id + "?finaly=true");
            }

            return BadRequest();
        }
        
        public IActionResult UseDiscount(int orderId, string code)
        {
            DiscountUseType type = _orderService.UseDiscount(orderId, code);
            return Redirect("/UserPanel/MyOrders/ShowOrder/" + orderId + "?type=" + type.ToString());
        }
    }
}