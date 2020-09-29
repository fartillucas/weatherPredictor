using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using CsvHelper;
using System.Globalization;
using Extreme.Statistics.TimeSeriesAnalysis;
using Extreme.Mathematics;
using Extreme.Statistics;
using Extreme.DataAnalysis;

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
                //Console.WriteLine(value);
            }
            return diff;
         }

        [Obsolete]
        public void testMethod()
        {
            model md = new model();
            List<decimal> testList = md.difference(365);
            List<double> inputList = md.converListToDobule(testList);

            for(int i=0; i<inputList.Count; i++)
            {
                Console.WriteLine(inputList[i]);
            }

            double[] myArray = inputList.ToArray();
            //for (int i = 0; i < myArray.Length; i++)
            //{
            //    Console.WriteLine(myArray[i]);
            //}
            ArimaModel arimam = new ArimaModel(myArray, 7, 0, 1);
            arimam.EstimateMean = true;
            arimam.Compute();


            Console.WriteLine("Variable              Value    Std.Error  t-stat  p-Value");
            foreach (Parameter parameter in arimam.Parameters)
                // Parameter objects have the following properties:
                Console.WriteLine("{0,-20}{1,10:F5}{2,10:F5}{3,8:F2} {4,7:F4}",
                    // Name, usually the name of the variable:
                    parameter.Name,
                    // Estimated value of the parameter:
                    parameter.Value,
                    // Standard error:
                    parameter.StandardError,
                    // The value of the t statistic for the hypothesis that the parameter
                    // is zero.
                    parameter.Statistic,
                    // Probability corresponding to the t statistic.
                    parameter.PValue);

            Console.WriteLine("Error variance: {0:F4}", arimam.ErrorVariance);

            Console.WriteLine("Log-likelihood: {0:F4}", arimam.LogLikelihood);
            Console.WriteLine("AIC: {0:F5}", arimam.GetAkaikeInformationCriterion());
            Console.WriteLine("BIC: " + arimam.GetBayesianInformationCriterion());

            double nextValue = arimam.Forecast();

            Console.WriteLine(nextValue + "PENIS");

            //using (var writer = new StreamWriter(@"C:\Users\Asmus\Source\Repos\fartillucas\weatherPredictor\shit3.csv"))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))

            //    foreach (var value in testList)
            //    {
            //        csv.WriteField(value);
            //        csv.NextRecord();
            //        // csv.WriteRecords(testList.ToArray());
            //        writer.Flush();
            //    }
        }
    }
    }
   

