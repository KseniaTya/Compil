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

            lexA.keywords.Sort();
            lexA.separators.Sort();
            lexA.richTextBox = richTextBox1.Text;
            lexA.LexAnalyzer();
            foreach(var s in lexA.errors)
            {
                textBox1.Text += s;
                textBox1.Text += "\n";
            }
        }

        private void save_to_file_Click(object sender, EventArgs e)
        {

        }
    }
}
