using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;

namespace peanut.Database
{
    public class Test : QueryBuilder
    {
        private SQLiteConnection dbConnection { get; set; }
        private SQLiteCommand command { get; set; }
        private SQLiteDataReader reader { get; set; }
        private SQLiteHelper helper { get; set; }

        public Test(SQLiteConnection dbcon)
        {
            this.dbConnection = dbcon;
        }

        public bool allTablesExist()
        {
            command = new SQLiteCommand();
            command.Connection = dbConnection;
            helper = new SQLiteHelper(command);
            DataTable dt = helper.GetTableList();

            Console.WriteLine("Checking all tables exist...");
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
                return true;
            }
            else
            {
                Console.WriteLine("Failed.");
                return false;
            }
        }
        public bool tableExists(string tableName)
        {
            Console.WriteLine("Checking `"+tableName+"` table exists...");

            sql = Resources.tableExists;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));

            int exists = command.ExecuteNonQuery();
            Console.WriteLine("Passed.");
            return true;
        }
    }
}
