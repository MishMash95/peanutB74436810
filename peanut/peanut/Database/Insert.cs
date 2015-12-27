using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

using peanut.Database;

namespace peanut.Database
{
    /*
        All data to be inserted into the database must be done through this class
    */
    public class Insert : QueryBuilder
    {
        private SQLiteConnection dbConnection { get; set; }
        private SQLiteCommand command { get; set; }
        private SQLiteHelper helper { get; set; }

        /*
            Although all statistics can be derived directly from the F,R,X,C actions made by players
            some queries can be long, tedious and processor intensive.
            
            So some general stats such as 3bet, 4bet, opened etc are now flagged in the history tables
            for quick select lookups.
        */
        private Dictionary<string, int> flags;

        private string sql { get; set; }

        public Insert(SQLiteConnection dbcon) {
            this.dbConnection = dbcon;
            this.resetFlags();
        }

        public void player(string username) {
            sql = Resources.insertPlayer;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.ExecuteNonQuery();
        }

        /*
            If the flags are not specified the default value will be 0
        */
        public void resetFlags() {
            this.flags = new Dictionary<string, int>() {
                {"has_position", 0},
                {"open", 0},
                {"3bet", 0},
                {"4bet", 0},
                {"limp", 0},
                {"cold_call", 0},
                {"squeeze", 0},
                {"aggressor", 0},
                {"donk", 0},
                {"win", 0}
            };
        }


        public void setFlag(string key) {
            this.flags[key] = 1;
        }

        public void setFlag(string[] keys) {
            foreach(string key in keys) {
                this.flags[key] = 1;
            }
        }

        public void raises(int historyId, int raise1, int raise2 = 0, int raise3 = 0, int raise4 = 0, int raise5 = 0) {
            sql = Resources.insertRaises;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@historyId", historyId));
            command.Parameters.Add(new SQLiteParameter("@raise1", raise1));
            command.Parameters.Add(new SQLiteParameter("@raise2", raise2));
            command.Parameters.Add(new SQLiteParameter("@raise3", raise3));
            command.Parameters.Add(new SQLiteParameter("@raise4", raise4));
            command.Parameters.Add(new SQLiteParameter("@raise5", raise5));
            command.ExecuteNonQuery();
        }

        public void actions(string actions, int handId, string username, string position, string tableName, string streetName,  int potSize) {

            // The button player preflop always has position on the other players in the hand
            if (position == "BTN" && streetName == "preflop") {
                this.flags["has_position"] = 1;
            }


            sql = Resources.insertActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.Parameters.Add(new SQLiteParameter("@streetName", streetName));
            command.Parameters.Add(new SQLiteParameter("@potSize", potSize));

            command.Parameters.Add(new SQLiteParameter("@flg_has_position", this.flags["has_position"]));
            command.Parameters.Add(new SQLiteParameter("@flg_open", this.flags["open"]));
            command.Parameters.Add(new SQLiteParameter("@flg_3bet", this.flags["3bet"]));
            command.Parameters.Add(new SQLiteParameter("@flg_4bet", this.flags["4bet"]));
            command.Parameters.Add(new SQLiteParameter("@flg_limp", this.flags["limp"]));
            command.Parameters.Add(new SQLiteParameter("@flg_cold_call", this.flags["cold_call"]));
            command.Parameters.Add(new SQLiteParameter("@flg_squeeze", this.flags["squeeze"]));
            command.Parameters.Add(new SQLiteParameter("@flg_aggressor", this.flags["aggressor"]));
            command.Parameters.Add(new SQLiteParameter("@flg_donk", this.flags["donk"]));
            command.Parameters.Add(new SQLiteParameter("@flg_win", this.flags["win"]));

            command.ExecuteNonQuery();
            this.resetFlags();
        }

        public void table(string tableName) {
            sql = Resources.insertTable;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }

    }
}
