using System.Data;
using ClosedXML.Excel;

namespace Pomotivo.Entities
{
    static class Open
    {
        static DataTable dtTemp;
        public static bool OpenTable(string path)
        {
            dtTemp = new DataTable(); //cria um DataTable temp
            using (XLWorkbook workbook = new XLWorkbook(path))
            {
                IXLWorksheet worksheet1 = workbook.Worksheet(1);
                CreateTable(worksheet1);

                if (dtTemp.Columns[0].ColumnName == "Sequence" && dtTemp.Columns[1].ColumnName == "Task" && dtTemp.Columns[2].ColumnName == "Quantity" && dtTemp.Columns[3].ColumnName == "Time")
                {
                    //verifica se o formato dos valores na tabela temp são validos
                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtTemp.Columns.Count; j++)
                        {
                            switch (j)
                            {
                                case 1:
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
            return true;
        }
        //pega todos os dados de em xlsx e converte para data table
        static void CreateTable(IXLWorksheet p)
        {
            dtTemp.Columns.Add(p.Cell("A1").Value.ToString());
            dtTemp.Columns.Add(p.Cell("B1").Value.ToString());
            dtTemp.Columns.Add(p.Cell("C1").Value.ToString());
            dtTemp.Columns.Add(p.Cell("D1").Value.ToString());

            for (int i = 0; i < p.RangeUsed().RowCount() - 1; i++)
            {
                string[] tableInfos = new string[p.RangeUsed().ColumnCount()];
                for (int j = 0; j < dtTemp.Columns.Count; j++)
                {
                    tableInfos[j] = p.Cell(NumberConvert(j, i)).Value.ToString();
                }
                dtTemp.Rows.Add(tableInfos[0], tableInfos[1], tableInfos[2], tableInfos[3]);
            }
        }

        //converte os numeros para o formato excel
        static string NumberConvert(int le, int n)
        {
            string l = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            n += 2; //excel começa com a linha 1 invez de 0, mas a l é o nome das colunas então n linha recebe +2
            string num = l.Substring(le, 1) + n.ToString();
            return num;
        }
    }
}
