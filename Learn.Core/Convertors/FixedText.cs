using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.Convertors
{
  public static class FixedText
    {
        public static string FixEmail(string email)
        {
            return email.Trim().ToLower();
        }
    }
}
