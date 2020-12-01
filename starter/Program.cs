using consoletester.aquaintance;
using consoletester.data;
using consoletester.logic;
using System;
using System.Collections.Generic;


namespace consoletester.starter
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {


            IDatabaseFacade dc = databaseFacade.getInstance();
            IModelFacade ml = modelFacade.getInstance();
            List<string> StationIds = dc.GetAllStationIds();


            for (int i = 0; i < StationIds.Count; i++)
            {
                string[] pressure = ml.CreateArimaModelWithForecast(dc.GetDailyPressureFromObservations(StationIds[i]), 7, 2, 0, 0, StationIds[i]);
                string[] tempmean = ml.CreateArimaModelWithForecast(dc.GetDailyMeanTemperatureReading(StationIds[i]), 7, 4, 0, 1, StationIds[i]);
                string[] humidity = ml.CreateArimaModelWithForecast(dc.GetDailyHumidityFromObservations(StationIds[i]), 7, 4, 0, 2, StationIds[i]);
                string[] minTemp = ml.CreateArimaModelWithForecast(dc.GetDailyMinTempFromObservations(StationIds[i]), 7, 4, 0, 2, StationIds[i]);
                string[] maxTemp = ml.CreateArimaModelWithForecast(dc.GetDailyMaxTempFromObservations(StationIds[i]), 7, 3, 0, 2, StationIds[i]);

                for (int j = 0; j < pressure.Length; j++)
                {
                    dc.SaveDaliyArimaForecast(StationIds[i], Convert.ToDouble(tempmean[j]), Convert.ToInt32(Convert.ToDouble(humidity[j])), Convert.ToDouble(pressure[j]), (float)Convert.ToDouble(minTemp[j]), Convert.ToDouble(maxTemp[j]), j + 1);
                }

            }
            //Console.WriteLine("TempMean invalid model");
            //ml.testMethod(dc.GetDailyMeanTemperatureReading("06123"), 3, 0, 2); //not valid
            //Console.WriteLine("TempMean valid model");
            //ml.testMethod(dc.GetDailyMeanTemperatureReading("06123"), 4, 0, 1); // valid 
            //Console.WriteLine("TempMin invalid model");
            //ml.testMethod(dc.GetDailyMinTempFromObservations("06123"), 2, 0, 2); //not valid
            //Console.WriteLine("TempMin valid model");
            //ml.testMethod(dc.GetDailyMinTempFromObservations("06123"), 5, 0, 2); // valid
            //Console.WriteLine("TempMax invalid model");
            //ml.testMethod(dc.GetDailyMaxTempFromObservations("06123"), 3, 0, 2); //not valid
            //Console.WriteLine("TempMax valid model");
            //ml.testMethod(dc.GetDailyMaxTempFromObservations("06123"), 3, 0, 2); // valid
            //Console.WriteLine("Humidity invalid model");
            //ml.testMethod(dc.GetDailyHumidityFromObservations("06123"), 6, 0, 2); //not valid
            //Console.WriteLine("Humidity valid model");
            //ml.testMethod(dc.GetDailyHumidityFromObservations("06123"), 4, 0, 2); // valid 
            //Console.WriteLine("Pressure invalid model");
            //ml.testMethod(dc.GetDailyPressureFromObservations("06123"), 1, 0, 1); //not valid
            //Console.WriteLine("Pressure valid model");
            //ml.testMethod(dc.GetDailyPressureFromObservations("06123"), 2, 0, 0); // valid

        }


    }
}