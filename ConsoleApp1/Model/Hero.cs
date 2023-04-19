using ConsoleApp1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model
{
    public class Hero
    {
        public int Id { get; private set; }

        private string _name;

        public string MyNemesis { get; set; }

        public int Health { get; set; }

        public string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_name))
                {
                    return "Ananymous";
                }
                return _name;
            }
            set
            {
                if (value != null && value.Length > 20)
                {
                    _name = value.Substring(0, 20);
                }
                else
                {
                    _name = value;
                }
            }
        }
        public Hero(int id, string name)
            : this(id, name, "no nemesis")
        {

        }

        public Hero(int id, string name, string nemesis)
        {
            Id = id;
            Name = name;
            MyNemesis = nemesis;
        }

        private void DoInternalStuff()
        {

        }
        public void Draw()
        {

        }

        public override string ToString()
        {
            return $"* {Name} (Health: {Health}, nemesis: {MyNemesis})";
        }
    }

}

