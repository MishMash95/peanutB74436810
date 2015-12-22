using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Data.SQLite;
using System.Reflection;
using System.Data;

namespace Model
{
    public class Database
    {
        private string path { get; set; }
        private string filePath { get; set; }
        private string sql { get; set; }

        private SQLiteConnection dbConnection { get; set; }
        private SQLiteCommand command { get; set; }
        private SQLiteDataReader reader { get; set; }

        public Database(string dbFile = @"database.sqlite")
        {
            path = Directory.GetCurrentDirectory();
            Console.WriteLine("The current directory is {0}", path);

            if (File.Exists(dbFile))
            {
                System.Diagnostics.Debug.WriteLine("Database file exists");

                dbConnection = new SQLiteConnection("Data Source=" + path + dbFile + ";Version=3;");
                dbConnection.Open();
            }
            else
            {
                // Database file does not exists, so make it
                SQLiteConnection.CreateFile(dbFile);
                System.Diagnostics.Debug.WriteLine("Creating database file");

                dbConnection = new SQLiteConnection("Data Source=" + path + dbFile + ";Version=3;");
                dbConnection.Open();
                sql = Model.Resources.createTables;
                // Create all the tables
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }
        }

        ~Database()
        {
            // Cleanup
            // Close db connection
        }

        public int getVPIP(int userId)
        {
            sql = Model.Resources.getVPIP;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", userId));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["VPIP"];
        }
        public int getVPIP(string username)
        {
            int userId = getUserId(username);
            return getVPIP(userId);
        }
        public int getPFR(int userId)
        {
            sql = Model.Resources.getVPIP;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", userId));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["VPIP"];
        }
        public int getPFR(string username)
        {
            int userId = getUserId(username);
            return getPFR(userId);
        }
        public int getUserId(string username)
        {
            sql = Model.Resources.getUserId;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["username"];
        }
        public int getHandId()
        {
            sql = Model.Resources.getHandId;
            command = new SQLiteCommand(sql, dbConnection);
            reader = command.ExecuteReader();
            reader.Read();
            int i = reader.GetInt32(reader.GetOrdinal("handId")); // error
            return i;
        }


        public void insertPlayer(string username)
        {
            sql = Model.Resources.insertPlayer;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.ExecuteNonQuery();
        }

        public void insertPreFlopActions(string actions, int handId, int userId, string position, string tableName)
        {
            sql = Model.Resources.insertPreFlopActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@userId", userId));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }
        public void insertPreFlopActions(string actions, int handId, string username, string position, string tableName)
        {
            int userId = getUserId(username);
            insertPreFlopActions(actions, handId, userId, position, tableName);
        }
        public void insertFlopActions(string actions, int handId, int userId, string position, string tableName)
        {
            sql = Model.Resources.insertFlopActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@userId", userId));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }
        public void insertFlopActions(string actions, int handId, string username, string position, string tableName)
        {
            int userId = getUserId(username);
            insertFlopActions(actions, handId, userId, position, tableName);
        }
        public void insertTurnActions(string actions, int handId, int userId, string position, string tableName)
        {
            sql = Model.Resources.insertTurnActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@userId", userId));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }
        public void insertTurnActions(string actions, int handId, string username, string position, string tableName)
        {
            int userId = getUserId(username);
            insertFlopActions(actions, handId, userId, position, tableName);
        }
        public void insertRiverActions(string actions, int handId, int userId, string position, string tableName)
        {
            sql = Model.Resources.insertRiverActions;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@actionLine", actions));
            command.Parameters.Add(new SQLiteParameter("@handId", handId));
            command.Parameters.Add(new SQLiteParameter("@userId", userId));
            command.Parameters.Add(new SQLiteParameter("@position", position));
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }
        public void insertRiverActions(string actions, int handId, string username, string position, string tableName)
        {
            int userId = getUserId(username);
            insertRiverActions(actions, handId, userId, position, tableName);
        }

        public void insertTable(string tableName)
        {
            sql = Model.Resources.insertTable;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }

        public void truncateTable(string tableName)
        {
            if (tableExists(tableName))
            {
                sql = Model.Resources.truncateTable;
                command = new SQLiteCommand(sql, dbConnection);
                //var tableNameParameter = new SQLiteParameter("tableName", SqlDbType.VarChar) { Value = tableName };
                command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
                //command.Parameters.Add(tableNameParameter);
                command.ExecuteNonQuery();
            }
        }

        public bool tableExists(string tableName)
        {
            sql = Model.Resources.tableExists;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
 
            int exists = command.ExecuteNonQuery();
            return true;
        }

    }
}
