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
using System.Runtime.InteropServices;

namespace consoletester
{
    public class model
    {
        public string[] dataSet;
        public int interval = 1;
        public dynamic tempSet;
        List<decimal> diff = new List<decimal>();
        List<decimal> tempList = new List<decimal>();

        public List<decimal> getTempList()
        {
            return tempList;
        }


        public List<double> converListToDobule(List<decimal> listToBeConverted)
        {
            List<double> returnList = new List<double>();
            for (int i = 0; i < listToBeConverted.Count; i++)
            {
                returnList.Add(Decimal.ToDouble(listToBeConverted[i]));
            }
            return returnList;
        }

        public List<decimal> difference(int interval)
        {
            int header = 1;

            dataSet = File.ReadAllLines(@"C:\Users\farti\Source\Repos\weatherPredictor\trainData.csv");
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
            }


            foreach (var i in Enumerable.Range(interval, tempList.Count - interval))
            {
                var value = tempList[i] - tempList[i - interval];
                diff.Add(value);
                //Console.WriteLine(value);
            }
            return diff;
        }

        public double inverse(double[] history, double yhat, int interval = 1)
        {

            return yhat + history[^interval];
        }


        [Obsolete]
        public void testMethod()
        {
            model md = new model();
            List<decimal> testList = md.difference(365);
            List<double> inputList = md.converListToDobule(testList);
            double[] myArray = inputList.ToArray();

            List<double> templist2 = md.converListToDobule(md.getTempList());
            //double[] mytemparray = templist2.ToArray();

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

            double forecast = arimam.Forecast();

            Vector<double> multipleForecast = arimam.Forecast(7);
            double[] localTempList = md.converListToDobule(md.getTempList()).ToArray();
            double[] mytemparray = (from x in localTempList select x).ToArray();

            Console.WriteLine(mytemparray.Length);
            int days = 365;
            for (int i = 0; i < multipleForecast.Length; i++)
            {
                Console.WriteLine(multipleForecast[i]);
                double forecasts = inverse(mytemparray, multipleForecast[i], days);
                mytemparray.Append(forecasts);
                Console.WriteLine(forecasts);
                days--;
            }



            //forecast = inverse(mytemparray, forecast, 365);
            //Console.WriteLine(forecast + " forecast test");

            //Vector<double> nextValue = arimam.Forecast(myArray.Length);

            //Console.WriteLine(nextValue + " Test");



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


