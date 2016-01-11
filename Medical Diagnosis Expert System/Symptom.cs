using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Diagnosis_Expert_System
{
    public class Symptom
    {
        public string symptomName;
        public double symptomCF;
        public double expertCF;
        double userCF;

        public Symptom() { }
        public Symptom(string name, double expertCF)
        {
            this.symptomName = name;
            this.expertCF = expertCF;
        }

        public void setUserCF(double userCF)
        {
            this.userCF=userCF;
        }
        
        public double getUserCF()
        {
            return this.userCF;
        }
        
    }
}
