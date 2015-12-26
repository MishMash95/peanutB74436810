using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoldemHand;

namespace peanut.Common {


    /*
        Returns the state of a villain at a given capture moment.
    */
    class Villain {
        private int      stackSize { get; }
        private String   name { get; }
        private Position pos { get; }
        private State    state { get; }
        private Hand     handMask { get; }
    }


    
}
