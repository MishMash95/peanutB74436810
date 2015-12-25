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
            /*PokerBotController pbc = new PokerBotController(new PokerBotTest());
            pbc.start();*/

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

            Console.WriteLine("");
            Console.WriteLine("Testing database API...");
            Console.WriteLine("");

            //db.truncateTable("possibleActions");
            db.truncateTable("history");
            //db.truncateTable("tableNames");
            db.truncateTable("users");
            //db.truncateTable("streets");
            //db.truncateTable("positions");
            Console.WriteLine("Finished emptying tables...");
            Console.WriteLine("");
            
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
            Console.WriteLine("Start of test. handId = " + oldHandId);
            // VPIP(ALL) = 87.5%  VPIP(BTN) = 100%  PFR(ALL) = 50%   PFR(CO) = 0%
            db.insert.actions("R", oldHandId, "Christie", "BTN", "table1", "preflop", 10, 0, 1, 0, 1, 1);
            db.insert.actions("CR", oldHandId + 1, "Christie", "SB", "table1", "preflop", 10);
            db.insert.actions("XC", oldHandId + 2, "Christie", "BB", "table1", "preflop", 10);
            db.insert.actions("F", oldHandId + 3, "Christie", "UTG", "table1", "preflop", 10);
            db.insert.actions("CC", oldHandId + 4, "Christie", "MP", "table1", "preflop", 10);
            db.insert.actions("C", oldHandId + 5, "Christie", "CO", "table1", "preflop", 10);
            // Two more to make a neat number.  3B and a 4B
            db.insert.actions("CR", oldHandId + 6, "Christie", "MP", "table1", "preflop", 10);
            db.insert.actions("CRR", oldHandId + 7, "Christie", "MP", "table1", "preflop", 10);
            int newHandId = db.select.handId();

            Debug.Assert(newHandId == oldHandId + 8, "select.HandId() is not working");
            Console.WriteLine("select.handId() is working...");
            Console.WriteLine("");

            double vpip = db.select.VPIP("Christie");
            Console.WriteLine("VPIP:");
            Debug.Assert(vpip == 87.5, "Failed to get general VPIP stat");
            Console.WriteLine("Passed general VPIP...");

            double vpip_btn = db.select.VPIP("Christie", "BTN");
            Debug.Assert(vpip_btn == 100, "Failed to get positional VPIP stat");
            Console.WriteLine("Passed positional VPIP...");
            Console.WriteLine("select.VPIP() is working...");
            Console.WriteLine("");

            double pfr = db.select.PFR("Christie");
            Console.WriteLine("PFR:");
            Debug.Assert(pfr == 50, "Failed to get general PFR stat");
            Console.WriteLine("Passed general PFR...");

            double pfr_btn = db.select.PFR("Christie", "CO");
            Debug.Assert(pfr_btn == 0, "Failed to get positional PFR stat");
            Console.WriteLine("Passed positional PFR...");
            Console.WriteLine("select.PFR() is working...");
            Console.WriteLine("");

            Console.WriteLine("End of database API testing...");
            Console.WriteLine("");

            Console.ReadLine();

        }
    }
}
