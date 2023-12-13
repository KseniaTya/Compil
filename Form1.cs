using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compil
{
    public partial class Form1 : Form
    {
        LexicalAnalyzer lexA = new LexicalAnalyzer();
        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView1.Refresh();
            dataGridView2.Refresh();
            dataGridView3.Refresh();

            lexA.keywords.Sort();
            lexA.separators.Sort();
            lexA.richTextBox = richTextBox1.Text;
            lexA.LexAnalyzer();
            if(lexA.keys.Count==0)
            {
                textBox1.Text = "Not found lexem";
                textBox1.Text = textBox1.Text + "\n";

            }
            foreach(var s in lexA.errors)
            {
                textBox1.Text = textBox1.Text + s;
                textBox1.Text = textBox1.Text + "\n";
            }

            int i = 0;
            foreach (var s in lexA.keys)
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells[0].Value = s;
                i = i + 1;
            }
            i = 0;
            foreach (var s in lexA.numbers)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = s;
                i = i + 1;
            }
            i = 0;
            foreach (var s in lexA.words)
            {
                dataGridView3.Rows.Add();
                dataGridView3.Rows[i].Cells[0].Value = s;
                i = i + 1;
            }

        }

        private void save_to_file_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /*int i = 0;
            foreach(var s in lexA.keys)
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells[0].Value = s;
                i = i + 1;
            }*/
        }
    }
}
