﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SQLite;
using System.Data;

namespace peanut.Database
{
    /*
        A series of functions to test the database structure/schema and to verify the select/insert queries
        are executing successfully and as expected
    */
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

            List<String> expectedTableNames = new List<String>(new String[] { "possibleActions", "history", "communityCards", "streets", "userStats", "statNames", "userHands", "tableNames", "users", "positions", "preFlopHandStrengths", "cards" });
            if (!expectedTableNames.Except(tableNames).Any())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool tableExists(string tableName)
        {
            Console.WriteLine("Checking `"+tableName+"` table exists...");

            string sql = Resources.tableExists;
            command = new SQLiteCommand(sql, dbConnection);
            command.Parameters.Add(new SQLiteParameter("@tableName", tableName));

            int exists = command.ExecuteNonQuery();
            return true;
        }
    }
}
