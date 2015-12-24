using peanut.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peanut.Bot_logic {
    /*
        This is the main class dedicated to a logical bot. It is
        responsible for invoking reading, performing logic, fetching and storing database results.

        All other utility classes should make it as easy as possible for the bot to work using simplified
        functions.

        In the MVC model, this represents the Control component.

        ----------------------------------------------------------------------------------------------------------------
        IMPORTANT: This is the control back-end of the bot, a main instanciation of the overall system of control a bot
        may operate under. If you want to modify bot logic, this should be done by creating/modifying a logic bot:
        "PokerBot" (Abstract class representing simple behaviour of a bot)
        ----------------------------------------------------------------------------------------------------------------
        Each bot needs its own BotController, and ultimately they should be able to run on independent threads.
    */

    class PokerBotController {

        private PokerBot bot;

        public PokerBotController( PokerBot bot ) {
            this.bot = bot;
        }

        
    }


    



}
