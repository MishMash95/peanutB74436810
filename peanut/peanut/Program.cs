using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoldemHand;

namespace peanut
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test outs function
            string pocket = "As Ac";
            string board = "Kd 8h 9c";
            // Calcuate the outs
            ulong outsmask =
                Outs(Hand.ParseHand(pocket), Hand.ParseHand(board));

            System.Diagnostics.Debug.WriteLine("[{0}] {1} : Outs Count {2}",
                    pocket, board, Hand.BitCount(outsmask));

            // List the cards
            foreach (string card in Hand.Cards(outsmask))
            {
                System.Diagnostics.Debug.WriteLine("{0} ", card);
            }


            string dbName = "test_" + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + ".sqlite";
            var db = new Database(dbName);

            // Begin testing

            //db.truncateTable("playerActions");
            //db.truncateTable("history_preflop");
            //db.truncateTable("history_flop");
            //db.truncateTable("history_turn");
            //db.truncateTable("history_river");
            //db.truncateTable("tableNames");
            //db.truncateTable("users");
            //db.truncateTable("positions");

            string[] usernames = new string[30] { "Christie", "Vernon", "Keisha", "Tonia", "Leopoldo", "Asuncion", "Pattie", "Mimi", "Alessandra", "Genesis", "Latoya", "Winfred", "Bennie", "Les", "Jerri", "Ashlea", "Faustino", "Corey", "Tonja", "Diann", "Spring", "Coral", "Dominque", "Olene", "Ileen", "Barbar", "Rachell", "Brice", "Shizuko", "Sondra" };
            string[] positions = new string[9] { "BTN", "SB", "BB", "UTG", "UTG+1", "MP1", "MP2", "HJ", "CO" };
            string[] tables = new string[4] { "table1", "table2", "table3", "table4" };
            db.insertTable(tables[0]);
            db.insertTable(tables[1]);
            db.insertTable(tables[2]);
            db.insertTable(tables[3]);
            for (int i = 0; i < 30; i++){
                db.insertPlayer(usernames[i]);  
            }

            int handId = db.getHandId();
            // These stats should have a VPIP = 3/9=33%, PFR = 3/9=33%
            db.insertPreFlopActions("R", handId, "Christie", "BTN", "table1");
            db.insertPreFlopActions("CR", handId+1, "Christie", "SB", "table1");
            db.insertPreFlopActions("XC", handId+2, "Christie", "BB", "table1");
            db.insertPreFlopActions("F", handId+3, "Christie", "UTG", "table1");
            db.insertPreFlopActions("F", handId+4, "Christie", "UTG+1", "table1");
            db.insertPreFlopActions("CC", handId+5, "Christie", "MP1", "table1");
            db.insertPreFlopActions("F", handId+6, "Christie", "MP2", "table1");
            db.insertPreFlopActions("R", handId+7, "Christie", "HJ", "table1");
            db.insertPreFlopActions("C", handId+8, "Christie", "CO", "table1");

            int vpip = db.getVPIP("Christie");
            int pfr = db.getPFR("Christie");
        }

        // Return a hand mask of the cards that improve our hand
        static ulong Outs(ulong pocket, ulong board)
        {
            ulong retval = 0UL;
            ulong hand = pocket + board;

            // Get original hand value
            uint playerOrigHandVal = Hand.Evaluate(hand);

            // Look ahead one card
            foreach (ulong card in Hand.Hands(0UL, hand, 1))
            {
                // Get new hand value
                uint playerNewHandVal = Hand.Evaluate(hand + card);

                // If the hand improved then we have an out
                if (playerNewHandVal > playerOrigHandVal)
                {
                    // Add card to outs mask
                    retval = card;
                }
            }

            // return outs as a hand mask
            return retval;
        }

    }
}
