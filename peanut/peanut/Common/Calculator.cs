using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoldemHand;

namespace peanut.Common
{
    /*
        Abstraction of the HoldemHand API
    */
    class Calculator
    {
        public double getEquity(HoldemHand.Hand hero, List<Villain> villains) { 
            return 0;
        }

        public double getPotOdds(int priceToCall, int potSize) { 
            // potSize must be the total size of the pot which includes the raise made by the villain
            return (priceToCall / potSize) * 100;
        }

        public double getImpliedOdds() {
            return 0;
        }

        public double getCardOdds() {
            /* Hit odds are not calculated, they are instead searched in the hitOdds table */
            Console.WriteLine("Card odds are accessible in the database class");
            return 0;
        }
    }
}
