using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Medical_Diagnosis_Expert_System
{
    public partial class Form3 : Form
    {
        Controller c = new Controller();
        public Form3()
        {


        }
        public Form3(Controller x)
        {
            InitializeComponent();
            this.c = x;
            for (int i = 0; i < this.c.Diseases.Count; i++)
            {
                this.c.Diseases[i].calculateCF();
            }

            string s="";
            this.c.output();
            for (int i = 0; i < this.c.result.Count;i++ )
            {
                s += "\n";
                s += this.c.result[i].Key+" : ";
                s +="\n";
                s += this.c.result[i].Value+" % ";
               s += "\n";
            }
                textBox1.Text = s;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
