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
                {"opened", 0},
                {"3bet", 0},
                {"4bet", 0},
                {"aggressor", 0},
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

        public void actions(string actions, int handId, string username, string position, string tableName, string streetName,  int potSize) {
            sql = Resources.insertActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.Parameters.Add(new SQLiteParameter("@streetName", streetName));
            command.Parameters.Add(new SQLiteParameter("@potSize", potSize));

            command.Parameters.Add(new SQLiteParameter("@flg_opp", this.flags["opened"]));
            command.Parameters.Add(new SQLiteParameter("@flg_3b", this.flags["3bet"]));
            command.Parameters.Add(new SQLiteParameter("@flg_4b", this.flags["4bet"]));
            command.Parameters.Add(new SQLiteParameter("@flg_agg", this.flags["aggressor"]));
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
