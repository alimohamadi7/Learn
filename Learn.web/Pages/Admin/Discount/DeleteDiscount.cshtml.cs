using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Discount
{
    [PermissionChecker(22)]
    public class DeleteDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public DeleteDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [BindProperty]
        public DataLayer.Entities.Order.Discount Discount { get; set; }
        public void OnGet(int id)
        {
            Discount = _orderService.GetDiscountById(id);
        }
        public IActionResult OnPost()
        {
            _orderService.DeleteDiscount(Discount); 
            return Redirect("/Admin/Discount?Succes=DeleteOk");
        }
    }
}