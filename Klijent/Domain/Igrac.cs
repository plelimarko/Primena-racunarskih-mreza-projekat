using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Igrac
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int[] niz { get; set; }

        public Igrac(int id, string name, int[] niz)
        {
            ID = id;
            Name = name;
            this.niz = niz;
        }

    }
}
