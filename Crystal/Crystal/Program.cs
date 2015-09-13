using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Crystal
{
    class Program
    {
        static void Main(string[] args)
        {
            
            DFA a = new DFA();



            string inputstr = System.IO.File.ReadAllText(@"C:\Users\Mir\Desktop\input.txt");
            try
            {
                //Console.WriteLine();
                Console.WriteLine(a.compile(inputstr));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
