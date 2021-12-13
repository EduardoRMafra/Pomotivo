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

        public static void AddNewTask(int id, string txtTask, string txtQuantity, string txtTime)
        {
            //adiciona uma nova linha apenas se a caixa de id estiver vazia
            if (id == 0)
            {
                dt.Rows.Add(dt.Rows.Count + 1, txtTask, txtQuantity, txtTime);
            }
        }
        public static void ModifyTask(int id, string txtTask, string txtQuantity, string txtTime)
        {
            dt.Rows[id]["Task"] = txtTask;

            if (txtQuantity != "")
            {
                {
                    dt.Rows[id]["Quantity"] = Convert.ToInt32(txtQuantity);
                }
            }
            if (dt.Rows[id]["Time"].ToString() != txtTime)
            {
                dt.Rows[id]["Time"] = Convert.ToInt32(txtTime);
            }
        }
        public static void DelTask(int id)
        {
            if (dt.Rows.Count > id)
            {
                dt.Rows.RemoveAt(id);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Sequence"] = i + 1;
                }
            }
        }
    }
}
