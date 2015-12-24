using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace peanut.Database
{
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


        public void insertPlayer(string username)
        {
            sql = Resources.insertPlayer;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.ExecuteNonQuery();
        }
        public void insertPreFlopActions(string actions, int handId, string username, string position, string tableName)
        {
            sql = Resources.insertPreFlopActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }
        public void insertFlopActions(string actions, int handId, string username, string position, string tableName)
        {
            sql = Resources.insertFlopActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }
        public void insertTurnActions(string actions, int handId, string username, string position, string tableName)
        {
            sql = Resources.insertTurnActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }
        public void insertRiverActions(string actions, int handId, string username, string position, string tableName)
        {
            sql = Resources.insertRiverActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }
        public void insertTable(string tableName)
        {
            sql = Resources.insertTable;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }

    }
}
