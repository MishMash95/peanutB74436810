using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace peanut.Database
{
    /*
        Functions to return all statistics / data from the database
    */
    public class Select : QueryBuilder
    {
        private SQLiteConnection dbConnection { get; set; }
        private SQLiteCommand command { get; set; }
        private SQLiteDataReader reader { get; set; }
        private SQLiteHelper helper { get; set; }

        private string sql { get; set; }

        public Select(SQLiteConnection dbcon)
        {
            this.dbConnection = dbcon;
        }


        /*
            To add:
            -agg fac/steal%/flop cbet/foldflopcbet/limpcall%/bbfoldtosteal/wtsd

            Parameters:
            -people in pot/pot size

            Have yet to test any functions and still need to add sql structures for most of them
        */
        public double VPIP(string username, string position = "ANY")
        {
            sql = Resources.getVPIP;
            sql = buildQuery(sql, position);
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return Convert.ToDouble(reader["VPIP"]);
        }
        public double PFR(string username, string position = "ANY")
        {
            sql = Resources.getPFR;
            sql = buildQuery(sql, position);
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return Convert.ToDouble(reader["PFR"]);
        }

        public int BET3(string username, string position = "ANY")
        {
            sql = Resources.get3Bet;
            sql = buildQuery(sql, position);
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["3BET"];
        }

        /*
        public int getF3B(string username, string position = "ANY")
        {
            sql = Resources.getF3B;
            sql = buildQuery(sql, position);
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["F3B"];
        }

        public int get4Bet(string username, string position = "ANY")
        {
            sql = Resources.get3Bet;
            sql = buildQuery(sql, position);
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["4BET"];
        }
        public int getDonkBet(string username, string position = "ANY")
        {
            sql = Resources.getDonkBet;
            sql = buildQuery(sql, position);
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["DONKBET"];
        }*/


        public int userId(string username)
        {
            sql = Resources.getUserId;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["username"];
        }

        /*
            Returns the next available handId for insertion into the database
        */
        public int handId()
        {
            sql = Resources.getHandId;
            command = new SQLiteCommand(sql, dbConnection);
            reader = command.ExecuteReader();
            reader.Read();
            if (reader["HANDID"] != DBNull.Value)
            {
                Console.WriteLine("Current handId = " + reader["HANDID"]);
                return Convert.ToInt32(reader["HANDID"]) + 1;
            }
            else
            {
                Console.WriteLine("No hands stored in table. HandId = 1");
                return 1;
            }
        }
        /*public int getNumberOfHandsOnPlayer(string username, string position = "ANY")
        {
            sql = Resources.getNumberOfHandsOnPlayer;
            sql = buildQuery(sql, position);
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return Convert.ToInt32(reader["handId"]);
        }*/

    }
}