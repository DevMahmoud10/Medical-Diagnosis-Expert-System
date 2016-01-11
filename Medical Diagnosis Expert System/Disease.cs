using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Diagnosis_Expert_System
{
    public class Disease
    {
        public string name;
        public List<Symptom> symptoms;
        public double finalScore;
        public Symptom mainSymptom;

        public Disease ()
        {
            this.symptoms = new List<Symptom>();
        }

        public Disease(string name)
        {
            this.name = name;
            this.symptoms = new List<Symptom>();
        }

        public void setfinalScore(double score)
        {
            this.finalScore = score;
        }

        public double getUserCF()
        {
            return this.finalScore;
        }

        public void setMainSymptom(string name,double score)
        {

            this.mainSymptom = new Symptom(name, score);
        }

        public void calculateCF()
        {

            var minCF = -99.0;
            var mb = 0.0;
            var md = 0.0;
            //main symptom CF
            if (this.mainSymptom != null)
            {
                if (this.mainSymptom.getUserCF() != -1)
                {
                    mb = calculateMB(this.mainSymptom.expertCF, this.mainSymptom.getUserCF());
                    md = calculateMD(this.mainSymptom.expertCF, this.mainSymptom.getUserCF());
                    this.mainSymptom.symptomCF = (mb - md) / (1 - Math.Min(mb, md));
                    minCF = this.mainSymptom.symptomCF;
                }
            }// el else 
            
                
            for(int i=0;i<this.symptoms.Count;i++)
            {
                if (this.mainSymptom != null)
                {
                    if (this.mainSymptom.getUserCF() != -1)
                    {
                        mb = calculateMB(this.symptoms[i].expertCF, this.symptoms[i].getUserCF());
                        md = calculateMD(this.symptoms[i].expertCF, this.symptoms[i].getUserCF());
                        this.symptoms[i].symptomCF = (mb - md) / (1 - Math.Min(mb, md));

                        if (minCF < this.symptoms[i].symptomCF)
                            minCF = this.symptoms[i].symptomCF;


                    }

                }
                else
                {
                    mb = calculateMB(this.symptoms[i].expertCF, this.symptoms[i].getUserCF());
                    md = calculateMD(this.symptoms[i].expertCF, this.symptoms[i].getUserCF());
                    this.symptoms[i].symptomCF = (mb - md) / (1 - Math.Min(mb, md));

                    if (minCF < this.symptoms[i].symptomCF)
                        minCF = this.symptoms[i].symptomCF;
                }
            }

            this.setfinalScore(minCF);
        }

        double calculateMB(double pH,double pHE) // ph prior ----- phe liklihood
        {
            if (pH == 1)
                return 1;
            else
            {
                var r = ((Math.Max(pHE, pH) - pH) / ((Math.Max(1, 0)) - pH));
                return r;
            }
        }

        double calculateMD(double pH, double pHE)
        {
            if (pH == 0)
                return 1;
            else
            {
                var r = ((Math.Min(pHE, pH) - pH) / ((Math.Min(1, 0)) - pH));
                return r;
            }
        }

    }
}
