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

        private string sql { get; set; }

        public Insert(SQLiteConnection dbcon)
        {
            this.dbConnection = dbcon;
        }

        public void player(string username)
        {
            sql = Resources.insertPlayer;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.ExecuteNonQuery();
        }
        public void actions(string actions, int handId, string username, string position, string tableName, string streetName, 
                            int finalPotSize, 
                            int opened, int bet3, int bet4, int aggressor, int win) // Booleans represented as integers as SQLITE does not suppoer BOOL
        {
            sql = Resources.insertActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.Parameters.Add(new SQLiteParameter("@streetName", streetName));

            command.Parameters.Add(new SQLiteParameter("@finalPotSize", finalPotSize));

            command.Parameters.Add(new SQLiteParameter("@flg_opp", opened));
            command.Parameters.Add(new SQLiteParameter("@flg_3b", bet3));
            command.Parameters.Add(new SQLiteParameter("@flg_4b", bet4));
            command.Parameters.Add(new SQLiteParameter("@flg_agg", aggressor));
            command.Parameters.Add(new SQLiteParameter("@flg_win", win));
            command.ExecuteNonQuery();
        }
        public void table(string tableName)
        {
            sql = Resources.insertTable;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }

    }
}
