using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Numpy;
using System.IO;

namespace consoletester
{
    public class model
    {
        public string[] dataSet;
        public int interval = 1;
        public dynamic tempSet;
        List<double> diff = new List<double>();

        public List<double> difference() {
            dataSet = File.ReadAllLines(@"C:\Users\Michael\PycharmProjects\testExample\trainData.csv");
            tempSet = (from temp in dataSet
                       let data = temp.Split(',')
                       select new
                       {                          
                           Temp = data[1],

                       });
            for (int i = 0; i <= tempSet.Length(); i++) 
            {
                double value = tempSet[i] - tempSet[i - interval];
                diff.Add(value);
            }
                        
            return diff;

         }
        public void testMethod()
        {
            model md = new model();
            List<double> testList = md.difference();

            foreach (int item in testList)
            {
                Console.WriteLine(item);
            }
        }
    }
   

}