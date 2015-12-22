using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoldemHand;
using Model;

namespace peanut
{
    class Program
    {
        static void Main(string[] args)
        {
            // Entry point for program

            string dbName = "test_" + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + ".sqlite";
            var db = new Database(dbName);

        }
    }
}
