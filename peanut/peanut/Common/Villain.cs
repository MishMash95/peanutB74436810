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
        public String     range { get; } /* Range of hands the bot has put him on at the current time */

        public Villain( String name, int stackSize, Position pos, State state ) {
            this.name       = name;
            this.stackSize  = stackSize;
            this.pos        = pos;
            this.state      = state;
        }

        public override string ToString() {
            return 
            "Villain:  " + this.name + "\n" +
            "Stack:    " + this.stackSize + "\n" +
            "State:    " + this.state.ToString() + "\n" +
            "Position: " + this.pos.ToString(); 
        }
    }


    
}
