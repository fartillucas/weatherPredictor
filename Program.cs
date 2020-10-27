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
            string[] pressure = ml.CreateArimaModelWithForecast(dc.GetDailyPressureFromObservations(), 7, 1, 0, 1);
            for (int i = 0; i < pressure.Length; i++)
            {
            }
            string[] tempmean = ml.CreateArimaModelWithForecast(dc.GetDailyMeanTemperatureReading(), 7, 1, 0, 1);
            string[] humidity = ml.CreateArimaModelWithForecast(dc.GetDailyHumidityFromObservations(), 7, 1, 0, 1);
            string[] minTemp = ml.CreateArimaModelWithForecast(dc.GetDailyMinTempFromObservations(), 7, 1, 0, 1);
            string[] maxTemp = ml.CreateArimaModelWithForecast(dc.GetDailyMaxTempFromObservations(), 7, 1, 0, 1);

            //  ml.testMethod(dc.GetDailyMinTempFromObservations());
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