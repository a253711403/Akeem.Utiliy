using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akeem.Utiliy;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string md5 = "abc".ToMD5();
            Console.WriteLine(md5);
        }
    }
}
