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
        string taskTimeDefault = "25";
        string smallBreakDefault = "5";
        string longBreakDefault = "30";
        public Form1()
        {
            InitializeComponent();
            InitializeProgram();
        }

        public void InitializeProgram()
        {
            DefaultInfo();
            StartTable();
        }
        public void StartTable()
        {
            //atribui as colunas que a tabela deve ter
            TableInfo.TableColumns();
            //exibe a tabela no dataGrid
            dataGridView1.DataSource = TableInfo.dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsValid("Add"))
            {
                TableInfo.AddNewTask(Convert.ToInt32("0" + txtSequence.Text), txtTask.Text, txtQuantity.Text,txtPomo.Text);
                ClearForm();
            }
        }
        void DefaultInfo()
        {
            txtPomo.Text = taskTimeDefault;
            txtSmallBreak.Text = smallBreakDefault;
            txtLongBreak.Text = longBreakDefault;

            txtTask.Select();
        }
        //verifica se as ações na tabela são validas e corrige para valores padrão
        bool IsValid(string btnName)
        {
            bool result = true;
            switch (btnName)
            {
                case "Add":
                    //retira os espaços para verificar se o campo está em branco
                    string task = txtTask.Text.Trim();
                    //------
                    if (task == "")
                    {
                        MessageBox.Show("Task description is needed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTask.Focus();
                        result = false;
                    }
                    else if (txtQuantity.Text == "" || Convert.ToInt32(txtQuantity.Text) <= 0)
                    {
                        txtQuantity.Text = "1";
                    }
                    if (txtPomo.Text == "" || int.Parse(txtPomo.Text) <= 0)
                    {
                        txtPomo.Text = taskTimeDefault;
                    }
                    break;

                case "Modify":

                    if (txtTask.Text == "")
                    {
                        MessageBox.Show("Task description is needed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTask.Focus();
                        result = false;
                    }
                    else if (txtSequence.Text == "")
                    {
                        MessageBox.Show("Sequence number is needed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSequence.Focus();
                        result = false;
                    }
                    else if (dataGridView1.Rows.Count < Convert.ToInt32(txtSequence.Text))
                    {
                        MessageBox.Show("Sequence number is invalid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSequence.Focus();
                        result = false;
                    }
                    break;

                case "Del":
                    if (dataGridView1.Rows.Count < Convert.ToInt32(txtSequence.Text))
                    {
                        MessageBox.Show("Sequence number is invalid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSequence.Focus();
                        result = false;
                    }
                    else if (txtSequence.Text == "")
                    {
                        MessageBox.Show("Sequence number is needed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSequence.Focus();
                        result = false;
                    }
                    break;

                case "Move":
                    if (txtFrom.Text == "")
                    {
                        MessageBox.Show("From number is needed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtFrom.Focus();
                        result = false;
                    }
                    else if (txtTo.Text == "")
                    {
                        MessageBox.Show("To number is needed!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTo.Focus();
                        result = false;
                    }
                    else if (dataGridView1.Rows.Count < Convert.ToInt32(txtFrom.Text))
                    {
                        MessageBox.Show("From number is invalid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtFrom.Focus();
                        result = false;
                    }
                    else if (dataGridView1.Rows.Count < Convert.ToInt32(txtTo.Text))
                    {
                        MessageBox.Show("To number is invalid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTo.Focus();
                        result = false;
                    }
                    else if (0 > Convert.ToInt32(txtFrom.Text) - 1)
                    {
                        MessageBox.Show("From number is invalid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtFrom.Focus();
                        result = false;
                    }
                    else if (0 > Convert.ToInt32(txtTo.Text) - 1)
                    {
                        MessageBox.Show("To number is invalid!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtTo.Focus();
                        result = false;
                    }
                    break;
            }
            return result;
        }
        //limpa todos os text boxes
        void ClearForm()
        {
            txtSequence.Clear();
            txtTask.Clear();
            txtQuantity.Clear();
            txtFrom.Clear();
            txtTo.Clear();
        }
    }
}
