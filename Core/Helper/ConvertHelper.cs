using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class ConvertHelper
    {
        public static int ConvertCode(this string[] code)
        {
			try
			{
                return Convert.ToInt32(string.Join("", code));
			}
			catch (Exception)
			{
                return 0;
			}
        }
    }
}
