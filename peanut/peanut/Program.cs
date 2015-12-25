using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HoldemHand;
using System.Drawing;
using peanut.Bot_logic;

using peanut.Database;
using System.Diagnostics;

namespace peanut
{
    class Program
    {
        static void Main(string[] args)
        {
            // Test spawning off a bot controller
            PokerBotController pbc = new PokerBotController(new PokerBotRandom(), new IntPtr(0));
            pbc.begin();

            // Entry point for program

            string dbName = "test_" + string.Format("{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + ".sqlite";
            var db = new Database.Database(dbName);

            /*var cd = new CardDetection();
            Card[] cards = cd.RetieveCommunityCards();
            foreach (Card c in cards)
            {
                Console.WriteLine("RANK:" + c.rank);
                Console.WriteLine("SUIT:" + c.suit);
                Console.WriteLine("NEXT CARD...");
            }*/


            // Begin testing

            db.truncateTable("possibleActions");
            db.truncateTable("history_preflop");
            db.truncateTable("history_flop");
            db.truncateTable("history_turn");
            db.truncateTable("history_river");
            db.truncateTable("tableNames");
            db.truncateTable("users");
            db.truncateTable("positions");
            Console.WriteLine("Finished emptying tables...");
            Console.WriteLine("\n");
            
            string[] usernames = new string[30] { "Christie", "Vernon", "Keisha", "Tonia", "Leopoldo", "Asuncion", "Pattie", "Mimi", "Alessandra", "Genesis", "Latoya", "Winfred", "Bennie", "Les", "Jerri", "Ashlea", "Faustino", "Corey", "Tonja", "Diann", "Spring", "Coral", "Dominque", "Olene", "Ileen", "Barbar", "Rachell", "Brice", "Shizuko", "Sondra" };
            string[] positions = new string[9] { "BTN", "SB", "BB", "UTG", "UTG+1", "MP1", "MP2", "HJ", "CO" };
            string[] tables = new string[4] { "table1", "table2", "table3", "table4" };
            db.insert.table(tables[0]);
            db.insert.table(tables[1]);
            db.insert.table(tables[2]);
            db.insert.table(tables[3]);
            for (int i = 0; i < 30; i++)
            {
                db.insert.player(usernames[i]);
            }

            int oldHandId = db.select.handId();
            // VPIP(ALL) = 87.5%  VPIP(BTN) = 100%  PFR(ALL) = 50%   PFR(CO) = 0%
            db.insert.preFlopActions("R", oldHandId, "Christie", "BTN", "table1");
            db.insert.preFlopActions("CR", oldHandId + 1, "Christie", "SB", "table1");
            db.insert.preFlopActions("XC", oldHandId + 2, "Christie", "BB", "table1");
            db.insert.preFlopActions("F", oldHandId + 3, "Christie", "UTG", "table1");
            db.insert.preFlopActions("CC", oldHandId + 4, "Christie", "MP", "table1");
            db.insert.preFlopActions("C", oldHandId + 5, "Christie", "CO", "table1");
            // Two more to make a neat number  3B and a 4B
            db.insert.preFlopActions("CR", oldHandId + 6, "Christie", "MP", "table1");
            db.insert.preFlopActions("CRR", oldHandId + 7, "Christie", "MP", "table1");
            int newHandId = db.select.handId();

            Debug.Assert(newHandId == oldHandId + 5, "HandId is failing to increment");
            Console.WriteLine("HandId is successfully incrementing");

            int vpip = db.select.VPIP("Christie");
            Console.WriteLine("Testing database API...");
            Console.WriteLine("VPIP:");
            Debug.Assert(vpip == 88, "Failed to get positional VPIP stat");

            int vpip_btn = db.select.VPIP("Christie", "BTN");
            Debug.Assert(vpip == 100, "Failed to get general VPIP stat");
            Console.WriteLine("Passed VPIP...");
            Console.WriteLine("\n");

            
            Console.ReadLine();

        }
    }
}
