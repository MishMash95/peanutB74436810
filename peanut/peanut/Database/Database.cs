using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Data.SQLite;
using System.Reflection;
using System.Data;

namespace peanut
{
    public class Database
    {
        private string path { get; set; }
        private string filePath { get; set; }
        private string sql { get; set; }

        private SQLiteConnection dbConnection { get; set; }
        private SQLiteCommand command { get; set; }
        private SQLiteDataReader reader { get; set; }
        private SQLiteHelper sh { get; set; }

        public Database(string dbFile = @"database.sqlite")
        {
            path = Directory.GetCurrentDirectory() + "\\";
            Console.WriteLine("The current directory is {0}", path);

            if (File.Exists(dbFile))
            {
                Console.WriteLine("Database file exists");

                dbConnection = new SQLiteConnection("Data Source=" + path + dbFile + ";Version=3;");
                dbConnection.Open();
            }
            else
            {
                // Database file does not exists, so make it
                SQLiteConnection.CreateFile(path + dbFile);
                Console.WriteLine("Creating database file: `" + path + dbFile + "`");

                dbConnection = new SQLiteConnection("Data Source=" + path + dbFile + ";Version=3;");
                dbConnection.Open();
                sql = Resources.createTables;
                // Create all the tables
                command = new SQLiteCommand(sql, dbConnection);
                command.ExecuteNonQuery();
            }

            command = new SQLiteCommand();
            command.Connection = dbConnection;
            sh = new SQLiteHelper(command);
            DataTable dt = sh.GetTableList();

            Console.WriteLine("Checking tables exist...");
            List<String> tableNames = new List<String>();
            foreach (DataRow row in dt.Rows) // Loop over the rows.
            {
                foreach (var tableName in row.ItemArray) // Loop over the items.
                {
                    tableNames.Add(tableName.ToString());
                }
            }

            List<String> expectedTableNames = new List<String>(new String[] { "playerActions", "history_preflop", "history_flop", "history_turn", "history_river", "communityCards", "userStats", "statNames", "userHands", "tableNames", "users", "positions", "preFlopHandStrengths", "cards" });
            if (!expectedTableNames.Except(tableNames).Any())
            {
                Console.WriteLine("Passed.");
            }
            else
            {
                Console.WriteLine("Failed.");
            }
        }

        ~Database()
        {
            // Cleanup
            // Close db connection
        }

        public int getVPIP(int userId, string position = "ANY")
        {
            sql = Resources.getVPIP;
            if(position == "ANY")
            {
                sql.Replace("|WHERE_ALLHANDS|","");
                sql.Replace("|WHERE_VPIPHANDS|","");
            }
            else
            {
                sql.Replace("|WHERE_ALLHANDS|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
                sql.Replace("|WHERE_VPIPHANDS|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
            }

            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", userId));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["VPIP"];
        }
        public int getVPIP(string username, string position = "ANY")
        {
            int userId = getUserId(username);
            return getVPIP(userId, position);
        }
        public int getPFR(int userId, string position = "ANY")
        {
            sql = Resources.getPFR;
            if (position == "ANY")
            {
                sql.Replace("|WHERE_ALLHANDS|", "");
                sql.Replace("|WHERE_PFRHANDS|", "");
            }
            else
            {
                sql.Replace("|WHERE_ALLHANDS|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
                sql.Replace("|WHERE_PFRHANDS|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
            }
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", userId));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["PFR"];
        }
        public int getPFR(string username, string position = "ANY")
        {
            int userId = getUserId(username);
            return getPFR(userId, position);
        }
        public int get3Bet(int userId, string position = "ANY")
        {
            sql = Resources.get3Bet;
            if (position == "ANY")
            {
                sql.Replace("|WHERE_ALLHANDS|", "");
                sql.Replace("|WHERE_3BETHANDS|", "");
            }
            else
            {
                sql.Replace("|WHERE_ALLHANDS|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
                sql.Replace("|WHERE_3BETHANDS|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
            }
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", userId));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["3BET"];
        }
        public int get3Bet(string username, string position = "ANY")
        {
            int userId = getUserId(username);
            return get3Bet(userId, position);
        }
        public int getUserId(string username)
        {
            sql = Resources.getUserId;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            reader = command.ExecuteReader();
            reader.Read();
            return (int)reader["username"];
        }
        public int getHandId()
        {
            sql = Resources.getHandId;
            command = new SQLiteCommand(sql, dbConnection);
            reader = command.ExecuteReader();
            reader.Read();
            return Convert.ToInt32(reader["handId"]);
        }
        public int getNumberOfHandsOnPlayer(string username, string position = "ANY")
        {
            sql = Resources.getNumberOfHandsOnPlayer;
            if (position == "ANY")
            {
                sql.Replace("|WHERE_ALLHANDS|", "");
            }
            else
            {
                sql.Replace("|WHERE_ALLHANDS|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
            }
            command = new SQLiteCommand(sql, dbConnection);
            reader = command.ExecuteReader();
            reader.Read();
            return Convert.ToInt32(reader["handId"]);
        }

        public void insertPlayer(string username)
        {
            sql = Resources.insertPlayer;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@username", username));
            command.ExecuteNonQuery();
        }
        public void insertPreFlopActions(string actions, int handId, int userId, string position, string tableName)
        {
            sql = Resources.insertPreFlopActions;
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
            sql = Resources.insertFlopActions;
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
            sql = Resources.insertTurnActions;
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
            sql = Resources.insertRiverActions;
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
            sql = Resources.insertTable;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));
            command.ExecuteNonQuery();
        }

        public void truncateTable(string tableName)
        {
            if (tableExists(tableName))
            {
                sql = Resources.truncateTable;

                sh.Execute(sql,
                    new SQLiteParameter[] {
                        new SQLiteParameter("@tableName", tableName)
                    }
                );
            }
        }

        public bool tableExists(string tableName)
        {
            sql = Resources.tableExists;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));

            int exists = command.ExecuteNonQuery();
            return true;
        }

    }
}
