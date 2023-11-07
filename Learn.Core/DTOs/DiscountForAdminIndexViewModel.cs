using Learn.DataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.DTOs
{
  public  class DiscountForAdminIndexViewModel
    {   
        public  List<Discount> Discounts { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public string trim { get; set; }
        //for message in view
        public string Message { get; set; }
    }
}
