using WeatherPredictorComponent.aquaintance;
using WeatherPredictorComponent.data;
using WeatherPredictorComponent.logic;
using System;
using System.Collections.Generic;
using System.Threading;

namespace WeatherPredictorComponent.starter
{
    class starter
    {
        [Obsolete]
        static void Main(string[] args)
        {


            IDatabaseFacade dc = databaseFacade.getInstance();
            IModelFacade ml = ModelFacade.getInstance();
            List<string> StationIds = dc.GetAllStationIds();

            while (true)
            {
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
                Thread.Sleep(86400000);
            }



        }


    }
}