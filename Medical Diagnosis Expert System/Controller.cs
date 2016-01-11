using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medical_Diagnosis_Expert_System
{
    public class Controller
    {
        public List<Disease> Diseases;
        public List<string> mainSymptoms;
        public List<string> commonSymptoms;
        public List<string> symptomsSheet;
        public Dictionary<string, double> allSymptoms;
        public List<KeyValuePair<string, string>> result;
        public Controller()
        {
            this.Diseases = new List<Disease>();
            this.mainSymptoms = new List<string>();
            this.commonSymptoms = new List<string>();
            this.symptomsSheet = new List<string>();
            this.allSymptoms = new Dictionary<string, double>();
            this.result = new List<KeyValuePair<string, string>>();
        }
        public void fillData(string filePath)
        {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);


            // Reading from a OpenXml Excel file (*.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);



            excelReader.IsFirstRowAsColumnNames = true;

            DataSet result = excelReader.AsDataSet();

            bool isDeseasesSet = false;
            while (excelReader.Read())
            {
                //Set Diseases
                if (!isDeseasesSet)
                {

                    for (int i = 1; i < excelReader.FieldCount; i++)
                    {
                        this.Diseases.Add(new Disease(excelReader.GetValue(i).ToString()));
                    }
                    isDeseasesSet = true;
                    continue;
                }

                //set symptoms
                int isCommon = 0;
                for (int j = 0; j < excelReader.FieldCount; j++)
                {
                    if (excelReader.GetValue(j) != null)
                        isCommon++;
                }
                if (isCommon > 2)
                    commonSymptoms.Add(excelReader.GetValue(0).ToString());
                else
                    symptomsSheet.Add(excelReader.GetValue(0).ToString());


                string sym = excelReader.GetValue(0).ToString();
                for (int i = 1; i < excelReader.FieldCount; i++)
                {

                    if (excelReader.GetValue(i) != null)
                    {

                        var s = excelReader.GetValue(i).ToString();
                        double symScore;
                        if (s.Contains('*'))
                        {
                            symScore = Convert.ToDouble(s.Substring(0, s.Length - 1));
                            this.Diseases[i - 1].setMainSymptom(sym, symScore);
                            continue;
                        }
                        else
                        {
                            symScore = Convert.ToDouble(s);
                        }

                        this.Diseases[i - 1].symptoms.Add(new Symptom(sym, symScore));
                    }

                }

            }
            fillMainSyms();

            for (int i = 0; i < this.mainSymptoms.Count;i++ )
            {
                this.symptomsSheet.Remove(this.mainSymptoms[i]);
            }

                excelReader.Close();



        }

        public string askQuestion(string symp)
        {

            return "Do you have/feel " + symp + " ?";
        }

        public void DistriputeUserCF()
        {
            for (int i = 0; i < this.Diseases.Count; i++)
            {
                if (this.Diseases[i].mainSymptom != null)
                    this.Diseases[i].mainSymptom.setUserCF(this.allSymptoms[this.Diseases[i].mainSymptom.symptomName]);

                for (int j = 0; j < this.Diseases[i].symptoms.Count; j++)
                {
                    if (this.allSymptoms.ContainsKey(this.Diseases[i].symptoms[j].symptomName))
                        this.Diseases[i].symptoms[j].setUserCF(this.allSymptoms[this.Diseases[i].symptoms[j].symptomName]);
                    else
                        this.Diseases[i].symptoms[j].setUserCF(-99.0);
                }

            }
        }

        public int isMinSymptom(string symp, List<Disease> d, Dictionary<string, double> symptomsScore)
        {
            int result = -1;
            for (int i = 0; i < d.Count; i++)
            {
                for (int j = 0; j < d[i].symptoms.Count; j++)
                {
                    if (symp == d[i].symptoms[j].symptomName)
                    {
                        if (d[i].mainSymptom != null)
                        {
                            if (symptomsScore[d[i].mainSymptom.symptomName] > -1)
                            {
                                result = 1;
                                break;

                            }
                            else
                            {
                                result = 0;
                                break;
                            }
                        }
                        else
                            result = 1; // no main symptom
                    }

                }
                if (result != -1)
                    break;
            }
            return result;
        }

        public void fillMainSyms()
        {
            for (int i = 0; i < this.Diseases.Count; i++)
            {
                if(this.Diseases[i].mainSymptom!=null)
                    this.mainSymptoms.Add(Diseases[i].mainSymptom.symptomName);
            }
        }

        public void removeUnusedSymptoms(Dictionary<string, double> symptomsScore)
        {
            for(int i=0;i<Diseases.Count;i++)
            {
                if (Diseases[i].mainSymptom != null)
                {
                    if (symptomsScore[Diseases[i].mainSymptom.symptomName] == (-1.0))
                    {
                        for (int j = 0; j < Diseases[i].symptoms.Count; j++)
                        {
                            this.symptomsSheet.Remove(Diseases[i].symptoms[j].symptomName);
                        }
                    }
                }
            }
        }

        public void output()
        {
            List<KeyValuePair<string,string>> res = new List<KeyValuePair<string,string>>();
            for(int i=0;i<this.Diseases.Count;i++)
            {
                if(this.Diseases[i].finalScore+0.5>0) //0.5 for scaling
                {
                    res.Add(new KeyValuePair<string, string>(this.Diseases[i].name, ((this.Diseases[i].finalScore)+0.5*100).ToString()));
                }
            }
           this.result=res;
        }
    }
}
