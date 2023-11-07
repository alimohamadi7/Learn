using System;
using System.Collections.Generic;
using System.Text;

namespace Learn.Core.Generator
{
   public class GenerateUniq
    {
        public static string GenerateUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}
