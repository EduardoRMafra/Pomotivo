using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pomotivo.Entities;

namespace Pomotivo
{
    public partial class Form1 : Form
    {
        string taskDefault = "25";
        string smallBreakDefault = "5";
        string longBreakDefault = "30";
        public Form1()
        {
            InitializeComponent();
            InitializeProgram();
        }

        public void InitializeProgram()
        {
            StartTable();
        }
        public void StartTable()
        {
            //atribui as colunas que a tabela deve ter
            TableInfo.TableColumns();
            //exibe a tabela no dataGrid
            dataGridView1.DataSource = TableInfo.dt;
        }

        
    }
}
