using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Reflection;
using System.Data;

using System.Data.SQLite;

namespace peanut.Database
{
    /*
        API class to interact with the database
        - Includes some general functions for emptying and creating the database/tables
    */

    public class Database
    {
        private string path { get; set; }
        private string filePath { get; set; }
        private string sql { get; set; }

        public Insert insert;
        public Select select;
        public Test test;

        private SQLiteConnection dbConnection { get; set; }
        private SQLiteCommand command { get; set; }
        private SQLiteDataReader reader { get; set; }

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
                Console.WriteLine("Creating database file: `" + dbFile + "`");

                dbConnection = new SQLiteConnection("Data Source=" + path + dbFile + ";Version=3;");
                dbConnection.Open();
               
                
                sql = Resources.createTables;
                // Create all the tables
                try
                {
                    command = new SQLiteCommand(sql, dbConnection);
                    command.CommandText = Resources.createTables;
                    //Console.WriteLine("SQL: "+command.CommandText+"/n");
                    command.ExecuteNonQuery();
                }
                catch( Exception e )
                {
                    Console.WriteLine("\nException:");
                    Console.WriteLine(e);
                    Console.WriteLine();
                }
            }

            this.insert = new Insert(dbConnection);
            this.select = new Select(dbConnection);
            this.test = new Test(dbConnection);
        }

        public void truncateTable(string tableName)
        {
           if (test.tableExists(tableName))
           {
                sql = Resources.truncateTable;
                // Parameters can't be used for table name, schemas etc
                sql = sql.Replace("@tableName", tableName);
                command = new SQLiteCommand(sql, dbConnection);
                Console.WriteLine("Emptying `" + tableName + "` table...");
                command.ExecuteNonQuery();
           }
        }

    }
}
