using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace peanut.Database
{
    /*
        This class constructs the queries based on a set of parameters/filters
    */
    public class QueryBuilder
    {
        private string filter = ""; // WHERE clause(s) attatched to the sql query
        protected string buildQuery(string sql, string position, string street)
        {
            filter = "";
            if(position == "ANY" && street == "ANY") {
                sql = sql.Replace("|WHERE_POSITION|", "");
                sql = sql.Replace("|ANDWHERE_POSITION|", "");
                return sql;
            }

            if(position != "ANY") {
                // Querying a specific player position
                filter += " positions.name = \"" + position + "\" ";
                if(street != "ANY") {
                    filter += " AND streets.name = \"" + street + "\" ";
                }
            }else if(street != "ANY") {
                filter += " streets.name = \"" + street + "\" ";
            }

            sql = sql.Replace("|WHERE_POSITION|", " WHERE " + filter);
            sql = sql.Replace("|ANDWHERE_POSITION|", " AND " + filter);

            return sql;
        }
    }
}
