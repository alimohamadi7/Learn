using Learn.Core.DTOs;
using Learn.Core.DTOs.OrderDiscountType;
using Learn.Core.Services.Interfaces;
using Learn.DataLayer.Context;
using Learn.DataLayer.Entities.Course;
using Learn.DataLayer.Entities.Order;
using Learn.DataLayer.Entities.User;
using Learn.DataLayer.Entities.Wallet;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learn.Core.Services
{
   public  class OrderService : IOrderService
    {
        private LearnContext _context;
        private IUserService _userService;

        public OrderService(LearnContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }



        public int AddOrder(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            Order order = _context.Orders
                .FirstOrDefault(o => o.UserId == userId && !o.IsFinaly);

            var course = _context.Courses.Find(courseId);

            if (order == null)
            {
                order = new Order()
                {
                    UserId = userId,
                    IsFinaly = false,
                    CreateDate = DateTime.Now,
                    OrderSum = course.CoursePrice,
                    OrderDetails = new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            CourseId = courseId,
                            Count = 1,
                            Price = course.CoursePrice
                        }
                    }
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            else
            {
                OrderDetail detail = _context.OrderDetails
                    .FirstOrDefault(d => d.OrderId == order.OrderId && d.CourseId == courseId);
                if (detail != null)
                {  // add count for same course
                    //detail.Count += 1;
                    //_context.OrderDetails.Update(detail);
                    return order.OrderId;
                }
                else
                {
                    detail = new OrderDetail()
                    {
                        OrderId = order.OrderId,
                        Count = 1,
                        CourseId = courseId,
                        Price = course.CoursePrice,
                    };
                    _context.OrderDetails.Add(detail);
                }

                _context.SaveChanges();
                UpdatePriceOrder(order.OrderId);
            }


            return order.OrderId;

        }

        public bool FinalyOrder(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            var order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null || order.IsFinaly)
            {
                return false;
            }

            if (_userService.BalanceUserWallet(userName) >= order.OrderSum)
            {
                order.IsFinaly = true;
                _userService.AddWallet(new Wallet()
                {
                    Amount = order.OrderSum,
                    CreateDate = DateTime.Now,
                    IsPay = true,
                    Description = "فاکتور شماره" + order.OrderId,
                    UserId = userId,
                    TypeId = 2
                });
                _context.Orders.Update(order);

                foreach (var detail in order.OrderDetails)
                {
                    _context.UserCourses.Add(new UserCourse()
                    {
                        CourseId = detail.CourseId,
                        UserId = userId
                    });
                }

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public OrderDetail GetOrderDetail(int id)
        {
            return _context.OrderDetails.Find(id);
        }
        public Order GetOrder(int id)
        {
            return _context.Orders.Find(id);
        }
        public bool RemoveOrder(int id)
        {
            bool returnmsg = false;
            OrderDetail orderDetail=GetOrderDetail(id);
            _context.OrderDetails.Remove(orderDetail);
            _context.SaveChanges();
            
            if (_context.OrderDetails.Any(p => p.OrderId == orderDetail.OrderId))
            {

                UpdatePriceOrder(orderDetail.OrderId);
                return returnmsg;
            }
            else
            {
                Order order1 = GetOrder(orderDetail.OrderId);
                _context.Orders.Remove(order1);
                returnmsg = true;
                _context.SaveChanges();
            }
            
            return returnmsg;
        }
        public Order GetOrderForUserPanel(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            RemoveOrderExpireDay(userId);
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.UserId == userId && o.OrderId == orderId);
        }

     

        public void UpdatePriceOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            order.OrderSum = _context.OrderDetails.Where(d => d.OrderId == orderId).Sum(d => d.Price);
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public List<Order> GetUserOrders(string userName)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            RemoveOrderExpireDay(userId);
             return _context.Orders.Where(o => o.UserId == userId).ToList();
         
        }

        public DiscountUseType UseDiscount(int orderId, string code)
        {
            var discount = _context.Discounts.SingleOrDefault(d => d.DiscountCode == code);

            if (discount == null)
                return DiscountUseType.NotFound;

            if (discount.StartDate != null && discount.StartDate > DateTime.Now)
                return DiscountUseType.ExpierDate;

            if (discount.EndDate != null && discount.EndDate <= DateTime.Now)
                return DiscountUseType.ExpierDate;


            if (discount.UsableCount != null && discount.UsableCount < 1)
                return DiscountUseType.Finished;

            var order = GetOrderById(orderId);
            if (_context.UserDiscountCodes.Any(d => d.UserId == order.UserId && d.DiscountId == discount.DiscountId))
                return DiscountUseType.UserUsed;
            int percent = (order.OrderSum * discount.DiscountPercent) / 100;
            order.OrderSum = order.OrderSum - percent;

            UpdateOrder(order);

            if (discount.UsableCount != null)
            {
                discount.UsableCount -= 1;
            }

            _context.Discounts.Update(discount);
            _context.UserDiscountCodes.Add(new UserDiscountCode()
            {
                UserId = order.UserId,
                DiscountId = discount.DiscountId
            });
            _context.SaveChanges();



            return DiscountUseType.Success;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void RemoveOrderExpireDay(int userId)
        {
            var order = _context.Orders.Where(o => o.UserId == userId & o.IsFinaly == false & o.CreateDate < DateTime.Now.AddHours(-48));
            if (order != null)
            {
                foreach (var item in order)
                {
                    var orderdetails = _context.OrderDetails.Where(o => o.OrderId == item.OrderId);

                    foreach (var orderdetail in orderdetails)
                    {
                        _context.OrderDetails.Remove(orderdetail);
                    }
                    _context.Orders.Remove(item);
                }
                _context.SaveChanges();
            }
        }

        public void AddDiscount(Discount discount)
        {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
        }

        public DiscountForAdminIndexViewModel GetDiscountForAdmin(int PageId = 1, string trim = "", string Succes = "")
        {
            IQueryable<Discount> result = _context.Discounts;
            if (!String.IsNullOrEmpty(trim))
            {
                result = result.Where(c => c.DiscountCode.Contains(trim));
            }
            //Show Item In Page
            int take = 10;
            int skip = (PageId - 1) * take;
            DiscountForAdminIndexViewModel discount = new DiscountForAdminIndexViewModel()
            {
                CurrentPage = PageId,
                PageCount = (int)Math.Ceiling(result.Count() / (double)take),
                trim = trim,
                Message = Succes,
                Discounts = result.OrderByDescending(c => c.StartDate).Skip(skip).Take(take).ToList()

            };
            return discount;
        }

        public bool IExistesDiscountcode(string Discountcode)
        {
            return _context.Discounts.Any(c => c.DiscountCode == Discountcode);
           
        }

        public Discount GetDiscountById(int discountId)
        {
            return _context.Discounts.Find(discountId);
        }

        public void UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            _context.SaveChanges();
        }

        public void DeleteDiscount(Discount discount)
        {
            Discount discount1 = GetDiscountById(discount.DiscountId);
            _context.Discounts.Remove(discount1);
            _context.SaveChanges();
        }

        public bool IsUserInCourse(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            return _context.UserCourses.Any(c => c.UserId == userId && c.CourseId == courseId);

        }

    
    }


    
}
