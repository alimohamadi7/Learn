using Learn.Core.DTOs;
using Learn.Core.DTOs.OrderDiscountType;
using Learn.DataLayer.Entities.Order;
using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.Services.Interfaces
{
  public  interface IOrderService
    {
        int AddOrder(string userName, int courseId);

        void UpdatePriceOrder(int orderId);
        Order GetOrderForUserPanel(string userName, int orderId);
        Order GetOrderById(int orderId);
        bool FinalyOrder(string userName, int orderId);
        OrderDetail GetOrderDetail(int id);
        Order GetOrder(int id);
        bool RemoveOrder(int id);
        void RemoveOrderExpireDay(int userId);
        List<Order> GetUserOrders(string userName);
        
        void UpdateOrder(Order order);
        bool IsUserInCourse(string userName, int courseId);
        #region Discount
        DiscountUseType UseDiscount(int orderId, string code);
        void AddDiscount(Discount discount);
        DiscountForAdminIndexViewModel GetDiscountForAdmin(int PageId = 1, string trim = "", string Succes = "");
        bool IExistesDiscountcode(string Discountcode);
        Discount GetDiscountById(int discountId);
         void UpdateDiscount(Discount discount);
        void DeleteDiscount(Discount discount);
        #endregion

    }
}
