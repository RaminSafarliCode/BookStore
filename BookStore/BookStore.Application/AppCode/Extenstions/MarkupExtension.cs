using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BookStore.Application.AppCode.Extenstions
{
    public static partial class Extension
    {
        static public string ToPlainText(this string text)
        {
            text = Regex.Replace(text, "<[^>]*>", "");
            return text;
        }
    }
}
