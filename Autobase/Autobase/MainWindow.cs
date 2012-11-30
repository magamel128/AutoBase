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
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            db = new Work_DB();
            LoadForm load_form = new LoadForm();
            load_form.fm = this;
            load_form.ShowDialog();
        }

        public Work_DB db;
          
        private void button1_Click(object sender, EventArgs e)
        {
           // dataGridView1.DataSource = ds.Tables[0];           
        }

        private void вихідToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void довідкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutProgram about = new AboutProgram();
            about.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ModifyData ins_data = new ModifyData();
            ins_data.panel1.Visible = true;
            ins_data.panel1.Enabled = true;
            ins_data.Text = "Внесення інформації про працівника";
            ins_data.fm = this;
            ins_data.operate=1;
            ins_data.ShowDialog();           
            dataGridView1.DataSource = db.ds.Tables["personal"];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть значення для зміни");
                return;
            }
            string index = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
            string command = "DELETE FROM personal WHERE id='" + index + "';";
            string table_name = "personal";
            if (db.ExecuteCommand(command) == true && db.renew_table(table_name) == true)
            {
                MessageBox.Show("Дані видалені успішно");
            }
            else { MessageBox.Show("Помилка при виконанні операції"); }           
        }

        void pre_mod_personal()
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {
                try
                {
                    ModifyData ins_data = new ModifyData();
                    ins_data.Text = "Модифікація інформації про працівника";
                    ins_data.panel1.Visible = true;
                    ins_data.panel1.Enabled = true;
                    ins_data.fm = this;
                    ins_data.table_name = comboBox1.SelectedItem.ToString();
                    ins_data.operate = 2;
                    ins_data.id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                    //Ініціалізація полів;                    
                    ins_data.textBox1.Text = dataGridView1.SelectedRows[0].Cells["pib"].Value.ToString();
                    ins_data.textBox2.Text = dataGridView1.SelectedRows[0].Cells["position"].Value.ToString();
                    ins_data.textBox3.Text = dataGridView1.SelectedRows[0].Cells["telephon"].Value.ToString();
                    ins_data.numericUpDown1.Value = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["salary"].Value);
                    ins_data.dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["birth"].Value);
                    ins_data.dateTimePicker2.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["date_work"].Value);
                    ins_data.ShowDialog();
                    dataGridView1 = new DataGridView();
                    dataGridView1.DataSource = db.ds.Tables["personal"];
                 }
                catch (Exception ex) { MessageBox.Show("Сталась помилка"+ex.ToString()); }
            }
            else
            {
                MessageBox.Show("Виберіть значення для зміни");
            }
           
        }        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.ds.Tables[comboBox1.SelectedIndex];
        }

        private void персоналToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.ds.Tables["personal"];
        }

        private void автомобіліToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.ds.Tables["model"];
        }

        private void комплектуючіToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.ds.Tables["component"];
        }
    }
}


public class Work_DB
{
    SQLiteConnection connection;
    SQLiteDataAdapter da;
    public DataSet ds;

    public bool Connect()
    {
        try
            {
                ds = new DataSet();
                using (connection = new SQLiteConnection("Data source=autobase.db"))
                {
                    connection.Open();                    
                    da = new SQLiteDataAdapter("SELECT name FROM sqlite_master WHERE type='table' ORDER BY name;", connection);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string table_name = dt.Rows[i]["name"].ToString();
                        if (table_name != "sqlite_sequence")
                        {
                            string command = "SELECT * FROM " + table_name;
                            da = new SQLiteDataAdapter(command, connection);
                            da.Fill(ds, table_name);
                        }                    
                    }
                    connection.Close();                   
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.ToString());
               MessageBox.Show("Помилка завантаження бази даних" + (char)13 + "Програма буде закрита");
               return false;
            }
        return true;
    }

    public bool ExecuteCommand(string commamnd)
    {
        try
        {
            using (connection = new SQLiteConnection("Data source=autobase.db"))
            {
                connection.Open();
                SQLiteCommand command = new SQLiteCommand(connection);
                command.CommandText = commamnd;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch { return false; }
        return true;
    }

    public bool renew_table(string table_name)
    {       
        try
        {
            using (connection = new SQLiteConnection("Data source=autobase.db"))
            {
                connection.Open();
                da = new SQLiteDataAdapter("SELECT * FROM " + table_name, connection);
                da.Fill(ds, table_name);
                connection.Close();
            }
        }
        catch {return false; }
        return true;
    }
}