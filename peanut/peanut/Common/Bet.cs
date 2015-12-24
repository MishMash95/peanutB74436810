using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peanut.Common {

    /*
        A 'Bet' is the container class for any kind of bet, it contains information such as amount, and villain who made the bet.
    */
    class Bet {
        private int betAmount;
        private Villain aggressor;

        public Bet( int betAmt, Villain villain ) {
            betAmount = betAmt;
            aggressor = villain;
        }

        public int     getBetAmount() { return betAmount;  }
        public Villain getVillain() { return aggressor; }
    }
}
