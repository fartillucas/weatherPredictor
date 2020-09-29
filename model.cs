using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Numpy;
using System.IO;
using CsvHelper;
using System.Globalization;

namespace consoletester
{
    public class model
    {
        public string[] dataSet;
        public int interval = 1;
        public dynamic tempSet;
        List<double> diff = new List<double>();
        List<double> tempList = new List<double>();
        public double doubleNumber;

        public List<double> difference() {
            int header = 1;
            dataSet = File.ReadAllLines(@"C:\Users\Michael\PycharmProjects\testExample\trainData.csv");
            tempSet = (from temp in dataSet
                       let data = temp.Split(',')
                       select new
                       {   
                           Temp = data[1],


                       });
            foreach (var temp in tempSet)
            {
                if (header == 1)
                {   
                    header++;
                    continue;
                }
                tempList.Add(double.Parse(temp.Temp.Trim('"','\'')));

                Console.WriteLine(double.Parse(temp.Temp.Trim(new char[] {'"'}), NumberFormatInfo.InvariantInfo));

                //Console.WriteLine(temp.Temp.Trim);
            }



                for (int i = 365; i < tempList.Count; i++) 
            {
                double value = tempList[i] - tempList[i - interval];
                diff.Add(value);
                //Console.WriteLine(value);
            }
                        
            return diff;

         }
        public void testMethod()
        {
            model md = new model();
            List<double> testList = md.difference();
            using (var writer = new StreamWriter(@"C:\Users\Michael\PycharmProjects\testExample\shit2.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))

                foreach (var value in testList)
                {
                    csv.WriteField(value);
                    csv.NextRecord();
                    // csv.WriteRecords(testList.ToArray());
                    writer.Flush();
                }
        }
    }
    }
   

