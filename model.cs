using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using Extreme.Statistics.TimeSeriesAnalysis;
using Extreme.Mathematics;
using Extreme.DataAnalysis;
using consoletester.services;

namespace consoletester
{
    public class model
    {
        public string[] dataSet;
        public dynamic temporarySet;
        List<decimal> diff = new List<decimal>();
        List<decimal> temporaryList = new List<decimal>();

        //getter method til dataset
        public List<decimal> getTempList()
        {
            return temporaryList;
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
            temporarySet = (from tempMean in dataSet
                            let data = tempMean.Split(',')
                            select new
                            {
                                Temp = data[1]
                            });
            foreach (var temp in temporarySet)
            {
                if (string.Equals(temp.Temp, "Observed") || string.Equals(temp.Temp, "DateKey") || string.Equals(temp.Temp, "TempMean") || string.Equals(temp.Temp, "Alarms") || string.Equals(temp.Temp, "TempMin") || string.Equals(temp.Temp, "TempMax") || string.Equals(temp.Temp, "Humidity") || string.Equals(temp.Temp, "Pressure"))
                {
                    continue;
                }
                if (string.Equals(temp.Temp, ""))
                {
                    continue;
                }
                temporaryList.Add(decimal.Parse(temp.Temp.Trim('"'), NumberFormatInfo.InvariantInfo));
            }


            foreach (var i in Enumerable.Range(interval, temporaryList.Count - interval))
            {
                var value = temporaryList[i] - temporaryList[i - interval];
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

        public string[] SaveMultiStepForecast(double[] dataSet, Vector<double> multipleForecast, int interval)
        {
            string[] returnArray = new string[multipleForecast.Length];
            for (int i = 0; i < multipleForecast.Length; i++)
            {
                double forecasts = inverse(dataSet, multipleForecast[i], interval);
                dataSet.Append(forecasts);
                //Console.WriteLine("day " + (i + 1) + ": " + forecasts);
                returnArray[i] = ($"{forecasts}");
                interval--;
            }
            return returnArray;
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
        public string[] CreateArimaModelWithForecast(string[] data, int daysToForecast, int p, int d, int q, string StationId)
        {
            model md = new model();
            if (data.Length <= 365)
            {
                DbConnector dbConnector = new DbConnector();
                if (String.Equals(data[0], "DateKey,Pressure"))
                {
                    data = dbConnector.GetDailyPressureFromObservationsByRegionId(dbConnector.GetRegionIdFromStationId(StationId));
                }
                if (String.Equals(data[0], "DateKey,TempMean"))
                {
                    data = dbConnector.GetDailyMeanTemperatureReadingByRegionId(dbConnector.GetRegionIdFromStationId(StationId));
                }
                if (String.Equals(data[0], "DateKey,TempMin"))
                {
                    data = dbConnector.GetDailyMinTempFromObservationsByRegionId(dbConnector.GetRegionIdFromStationId(StationId));
                }
                if (String.Equals(data[0], "DateKey,TempMax"))
                {
                    data = dbConnector.GetDailyMaxTempFromObservationsByRegionId(dbConnector.GetRegionIdFromStationId(StationId));
                }
                if (String.Equals(data[0], "DateKey,Humidity"))
                {
                    data = dbConnector.GetDailyHumidityFromObservationsByRegionId(dbConnector.GetRegionIdFromStationId(StationId));
                }
            }
            List<decimal> testList = md.difference(data, 365);
            double[] myArray = md.convertDecimalListToDoubleArray(testList);
            List<double> templist2 = md.convertDecimalListToDobuleList(md.getTempList());

            ArimaModel arimam = new ArimaModel(myArray, p, d, q);
            arimam.EstimateMean = true;
            arimam.Compute();

            Vector<double> multipleForecast = arimam.Forecast(daysToForecast);
            double[] localTempMeanList = md.convertDecimalListToDoubleArray(md.getTempList());
            double[] mytemparray = (from x in localTempMeanList select x).ToArray();

            return md.SaveMultiStepForecast(mytemparray, multipleForecast, 365);
        }


        public void testMethod(string[] data, int p, int q, int d)
        {
            model md = new model();
            List<decimal> testList = md.difference(data, 365);
            double[] myArray = md.convertDecimalListToDoubleArray(testList);

            List<double> templist2 = md.convertDecimalListToDobuleList(md.getTempList());
            //double[] mytemparray = templist2.ToArray();

            ArimaModel arimam = new ArimaModel(myArray, p, d, q);
            arimam.EstimateMean = true;
            arimam.Fit();
            //md.SummarizeArima(arimam);

            double forecast = arimam.Forecast();


            Vector<double> multipleForecast = arimam.Forecast(1);
            double[] localTempMeanList = md.convertDecimalListToDoubleArray(md.getTempList());
            double[] mytemparray = (from x in localTempMeanList select x).ToArray();

            int days = 365;
            md.ShowMultiStepForecast(mytemparray, multipleForecast, days);


            forecast = inverse(mytemparray, forecast, 365);
            Console.WriteLine(forecast + " forecast test");

            Vector<double> nextValue = arimam.Forecast(myArray.Length);
            //for (int i = 0; i < myArray.Length; i++)
            //Console.WriteLine(myArray[i]);

            //Console.WriteLine(nextValue + " Test");



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


