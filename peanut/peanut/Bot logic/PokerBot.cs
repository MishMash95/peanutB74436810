using peanut.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peanut.Bot_logic {

    /*
        Actual Bot decision making logic is separated out. Any custom bot can extend PokerBot to 
        implement custom behaviour at each stage.

        Each stage may be executed multiple times if multiple rounds of betting occur. In each case,
        the hand will have history of all rounds of action, but it will mostly be based on the final round of action.

        The idea here is to abstract away the logic of the bot, from the control of the bot.
    */
    abstract class PokerBot {
        public string profileName { get; internal set; } = "Default";

        public Common.Action onPreFlop(Hand hand) {
            return new ActionFold();
        }

        public Common.Action onFlop(Hand hand) {
            return new ActionFold();
        }

        public Common.Action onTurn(Hand hand) {
            return new ActionFold();
        }

        public Common.Action onRiver(Hand hand) {
            return new ActionFold();
        }
    }

    // ~TEMP~
    class PokerBotRandom : PokerBot {
        public PokerBotRandom() {
            profileName = "RandomBot";
        }
    }
}
