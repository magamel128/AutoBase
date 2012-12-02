using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApplication1
{
    public partial class ModifyData : Form
    {
        public ModifyData()
        {
            InitializeComponent();
        }

        public MainWindow fm;
        public string operate;
        public string id="";
        public string table_name="";
        
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        void closewindow()
        {
            panel1.Visible = false;
            panel1.Enabled = false;
            panel2.Visible = false;
            panel2.Enabled = false;
            panel3.Visible = false;
            panel3.Enabled = false;
            this.Close();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string commamd = "";
            string mess = "";
            if (operate=="insert")
            {
                commamd = "INSERT INTO '" + table_name + "' ('pib', 'position','birth', 'date_work', 'telephon','salary') VALUES ('" + textBox1.Text +
                 "','" + textBox2.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "','" + textBox3.Text +
                 "','" + numericUpDown1.Value + "');";
                mess="Інформація успішно занесена";
            }
            if (operate=="modify")
            {
                commamd = "UPDATE '" + table_name + "' set pib='" + textBox1.Text + "', position='" + textBox2.Text + "', birth='" + dateTimePicker1.Text + "', date_work='" + dateTimePicker2.Text +
                    "', telephon='"+textBox3.Text +"', salary='"+numericUpDown1.Value+"' WHERE id="+id;
                mess ="Інформація успішно обновлена";
            }
            if (fm.db.ExecuteCommand(commamd) == true && fm.db.renew_table(table_name) == true) { MessageBox.Show(mess); }
            else MessageBox.Show("Помилка при виконанні операції");
            closewindow();           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string commamd = "";
            string mess = "";
            if (operate=="insert")
            {
                commamd = "INSERT INTO '" + table_name + "' ('type', 'create_date','number', 'model','driver') VALUES ('" + textBox5.Text +
                 "','" + dateTimePicker3.Text+ "','" + textBox6.Text + "','" +textBox4.Text + "','" + textBox7.Text + "');";
                mess="Інформація успішно занесена";
            }
            if (operate=="modify")
            {
                commamd = "UPDATE '" + table_name + "' set type='" + textBox5.Text + "', create_date='" + dateTimePicker3.Text + "', number='" + textBox6.Text +
                    "', model='" + textBox4.Text + "', driver='" + textBox7.Text + "' WHERE id=" + id;
                mess ="Інформація успішно обновлена";
            }
            if (fm.db.ExecuteCommand(commamd) == true && fm.db.renew_table(table_name) == true) { MessageBox.Show(mess); }
            else MessageBox.Show("Помилка при виконанні операції");
            closewindow();           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}
