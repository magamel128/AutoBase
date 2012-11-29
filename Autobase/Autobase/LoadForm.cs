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
    public partial class LoadForm : Form
    {
        public LoadForm()
        {
            InitializeComponent();
            timer1.Enabled = true;

        }

        public MainWindow fm;

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (fm.db.Connect()==false) { Application.Exit();}
            for (int i = 0; i < fm.db.ds.Tables.Count; i++)
                fm.comboBox1.Items.Add(fm.db.ds.Tables[i].TableName);
            fm.comboBox1.Text = fm.db.ds.Tables[0].TableName;
            fm.dataGridView1.DataSource = fm.db.ds.Tables[0];
            this.Close();
        }
    }
}
