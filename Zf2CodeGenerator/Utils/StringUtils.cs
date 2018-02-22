using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;

namespace Zf2CodeGenerator.Utils
{
    public static class StringUtils
    {

        /// <summary>
        /// 将字符串的每个单词首字母转换为大写，各单词需要以空格、逗号、下划线等符号间隔，否则当做一个单词
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Capitalize(String str)
        {
            return Strings.StrConv(str, VbStrConv.ProperCase, System.Globalization.CultureInfo.CurrentCulture.LCID);
        }

        /// <summary>
        /// 判断字符是否为不可见的空白字符，包括空格、回车、换行、制表符等
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool isNotBlank(string str)
        {
            return (str.Equals(" ") == false && str.Equals("\r") == false && str.Equals("\n") == false && str.Equals("\t") == false);
        }

    }
}
