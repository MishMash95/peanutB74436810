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
        protected string buildQuery(string sql, string position)
        {
            if (position == "ANY")
            {
                sql.Replace("|WHERE_POSITION|", "");
                sql.Replace("|ANDWHERE_POSITION|", "");
            }
            else
            {
                sql.Replace("|WHERE_POSITION|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
                sql.Replace("|ANDWHERE_POSITION|", "AND position_id = (SELECT id FROM positions WHERE positionName = " + position + " ");
            }

            return sql;
        }
    }
}
