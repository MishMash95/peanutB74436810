﻿using System;
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

            Console.WriteLine("");
            Console.WriteLine("Testing database API...");
            Console.WriteLine("");

            /*
                pre-populated tables should not be truncated/emptied as the queries rely on them. 
                They include: possibleActions, streets, positions
            */
            db.truncateTable("history");
            db.truncateTable("users");
            Console.WriteLine("test.allTablesExist() is working...");
            Console.WriteLine("test.tableExists() is working...");
            // Need to add test to see if tables are in fact empty
            Console.WriteLine("truncateTable() is working...");
            Console.WriteLine("");
            
            string[] usernames = new string[6] { "peanut", "villain1", "villain2", "villain3", "villain4", "villain5" };
            string[] positions = new string[6] { "BTN", "SB", "BB", "UTG", "MP", "CO" };
            string[] tables = new string[4] { "table1", "table2", "table3", "table4" };
            db.insert.table(tables[0]);
            db.insert.table(tables[1]);
            db.insert.table(tables[2]);
            db.insert.table(tables[3]);
            for (int i = 0; i < 6; i++)
            {
                db.insert.player(usernames[i]);
            }

            int handId = db.select.handId();
            Console.WriteLine("Start of test. handId = " + handId);
            int potSize;

            /*
                Scenario 1 (Stealing blinds):
                folds around to BTN who raises to steal blinds (6p)
            */
            potSize = 6 + 1 + 2;
            db.insert.actions("F", handId, "villain3", "UTG", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain4", "MP", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain5", "CO", "table1", "preflop", potSize);

            db.insert.setFlag(new string[] { "opened", "aggressor", "win" });
            db.insert.actions("R", handId, "peanut", "BTN", "table1", "preflop", potSize);
            db.insert.raises(db.select.lastHistoryId(), 4); /* Raised by 4p (3x) */

            db.insert.actions("F", handId, "villain1", "SB", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain2", "BB", "table1", "preflop", potSize);

            
            /*
                Scenario 2 (4bet):
                UTG raises (6p), SB 3bets (18p) and UTG 4bets (54), SB folds
            */
            potSize = 18 + 54 + 2;
            handId++;

            db.insert.setFlag(new string[] { "opened", "aggressor", "win", "4bet" });
            db.insert.actions("RR", handId, "villain2", "UTG", "table1", "preflop", potSize);
            db.insert.raises(db.select.lastHistoryId(), 4, 36);

            db.insert.actions("F", handId, "villain3", "MP", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain4", "CO", "table1", "preflop", potSize);
            db.insert.actions("R", handId, "villain5", "BTN", "table1", "preflop", potSize);

            db.insert.setFlag(new string[] { "3bets" });
            db.insert.actions("RF", handId, "peanut", "SB", "table1", "preflop", potSize);
            db.insert.raises(db.select.lastHistoryId(), 12);

            db.insert.actions("F", handId, "villain1", "BB", "table1", "preflop", potSize);

            /*
                Scenario 3 (Squeeze play):
                UTG limps, MP limps, CO limps, SB limps, peanut squeezes (14p), all fold
            */
            potSize = 14 + 2 + 2 + 2 + 2;
            handId++;
            db.insert.setFlag(new string[] { "limp" });
            db.insert.actions("CF", handId, "villain1", "UTG", "table1", "preflop", potSize);

            db.insert.setFlag(new string[] { "limp" });
            db.insert.actions("CF", handId, "villain2", "MP", "table1", "preflop", potSize);

            db.insert.setFlag(new string[] { "limp" });
            db.insert.actions("CF", handId, "villain3", "CO", "table1", "preflop", potSize);

            db.insert.actions("F", handId, "villain4", "BTN", "table1", "preflop", potSize);

            db.insert.setFlag(new string[] { "limp" });
            db.insert.actions("CF", handId, "villain5", "SB", "table1", "preflop", potSize);

            db.insert.setFlag(new string[] { "squeeze", "open", "win", "aggressor" });
            db.insert.actions("R", handId, "peanut", "BB", "table1", "preflop", potSize);
            db.insert.raises(db.select.lastHistoryId(), 12);

            /*
                Scenario 4 (Walk):
                Folds around and BB wins the pot
            */
            potSize = 1 + 2;
            handId++;
            db.insert.actions("F", handId, "peanut", "UTG", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain1", "MP", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain2", "CO", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain3", "BTN", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain4", "SB", "table1", "preflop", potSize);
            db.insert.actions("-", handId, "villain5", "BB", "table1", "preflop", potSize);

            /*
                More scenarios need to be added for extensive testing...
            */

            int newHandId = db.select.handId();

            Debug.Assert(newHandId == handId + 1, "select.HandId() is not working");
            Console.WriteLine("select.handId() is working...");
            Console.WriteLine("");

            /*
                VPIP testing
            */
            double vpip = db.select.VPIP("peanut");
            Console.WriteLine("VPIP:");
            Debug.Assert(vpip == 75, "Failed to get general VPIP stat");
            Console.WriteLine("Passed general VPIP...");

            double vpip_btn = db.select.VPIP("peanut", "BTN");
            Debug.Assert(vpip_btn == 100, "Failed to get positional VPIP stat");
            Console.WriteLine("Passed positional VPIP...");
            Console.WriteLine("select.VPIP() is working...");
            Console.WriteLine("");

            /*
                PFR testing
            */
            double pfr = db.select.PFR("peanut");
            Console.WriteLine("PFR:");
            Debug.Assert(pfr == 75, "Failed to get general PFR stat");
            Console.WriteLine("Passed general PFR...");

            double pfr_co = db.select.PFR("peanut", "CO");
            Debug.Assert(pfr_co == 0, "Failed to get positional PFR stat");
            Console.WriteLine("Passed positional PFR...");
            Console.WriteLine("select.PFR() is working...");
            Console.WriteLine("");

            /*
                Need to add a lot more post flop scenarios
            */

            /*
                Scenario 5 (Flop donk):
                MP opens (3x), BB calls -> BB raises 1/2 pot, MP folds
            */
            potSize = 6 + 6 + 1;
            handId++;
            db.insert.actions("F", handId, "villain5", "UTG", "table1", "preflop", potSize);

            db.insert.setFlag(new string[] { "open", "aggressor" });
            db.insert.actions("R", handId, "peanut", "MP", "table1", "preflop", potSize);
            db.insert.raises(db.select.lastHistoryId(), 4);

            db.insert.actions("F", handId, "villain1", "CO", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain2", "BTN", "table1", "preflop", potSize);
            db.insert.actions("F", handId, "villain3", "SB", "table1", "preflop", potSize);

            db.insert.setFlag(new string[] { "coldcall" });
            db.insert.actions("C", handId, "villain4", "BB", "table1", "preflop", potSize);
            /* ---- FLOP ---- */
            potSize += 8;

            db.insert.setFlag(new string[] { "donk", "win" });
            db.insert.actions("R", handId, "villain4", "BB", "table1", "flop", potSize);
            db.insert.raises(db.select.lastHistoryId(), 8);

            db.insert.setFlag(new string[] { "has_position" });
            db.insert.actions("F", handId, "villain4", "MP", "table1", "flop", potSize);

            /*
                3BET testing
            */
            /*double bet3 = db.select.BET3("peanut");
            Console.WriteLine("3BET:");
            Debug.Assert(pfr == 25, "Failed to get general 3Bet stat");
            Console.WriteLine("Passed general 3Bet...");

            double bet3_sb = db.select.PFR("peanut", "SB");
            Debug.Assert(bet3_sb == 0, "Failed to get positional 3Bet stat");
            Console.WriteLine("Passed positional 3Bet...");
            Console.WriteLine("select.BET3() is working...");
            Console.WriteLine("");*/

            Console.WriteLine("End of database API testing...");
            Console.WriteLine("");

            Console.ReadLine();

        }
    }
}
