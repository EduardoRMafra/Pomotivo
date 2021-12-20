using System.Data;
using System.Data.OleDb;

namespace Pomotivo.Entities
{
    static class Open
    {
        public static bool OpenTable(string path)
        {
            DataTable dtTemp = new DataTable(); //cria um DataTable temp
            string spreadsheet = "SELECT * FROM [Planilha1$]";  // Planilha1 é o nome padrão gerado pelo excel ao criar uma tabela
            var strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=0\"";

            using (OleDbConnection con = new OleDbConnection(strCon))
            {
                using (OleDbDataAdapter da = new OleDbDataAdapter(spreadsheet, con))
                {
                    da.Fill(dtTemp);    //preenche a tabela temp
                    //verifica se as colunas da tabela temporaria são validas
                    if (dtTemp.Columns[0].ColumnName == "Sequence" && dtTemp.Columns[1].ColumnName == "Task" && dtTemp.Columns[2].ColumnName == "Quantity" && dtTemp.Columns[3].ColumnName == "Time")
                    {
                        //verifica se o formato dos valores na tabela temp são validos
                        for(int i = 0; i < dtTemp.Rows.Count; i++)
                        {
                            for(int j = 0; j < dtTemp.Columns.Count; j++)
                            {
                                switch (j)
                                {
                                    case 1:
                                        break;
                                    case 3:
                                        double tempDouble;
                                        if (!double.TryParse(dtTemp.Rows[i][j].ToString(), out tempDouble) && tempDouble.ToString() != "")
                                        {
                                            return false;
                                        }
                                        break;
                                    default:
                                        int tempInt;
                                        if (!int.TryParse(dtTemp.Rows[i][j].ToString(), out tempInt) && tempInt.ToString() != "")
                                        {
                                            return false;
                                        }
                                        break;
                                }
                            }
                        }
                        //se o formato estiver correto apaga a tabela e atualiza ela com os valores da tabela temp
                        TableInfo.dt = new DataTable();
                        TableInfo.dt = dtTemp;
                        TableInfo.SequenceTable();  //ajusta os números da Sequencia caso estejem incorretos
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
