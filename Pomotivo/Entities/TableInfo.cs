using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Pomotivo.Entities
{
    static class TableInfo
    {
        public static DataTable dt = new DataTable();

        public static void TableColumns()
        {
            dt.Columns.Add("Sequence", typeof(int));
            dt.Columns.Add("Task", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Time", typeof(double));
        }
    }
}
