using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Security;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Learn.web.Pages.Admin.Discount
{
    [PermissionChecker(19)]
    public class IndexModel : PageModel
    {
        private IOrderService _orderService;

        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public DiscountForAdminIndexViewModel discountForAdminIndexViewModel { get; set; }
        public void OnGet(int PageId = 1, string trim = "", string Succes = "")
        {
            discountForAdminIndexViewModel = _orderService.GetDiscountForAdmin(PageId, trim, Succes);
            if (PageId-1 > discountForAdminIndexViewModel.PageCount)
            {
                ViewData["NotPage"] = "این صفحه وجود ندارد";
            }
        }
    }
}