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
    public partial class InsertData : Form
    {
        public InsertData()
        {
            InitializeComponent();
        }

        public MainWindow fm;
        public int operate;
        public string id="";

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        bool renew_data(string commandText)
        {
            bool flag = false;
            using (fm.connect = new SQLiteConnection("Data source=autobase.db"))
            {
                fm.connect.Open();
                SQLiteCommand command = new SQLiteCommand(fm.connect);
                command.CommandText = commandText; 
                command.ExecuteNonQuery();
                fm.ds = new DataSet();
                fm.da = new SQLiteDataAdapter("SELECT * FROM personal", fm.connect);
                fm.da.Fill(fm.ds);
                fm.connect.Close();
                flag = true;
            }
            return flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string op = "";
            string mess = "";
            if (operate==1)
            {
                op = "INSERT INTO 'personal' ('pib', 'position','birth', 'date_work', 'telephon','salary') VALUES ('" + textBox1.Text +
                 "','" + textBox2.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "','" + textBox3.Text +
                 "','" + numericUpDown1.Value + "');";
                mess="Інформація успішно занесена";
            }
            if (operate==2)
            {
                op = "UPDATE 'personal' set pib='" + textBox1.Text + "', position='"+textBox2.Text+"', birth='"+dateTimePicker1.Text+"', date_work='"+dateTimePicker2.Text+
                    "', telephon='"+textBox3.Text +"', salary='"+numericUpDown1.Value+"' WHERE id="+id;
                mess ="Інформація успішно обновлена";
            }
            if (renew_data(op) == true) { MessageBox.Show(mess); }
            else MessageBox.Show("Помилка при виконанні операції");
            this.Close();
        }
    }
}
