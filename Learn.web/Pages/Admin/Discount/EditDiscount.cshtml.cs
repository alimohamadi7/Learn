using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DNTPersianUtils.Core;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Discount
{
    [PermissionChecker(21)]
    public class EditDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public EditDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [BindProperty]
        public DataLayer.Entities.Order.Discount Discount { get; set; }

        public void OnGet(int id)
        {
            Discount = _orderService.GetDiscountById(id);
        }
        public IActionResult OnPost(string stDate = "",string LastDiscountcode="", string edDate = "")
        {
            

            if (Discount.DiscountCode != LastDiscountcode & _orderService.IExistesDiscountcode(Discount.DiscountCode))
            {
                ModelState.AddModelError("Discount.DiscountCode", " این کد قبلا ثبت شده است ");
                return Page();

            }
            if (stDate != "")
            {
                Discount.StartDate = stDate.ToGregorianDateTime();
            }

            if (edDate != "")
            {
                Discount.EndDate = edDate.ToGregorianDateTime();
            }

            if (!ModelState.IsValid)
                return Page();

            _orderService.UpdateDiscount(Discount);

            return Redirect("/Admin/Discount?Succes=EditOk");

        }

    }
}