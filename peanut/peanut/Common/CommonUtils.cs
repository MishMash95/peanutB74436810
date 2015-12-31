using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peanut.Common {
    /*
        Standardised enums / classes should be contained within the peanut.Common namespace.

        These are elements that should be used throughout the project
    */
    public enum State { ACTIVE, FOLDED, SITTING_OUT, EMPTY_SEAT }
    public enum Position { B = 0, SB = 1, BB = 2, UTG = 3, MP = 4, CO = 5  }
}
