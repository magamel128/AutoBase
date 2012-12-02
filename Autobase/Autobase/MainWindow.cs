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
        int active_table = 0;
          
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
            ins_data.operate = "insert";
            if (db.ds.Tables[active_table].TableName == "personal")
            {
                ins_data.Text = "Внесення інформації про працівника";
                ins_data.table_name = "personal";                
                ins_data.Size = new System.Drawing.Size(495, 355);  
                ins_data.panel1.Visible = true;
                ins_data.panel1.Enabled = true;

            }
            else
                if (db.ds.Tables[active_table].TableName == "model")
                {
                    ins_data.Text = "Внесення інформації по новому автомобілю";
                    ins_data.table_name = "model";
                    ins_data.Size = new System.Drawing.Size(395, 270);
                    ins_data.panel2.Visible = true;
                    ins_data.panel2.Enabled = true;                    
                }
           else
                    if (db.ds.Tables[active_table].TableName == "component")
                    {
                        ins_data.Text = "Внесення інформації по запчастинах";
                        ins_data.table_name = "components";
                    }
            ins_data.fm = this;
            ins_data.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть значення для видалення");
                return;
            }
            string index = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
            string table_name = db.ds.Tables[active_table].TableName;
            if (db.DeleteRec(table_name, index) == true)
            {
                MessageBox.Show("Дані видалені успішно");
            }
            else { MessageBox.Show("Помилка при виконанні операції"); }
            listView1.Items.Add("Здійснено видалення запису з таблиці" + table_name);
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Виберіть значення для зміни");
                return;
            }
            ModifyData mod_data = new ModifyData();
            mod_data.operate = "modify";
            if (db.ds.Tables[active_table].TableName == "personal")
            {
                mod_data.Text = "Модифікація інформації про працівника";
                mod_data.Size = new System.Drawing.Size(495, 355); 
                mod_data.panel1.Visible = true;
                mod_data.panel1.Enabled = true;
                mod_data.fm = this;
                mod_data.table_name = comboBox1.SelectedItem.ToString();
                mod_data.id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                //Ініціалізація полів;        
                mod_data.textBox1.Text = dataGridView1.SelectedRows[0].Cells["pib"].Value.ToString();
                mod_data.textBox2.Text = dataGridView1.SelectedRows[0].Cells["position"].Value.ToString();
                mod_data.textBox3.Text = dataGridView1.SelectedRows[0].Cells["telephon"].Value.ToString();
                mod_data.numericUpDown1.Value = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["salary"].Value);
                mod_data.dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["birth"].Value);
                mod_data.dateTimePicker2.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["date_work"].Value);   
                mod_data.panel1.Visible = true;
                mod_data.panel1.Enabled = true;
            }
            else
                if (db.ds.Tables[active_table].TableName == "model")
                {
                    mod_data.Text = "Внесення інформації по новому автомобілю";
                    mod_data.table_name = "model";
                    mod_data.Size = new System.Drawing.Size(395, 270);
                    mod_data.panel2.Visible = true;
                    mod_data.panel2.Enabled = true;
                    mod_data.table_name = comboBox1.SelectedItem.ToString();
                    mod_data.id = dataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                    mod_data.textBox4.Text = dataGridView1.SelectedRows[0].Cells["model"].Value.ToString();
                    mod_data.textBox5.Text = dataGridView1.SelectedRows[0].Cells["type"].Value.ToString();
                    mod_data.textBox6.Text = dataGridView1.SelectedRows[0].Cells["number"].Value.ToString();
                    mod_data.textBox7.Text = dataGridView1.SelectedRows[0].Cells["driver"].Value.ToString();
                    mod_data.dateTimePicker3.Text = dataGridView1.SelectedRows[0].Cells["create_date"].Value.ToString();

                }
           /*else
                    if (db.ds.Tables[active_table].TableName == "component")
                    {
                        ins_data.Text = "Внесення інформації по запчастинах";
                        ins_data.table_name = "components";
                    }*/
            mod_data.fm = this;
            mod_data.ShowDialog();     
        }        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.ds.Tables[comboBox1.SelectedIndex];
            active_table = comboBox1.SelectedIndex;
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

        private void вихідToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    da = new SQLiteDataAdapter("SELECT name FROM sqlite_master WHERE type='table';", connection);
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
        catch (Exception ex) { MessageBox.Show(ex.ToString()); return false; }
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
                ds.Tables[table_name].Rows.Clear();
                ds.Tables[table_name].Columns.Clear();
                da.Fill(ds, table_name);
                connection.Close();
            }
        }
        catch {return false; }
        return true;
    }

    public void InsertRec(string table_name, string id)
    {

    }

    public void ModifyRec(string table_name, string id)
    {

    }

    public bool DeleteRec(string table_name, string id)
    {
        try
        {
            string command = "DELETE FROM " + table_name + " WHERE id='" + id + "';";
            if (ExecuteCommand(command) == false) { return false; }
            if (renew_table(table_name) == false) { return false; }
        }
        catch { return false; }
        return true;
    }
}