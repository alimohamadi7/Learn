using System;
using System.Globalization;
using DNTPersianUtils.Core;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Discount
{
    [PermissionChecker(20)]
    public class CreateDiscountModel : PageModel
    {
        private IOrderService _orderService;

        public CreateDiscountModel(IOrderService orderService)
        {
            _orderService = orderService;
        }



        [BindProperty]
        public DataLayer.Entities.Order.Discount Discount { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPost(string stDate = "", string edDate = "")
        {
            if (_orderService.IExistesDiscountcode(Discount.DiscountCode))
            {
                ModelState.AddModelError("Discount.DiscountCode", " این کد قبلا ثبت شده است ");
                return Page();
                
            }
            if (stDate != "")
            {
                Discount.StartDate= stDate.ToGregorianDateTime();
                //string[] std = stDate.Split('/');
                //Discount.StartDate = new DateTime(int.Parse(std[0]),
                //    int.Parse(std[1]),
                //    int.Parse(std[2]),
                //    new PersianCalendar()
                //    );
            }

            if (edDate != "")
            {
                Discount.EndDate = edDate.ToGregorianDateTime();
                //string[] edd = edDate.Split('/');
                //Discount.EndDate = new DateTime(int.Parse(edd[0]),
                //    int.Parse(edd[1]),
                //    int.Parse(edd[2]),
                //    new PersianCalendar()
                //);
            }

            if (!ModelState.IsValid)
                return Page();

            _orderService.AddDiscount(Discount);

            return Redirect("/Admin/Discount?Succes=CreateOk");

        }
    }
}