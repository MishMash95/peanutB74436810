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
        public double getEquity(HoldemHand.Hand hero, List<Villain> villains)
        {
            return 0;
        }

        public double getPotOdds(int priceToCall, int potSize)
        {
            return 0;
        }

        public double getImpliedOdds()
        {
            return 0;
        }

        public double getCardOdds()
        {
            return 0;
        }
    }
}
