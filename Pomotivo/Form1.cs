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
        string taskTimeDefault = "25";  //minutos padrão da tarefa
        string smallBreakDefault = "5"; //minutos padrão do intervalo pequeno
        string longBreakDefault = "30"; //minutos padrão do intervalo grande
        int qtBreaks = 0;   //quantidade de intervalos, depois de 3 acontece um longo
        bool b; //se está no intervalo
        Time CurrentTimer = new Time(0,0,0);    //iniciando a classe de contador zerado
        public Form1()
        {
            InitializeComponent();
            InitializeProgram();
        }

        public void InitializeProgram()
        {
            DefaultInfo();  //chama a função DefaultInfo que ao abrir o programa atribui os valores padrão para a tarefa e para os intervalos
            StartTable();   //chama a função que inicia a tabela
        }
        public void StartTable()
        {
            //atribui as colunas que a tabela deve ter
            TableInfo.TableColumns();
            //exibe a tabela no dataGrid
            dataGridView1.DataSource = TableInfo.dt;
        }
        //---------------------------------------------------Funções de interação de Botões-------------------------------------------------------
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsValid("Add")) // IsValid verifica se os campos necessarios para chamar proxima função está preenchida corretamente
            {
                //chama a função da classe TableInfo para adicionar uma nova tarefa na tabela
                TableInfo.AddNewTask(Convert.ToInt32("0" + txtSequence.Text), txtTask.Text, txtQuantity.Text,txtPomo.Text);
                ClearForm();    //limpa todos os campos
                txtTask.Focus();    //da destaque ao campo da tarefa
            }
        }
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (IsValid("Modify"))
            {
                TableInfo.ModifyTask(Convert.ToInt32(txtSequence.Text) - 1, txtTask.Text, txtQuantity.Text, txtPomo.Text);
                ClearForm();
                txtTask.Focus();
            }
        }
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (IsValid("Del"))
            {
                TableInfo.DelTask(Convert.ToInt32(txtSequence.Text) - 1);
                ClearForm();
                txtTask.Focus();
            }
        }
        private void btnMove_Click(object sender, EventArgs e)
        {
            if (IsValid("Move"))
            {
                TableInfo.MoveTask(Convert.ToInt32(txtFrom.Text) - 1, Convert.ToInt32(txtTo.Text) - 1);
                ClearForm();
                txtTask.Focus();
            }
        }

        //Ao pressionar o botão Start
        private void btnStart_Click(object sender, EventArgs e)
        {
            //verifica se o contador está ativo ou não
            if (timer1.Enabled == false)
            {
                if (txtCurrent.Text == "" || txtTimer.Text == "")   //se a tarefa atual ou o tempo estiverem vazios verifica se existem tarefas na tabela
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        UpdateTimer(Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value), b);  //manda para a função UpdateTimer o tempo da tarefa e se é ou não intervalo
                        btnStart.Text = "Pause";
                    }
                    else   //Se não existem tarefas manda um aviso
                    {
                        MessageBox.Show("Has no tasks! Add news", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnStart.Text = "Start";
                    }
                }
                else if (dataGridView1.Rows.Count > 0)  //se existem tarefas ativa o contador
                {
                    timer1.Enabled = true;
                    btnStart.Text = "Pause";

                    //verifica se a tarefa atual do timer não é a mesma da tabela e se não está em intervalo
                    if (txtCurrent.Text != dataGridView1.Rows[0].Cells[1].Value.ToString() && txtCurrent.Text != "Break")
                    {
                        UpdateTimer(Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value), b);  //atualiza o timer
                    }
                }
            }
            else   //se o contador estiver ativo desativa
            {
                timer1.Enabled = false;
                btnStart.Text = "Start";
            }
        }

        //Ao pressionar o botão abrir
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled == true) //caso o contador estiver funcionando manda uma mensagem de erro
            {
                MessageBox.Show("This function can't be used on this moment!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(openFileDialog1.ShowDialog() == DialogResult.OK) //ao clicar ok na janela de abrir
            {
                if (Open.OpenTable(openFileDialog1.FileName))   //chama a função open que verifica se a tabela está com os dados corretos
                {
                    dataGridView1.DataSource = TableInfo.dt;    //atualiza os dados.
                }
                else  //caso o arquivo não possa ser aberto
                {
                    MessageBox.Show("Table Formate Incorrect!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
        }
        //botão salvar
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)    //ao pressionar ok salva a tabela em excel
            {
                Save.SaveTable(saveFileDialog1.FileName);
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------
        void UpdateTimer(int StartTimer, bool bk)
        {
            int tempMM = StartTimer;
            int tempHH = 0;
            if (!bk)    //Se não ser intervalo atribui o nome da tarefa para o txtCurrent
            {
                txtCurrent.Text = dataGridView1.Rows[0].Cells[1].Value.ToString();
            }
            else       //se for atribui o nome Break para o txtCurrent
            {
                txtCurrent.Text = "Break";
            }

            while (tempMM >= 60)    //converte cada 60min em 1hora
            {
                tempMM -= 60;
                tempHH += 1;
            }
            CurrentTimer = new Time(tempHH, tempMM, 0); //CurrentTimer recebe o tempo informado
            timer1.Enabled = true;  //contador habilitado
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //a cada segundo enquanto o timer estiver habilitado
            Time.StartTimer();  //diminui um segundo e mantem o formato de contador
            txtTimer.Text = CurrentTimer.ToString();    //apresenta o tempo atual no txtTimer
            if(CurrentTimer.ToString() == "0:0:0")  //se o tempo acabar desabilita o contador
            {
                timer1.Enabled = false;

                if (b == false) //se tiver acabado uma tarefa
                {
                    //se o usuario deseja ou não fazer o intervalo
                    SkipQuestion();
                }
                else            //se acabou um intervalo
                {
                    if (MessageBox.Show("The break is over! Start the task", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        b = false;  //intervalo recebe false

                        if (dataGridView1.Rows.Count > 0)   //se ainda tiver tarefas na lista pega o tempo da tarefa na lista e atualiza o contador
                        {
                            UpdateTimer(Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value), b);
                        }
                        else       //se não tiver mais tarefas manda um aviso
                        {
                            MessageBox.Show("Has no tasks! Add news", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            btnStart.Text = "Start";
                        }
                    }
                }
            }
        }
        void SkipQuestion() //atualiza a quantidade de tarefas e pergunta se o usuario deseja fazer o intervalo ou pular
        {
            if (MessageBox.Show(txtCurrent.Text + "is over! Start break", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                b = true;
                TableInfo.QuantityTask();
                qtBreaks++;

                if (qtBreaks < 4)
                {
                    UpdateTimer(Convert.ToInt32(txtSmallBreak.Text), b);
                }
                else
                {
                    UpdateTimer(Convert.ToInt32(txtLongBreak.Text), b);
                    qtBreaks = 0;
                }
            }
            else
            {
                if (MessageBox.Show("Are you sure that you want skip this break?", "Warning!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    TableInfo.QuantityTask();
                    qtBreaks++;

                    if (qtBreaks >= 4)
                    {
                        qtBreaks = 0;
                    }

                    if (dataGridView1.Rows.Count > 0)
                    {
                        UpdateTimer(Convert.ToInt32(dataGridView1.Rows[0].Cells[3].Value), b);
                    }
                    else
                    {
                        MessageBox.Show("Has no tasks! Add news", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnStart.Text = "Start";
                    }
                }
                else       // caso o usuario responda não para as perguntas ela pergunta novamente
                {
                    SkipQuestion();
                }
            }
        }
        void DefaultInfo()
        {
            txtPomo.Text = taskTimeDefault;
            txtSmallBreak.Text = smallBreakDefault;
            txtLongBreak.Text = longBreakDefault;

            txtTask.Select();
        }
        //verifica se as ações na tabela são validas e quando possivel corrige para valores padrão
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
                    break;

                case "Modify":
                    task = txtTask.Text.Trim();
                    if (task == "")
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
                    else if(Convert.ToInt32(txtSequence.Text) - 1 <= 0)
                    {
                        MessageBox.Show("This task don't exist or can't be modify!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    else if (Convert.ToInt32(txtSequence.Text) - 1 == 0 && timer1.Enabled == true)
                    {
                        MessageBox.Show("This task can't be deleted on this moment!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    else if (Convert.ToInt32(txtFrom.Text) - 1 == 0 && timer1.Enabled == true || Convert.ToInt32(txtTo.Text) - 1 == 0 && timer1.Enabled == true)
                    {
                        MessageBox.Show("This task can't be moved on this moment!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtFrom.Focus();
                        result = false;
                    }
                    break;
            }

            if (txtPomo.Text == "" || int.Parse(txtPomo.Text) <= 0)
            {
                txtPomo.Text = taskTimeDefault;
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
        //Impedir que o usuário consiga colocar letras em locais que devem aceitar apenas números
        void OnlyNumbers(KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != Convert.ToChar(Keys.Delete) && ch != Convert.ToChar(Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void txtPomo_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(e);
        }
        private void txtSmallBreak_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(e);
        }
        private void txtLongBreak_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(e);
        }
        private void txtSequence_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(e);
        }
        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(e);
        }
        private void txtFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(e);
        }
        private void txtTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            OnlyNumbers(e);
        }
    }
}
