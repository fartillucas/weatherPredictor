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
            ml.testMethod(dc.GetDailyMeanTemperatureReading());
            ml.testMethod(dc.GetDailyMinTempFromObservations());



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