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
    public enum Position { SB, BB, UTG, MP, CO, B }
}
