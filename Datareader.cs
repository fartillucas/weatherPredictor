
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace consoletester
{
    public class Datareader
    {
        public string[] weatherData;
        public dynamic weather;

        public void readData() {

            weatherData = File.ReadAllLines(@"C:\Users\Michael\PycharmProjects\testExample\daily-minimum-temperatures.csv");

         }
          public void splitData()
        {
             weather = (from temp in weatherData
                                     let data = temp.Split(',')
                                     select new
                                     {
                                         Date = data[0],
                                         Temp = data[1],

                                     });
            foreach (var temp in weather)
                Console.WriteLine(temp.Date + "|" + temp.Temp);
            Console.ReadLine();
            {
            }
        }

        }
    }


