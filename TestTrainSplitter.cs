using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using CsvHelper;

namespace consoletester
{
    class TestTrainSplitter
    {

        public string[] Set;
        public List<string> testList =new List<string>();
        
        public void TestReader()
        {
           Set = File.ReadAllLines(@"C:\Users\Michael\PycharmProjects\testExample\daily-minimum-temperatures.csv");
        }
        public void TestSplitter()
        {
           for(int i = 0; i< Set.Length - 7; i++)
            {
                testList.Add(Set[i]);
            }
        }
        public void SaveToCsv()
        {
           using (var writer = new StreamWriter(@"C:\Users\Michael\PycharmProjects\testExample\trainData.csv"))
           using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                
                foreach (var value in testList)
                {
                    csv.WriteField(value);
                    csv.NextRecord();
                    // csv.WriteRecords(testList.ToArray());
                    writer.Flush();

                }
            }
        }
    }
}
    
