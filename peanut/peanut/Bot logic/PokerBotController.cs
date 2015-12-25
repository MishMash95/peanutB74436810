using peanut.Common;
using peanut.Reader;
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
        private TableReader tableReader;
        private IntPtr windowHandle;

        public PokerBotController( PokerBot bot, IntPtr windowHandle ) {
            this.bot          = bot;
            this.windowHandle = windowHandle;
            tableReader       = new TableReader(windowHandle);
        }

        /*
            Start the bot logic.
            Sets up any necessary pre-conditions that the controller requires to run 
            and then instantiates the main logic loop.
        */
        public void begin() {


            while( true) {
                step();
                System.Threading.Thread.Sleep(2000);
            }
        }

        /*
            Perform interval repeated actions. 
            This is the loop process function which is called.
        */
        public void step() {

            // Update table reader to current state:
            tableReader.readImage();

            // Get Pocket cards
            // ...

            // Get Community Cards
            Console.WriteLine("\n\n --- Retrieving Community Cards --- ");
            Card[] communityCards = tableReader.getCommunityCards();
            foreach( Card c in communityCards) {
                Console.Write(c.ToString() + " ");
            }
            Console.WriteLine();
        }

        
    }


    



}
