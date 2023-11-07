using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Learn.Core.DTOs;
using Learn.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Learn.web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private IUserService _userService;
        public WalletController(IUserService userService)
        {
            _userService = userService;
        }
        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ChargeWalletViewModel charge = new ChargeWalletViewModel();
            charge.walletViewModels = _userService.GetWalletUser(User.Identity.Name);

            return View(charge);
        }
        [Route("UserPanel/Wallet")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            { 
                charge.walletViewModels = _userService.GetWalletUser(User.Identity.Name);
            return View(charge);
            }
             var walletId=_userService.ChargeWallet(User.Identity.Name, charge.Amount, "شارژ حساب");

            #region Onlin Payment
            var payment = new ZarinpalSandbox.Payment(charge.Amount);
            var res = payment.PaymentRequest("شارژ کیف پول", "http://www.toplearnfile.ir/OnlinePayment/" + walletId);
            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/"+res.Result.Authority);
            }    
            #endregion
            return null;
        }
    }
}