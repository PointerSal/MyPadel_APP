using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPadelApp.Helpers
{
    public static class ImageToBase64
    {
        public static string BytesToBase64(byte[] byteArray)
        {
            try
            {
                if (byteArray == null || byteArray.Length == 0)
                    return string.Empty;

                return Convert.ToBase64String(byteArray);
            }
            catch { }
            return string.Empty;
        }
    }
}
