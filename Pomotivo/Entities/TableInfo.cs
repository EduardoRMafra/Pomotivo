using System;
using System.Windows.Forms;
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
        static int qtd; //quantidade atual da tarefa especifica

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
            //modifica as informações da tabela presente na posição id
            dt.Rows[id]["Task"] = txtTask;

            if (txtQuantity != "")
            {
                {
                    dt.Rows[id]["Quantity"] = Convert.ToInt32(txtQuantity);
                }
            }
            //verifica se o tempo da tarefa na tabela é diferente do tempo atual do programa e se o tempo no programa é diferente de 0, caso seja atribui esse novo valor a tabela
            if (dt.Rows[id]["Time"].ToString() != txtTime && Convert.ToInt32(txtTime) != 0)
            {
                dt.Rows[id]["Time"] = Convert.ToInt32(txtTime);
            }
        }
        public static void DelTask(int id)
        {
            //deleta a tarefa presente na posição id
            if (dt.Rows.Count > id)
            {
                dt.Rows.RemoveAt(id);

                SequenceTable();// coloca os numeros das tarefas com base na posição delas na tabela
            }
        }
        public static void MoveTask(int from, int to)
        {
            //Inicia um array com o tamanho dos itens selecionados na tabela
            string[] row1 = new string[dt.Rows[from].ItemArray.Length];
            string[] row2 = new string[dt.Rows[to].ItemArray.Length];

            //Atribui para os arrays os valores dos itens selecionados
            for (int i = 0; i < row1.Length; i++)
            {
                row1[i] = dt.Rows[from][i].ToString();
                row2[i] = dt.Rows[to][i].ToString();
            }
            //o item 1 recebe os valores do item 2
            dt.Rows[from][1] = row2[1];
            dt.Rows[from][2] = Convert.ToInt32(row2[2]);
            dt.Rows[from][3] = Convert.ToInt32(row2[3]);
            //o item 2 recebe os valores do item 1
            dt.Rows[to][1] = row1[1];
            dt.Rows[to][2] = Convert.ToInt32(row1[2]);
            dt.Rows[to][3] = Convert.ToInt32(row1[3]);
        }

        //após terminar uma tarefa
        public static void QuantityTask()
        {
            //pega o valor da coluna quantity do 1 item e diminui 1
            qtd = Convert.ToInt32(dt.Rows[0][2]) - 1;

            //se o valor continuar 1 ou maior atribui esse valor ao quantity da tabela, se não deleta a tarefa
            if (qtd >= 1)
            {
                dt.Rows[0][2] = qtd;
            }
            else
            {
                DelTask(0);
            }
        }

        public static void SequenceTable()
        {
            //organiza novamente o numero das posições das tarefas
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["Sequence"] = i + 1;
            }
        }
    }
}
