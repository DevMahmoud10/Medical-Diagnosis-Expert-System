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
    public partial class Form2 : Form
    {
        Dictionary<string, string> scores = new Dictionary<string, string>();
        Controller c = new Controller();
        bool isCommonDone = false;
        int diseaseIndex = 0;
        int symptomIndex = 0;
        int commonIndex = 0;
        int mainSymptomIndex = 0;
        void setScores()
        {
            //MYCIN
            this.scores.Add((0).ToString(), (-1).ToString());
            this.scores.Add((1).ToString(), (-0.66).ToString());
            this.scores.Add((2).ToString(), (-0.33).ToString());
            this.scores.Add((3).ToString(), (0).ToString());
            this.scores.Add((4).ToString(), (0.33).ToString());
            this.scores.Add((5).ToString(), (0.66).ToString());
            this.scores.Add((6).ToString(), (1).ToString());

        }


        public Form2()
        {
            setScores();
            InitializeComponent();
            label1.AutoSize = true;
            label1.MaximumSize = new Size(290, 0);
            c.fillData("Dataset.xlsx");
            label1.Text = c.askQuestion(c.commonSymptoms[0]);


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void labelChange(int index, int type)
        {
            if (type == 0)
            {
                if (index < c.commonSymptoms.Count)
                    label1.Text = c.askQuestion(c.commonSymptoms[index]);
                else
                    label1.Text = c.askQuestion(c.mainSymptoms[0]);
            }
            else if (type == 1)
            {
                if (index < c.symptomsSheet.Count)
                    label1.Text = c.askQuestion(c.symptomsSheet[index]);
            }
            else if (type == 2) //main symptom
            {
                if (index < c.mainSymptoms.Count)
                    label1.Text = c.askQuestion(c.mainSymptoms[index]);
                else
                {
                    c.removeUnusedSymptoms(c.allSymptoms);
                    label1.Text = c.askQuestion(c.symptomsSheet[0]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (commonIndex < c.commonSymptoms.Count)
            {
                c.allSymptoms[c.commonSymptoms[commonIndex]] = Convert.ToDouble(scores[trackBar1.Value.ToString()]);
                commonIndex++;
                labelChange(commonIndex, 0);
            }
            else if (mainSymptomIndex < c.mainSymptoms.Count)
            {
                c.allSymptoms[c.mainSymptoms[mainSymptomIndex]] = Convert.ToDouble(scores[trackBar1.Value.ToString()]);
                mainSymptomIndex++;
                labelChange(mainSymptomIndex, 2);
            }
            else if (symptomIndex < c.symptomsSheet.Count)
            {

                c.allSymptoms[c.symptomsSheet[symptomIndex]] = Convert.ToDouble(scores[trackBar1.Value.ToString()]);
                symptomIndex++;
                labelChange(symptomIndex, 1);

            }
            else
            {

                c.DistriputeUserCF();
                Form3 f = new Form3(this.c);
                f.Show();
                this.Hide();
            }



        }


    }
}
