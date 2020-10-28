using consoletester.services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consoletester
{
    class Program
    {
        [Obsolete]
        static void Main(string[] args)
        {
            //Datareader dr = new Datareader();
            //dr.readData();
            //dr.splitData();

            //TestTrainSplitter tts = new TestTrainSplitter();
            //{
            //    tts.TestReader();
            //    tts.TestSplitter();
            //    tts.SaveToCsv();
            //}
            DbConnector dc = new DbConnector();
            model ml = new model();
            //ml.testMethod(dc.GetDailyPressureFromObservations());
            List<string> StationIds = dc.GetAllStationIds();


            for (int i = 0; i < StationIds.Count; i++)
            {
                string[] pressure = ml.CreateArimaModelWithForecast(dc.GetDailyPressureFromObservations(StationIds[i]), 7, 1, 0, 1);
                string[] tempmean = ml.CreateArimaModelWithForecast(dc.GetDailyMeanTemperatureReading(StationIds[i]), 7, 1, 0, 1);
                string[] humidity = ml.CreateArimaModelWithForecast(dc.GetDailyHumidityFromObservations(StationIds[i]), 7, 1, 0, 1);
                string[] minTemp = ml.CreateArimaModelWithForecast(dc.GetDailyMinTempFromObservations(StationIds[i]), 7, 1, 0, 1);
                string[] maxTemp = ml.CreateArimaModelWithForecast(dc.GetDailyMaxTempFromObservations(StationIds[i]), 7, 1, 0, 1);

                for (int j = 0; j < pressure.Length; j++)
                {
                    dc.SaveDaliyArimaForecast(StationIds[i], Convert.ToDouble(tempmean[j]), Convert.ToInt32(Convert.ToDouble(humidity[j])), Convert.ToDouble(pressure[j]), Convert.ToDouble(minTemp[j]), Convert.ToDouble(maxTemp[j]), j + 1);
                }

            }


            //ml.testMethod(dc.GetDailyMinTempFromObservations());
            //dc.saveLocalDataset();
            //dc.GetPressureFromObservations();
            //dc.GetMaxTempFromObservations();
            //dc.GetMinTempFromObservations();
            //dc.SavedailyDatasetWithDate();
            //dc.SavedailyDataset();
            //dc.SaveAlarmDataset();
            //dc.GetHumidityFromObservations();
        }
    }
}