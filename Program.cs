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

            model ml = new model();
            ml.testMethod();

            //DbConnector dc = new DbConnector();
            //dc.GetTemperatureFromObservations();
        }
    }
}