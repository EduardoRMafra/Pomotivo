using ClosedXML.Excel;

namespace Pomotivo.Entities
{
    static class Save
    {
        public static void SaveTable(string path)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet1 = workbook.Worksheets.Add("Planilha1");
                CreateTable(worksheet1);

                workbook.SaveAs(path);  //salva o arquivo excel gerado no caminho escolhido
            }
        }
        //grava todos os dados da tabela em excel
        static void CreateTable(IXLWorksheet p)
        {
            p.Cell("A1").Value = TableInfo.dt.Columns[0].ColumnName;
            p.Cell("B1").Value = TableInfo.dt.Columns[1].ColumnName;
            p.Cell("C1").Value = TableInfo.dt.Columns[2].ColumnName;
            p.Cell("D1").Value = TableInfo.dt.Columns[3].ColumnName;

            for (int i = 0; i < TableInfo.dt.Rows.Count; i++)
            {
                for (int j = 0; j < TableInfo.dt.Columns.Count; j++)
                {
                    p.Cell(NumberConvert(j, i)).Value = TableInfo.dt.Rows[i][j].ToString();
                }
            }
        }

        //converte os numeros para o formato excel
        static string NumberConvert(int le, int n)
        {
            string l = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            n += 2; //excel começa com a linha 1 invez de 0, mas a 1 é o nome das colunas então n linha recebe +2
            string num = l.Substring(le, 1) + n.ToString();
            return num;
        }
    }
}
