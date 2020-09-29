using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Numpy;
using System.IO;
using CsvHelper;
using System.Globalization;
using Extreme.DataAnalysis;
using Extreme.Mathematics;
using Extreme.Statistics;
using Extreme.Statistics.TimeSeriesAnalysis;


namespace consoletester
{
    public class model
    {
        public string[] dataSet;
        public int interval = 1;
        public dynamic tempSet;
        List<decimal> diff = new List<decimal>();
        List<decimal> tempList = new List<decimal>();

            public List<double> converListToDobule(List<decimal> listToBeConverted)
            {
            List<double> returnList = new List<double>();
                for(int i=0; i<listToBeConverted.Count; i++)
                {
                returnList.Add(Decimal.ToDouble(listToBeConverted[i]));
                }
              return returnList;
            }

             public List<decimal> difference(int interval) {
            int header = 1;
            
            dataSet = File.ReadAllLines(@"C:\Users\Asmus\Source\Repos\fartillucas\weatherPredictor\trainData.csv");
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
                tempList.Add(decimal.Parse(temp.Temp.Trim('"'), NumberFormatInfo.InvariantInfo));

                //Console.WriteLine(decimal.Parse(temp.Temp.Trim(new char[] {'"'}), NumberFormatInfo.InvariantInfo));

                //Console.WriteLine(tempList.Count());
            }

                
            foreach (var i in Enumerable.Range(interval, tempList.Count - interval))
            {
                var value = tempList[i] - tempList[i - interval];
                diff.Add(value);
                Console.WriteLine(value);
            }
            return diff;
         }


        //Fit the model



        public void testMethod()
        {
            model md = new model();
            List<decimal> testList = md.difference(365);
            using (var writer = new StreamWriter(@"C:\Users\Asmus\Source\Repos\fartillucas\weatherPredictor\shit3.csv"))
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
   

