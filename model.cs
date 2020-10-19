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
        public dynamic tempSet;
        List<decimal> diff = new List<decimal>();
        List<decimal> tempList = new List<decimal>();

        //getter method til dataset
        public List<decimal> getTempList()
        {
            return tempList;
        }

        //method til at konventere dataset fra decimal til double list
        public List<double> convertDecimalListToDobuleList(List<decimal> listToBeConverted)
        {
            List<double> returnList = new List<double>();
            for (int i = 0; i < listToBeConverted.Count; i++)
            {
                returnList.Add(Decimal.ToDouble(listToBeConverted[i]));
            }
            return returnList;
        }

        public double[] convertDecimalListToDoubleArray(List<decimal> listToBeConverted)
        {
            return convertDecimalListToDobuleList(listToBeConverted).ToArray();
        }


        public List<decimal> difference(string[] dataInput, int interval = 1)
        {
            dataSet = dataInput;
            tempSet = (from temp in dataSet
                       let data = temp.Split(',')
                       select new
                       {
                           Temp = data[1]
                       });
            foreach (var temp in tempSet)
            {
                if (string.Equals(temp.Temp, "Observed") || string.Equals(temp.Temp, "DateKey") || string.Equals(temp.Temp, "TempMean") || string.Equals(temp.Temp, "Alarms"))
                {
                    continue;
                }
                if (string.Equals(temp.Temp, ""))
                {
                    continue;
                }
                tempList.Add(decimal.Parse(temp.Temp.Trim('"'), NumberFormatInfo.InvariantInfo));
            }


            foreach (var i in Enumerable.Range(interval, tempList.Count - interval))
            {
                var value = tempList[i] - tempList[i - interval];
                diff.Add(value);
            }
            return diff;
        }

        public double inverse(double[] history, double yhat, int interval = 1)
        {
            return yhat + history[^interval];
        }

        public void ShowMultiStepForecast(double[] dataSet, Vector<double> multipleForecast, int interval)
        {
            for (int i = 0; i < multipleForecast.Length; i++)
            {
                double forecasts = inverse(dataSet, multipleForecast[i], interval);
                dataSet.Append(forecasts);
                Console.WriteLine("day " + (i + 1) + ": " + forecasts);
                interval--;
            }
        }

        public void SummarizeArima(ArimaModel arimam)
        {
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
        }

        [Obsolete]
        public void testMethod(string[] data)
        {
            model md = new model();
            List<decimal> testList = md.difference(data, 365);
            double[] myArray = md.convertDecimalListToDoubleArray(testList);

            List<double> templist2 = md.convertDecimalListToDobuleList(md.getTempList());
            //double[] mytemparray = templist2.ToArray();

            ArimaModel arimam = new ArimaModel(myArray, 3, 0, 2);
            arimam.EstimateMean = true;
            arimam.Compute();
            md.SummarizeArima(arimam);

            double forecast = arimam.Forecast();


            Vector<double> multipleForecast = arimam.Forecast(7);
            double[] localTempList = md.convertDecimalListToDoubleArray(md.getTempList());
            double[] mytemparray = (from x in localTempList select x).ToArray();

            int days = 365;
            md.ShowMultiStepForecast(mytemparray, multipleForecast, days);


            forecast = inverse(mytemparray, forecast, 365);
            Console.WriteLine(forecast + " forecast test");

            Vector<double> nextValue = arimam.Forecast(myArray.Length);
            // for (int i = 0; i < myArray.Length; i++)
            //Console.WriteLine(myArray[i]);

            Console.WriteLine(nextValue + " Test");



            //using (var writer = new StreamWriter(@"C:\Users\Michael\source\repos\consoletester\shit3.csv"))
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


