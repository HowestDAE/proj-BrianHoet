using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Model;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Hero myHero = new Hero(13, "Batman")
            {
                MyNemesis = "none",
                Health = 100
            };

            Console.Write(myHero);

            Hero SuperHero = new Hero(4, "Superman", "nemesis");
            //myHero.Id = 4;
            myHero.Name = "This is a really long name, longer than 20 characters so yeet";

            Console.WriteLine(myHero.Id);
            Console.WriteLine(myHero.Name);
            //string name;
            //Console.Write("Whats your name? ");
            //name = Console.ReadLine();
            //Console.Write("Hello " + name);
            //
            //
            Console.ReadLine();
        }
    }
}
