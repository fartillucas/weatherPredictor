using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Windows.Markup;

namespace consoletester.services
{
    public class DbConnector
    {
        public string StationName { get; set; }
        string ConnectionString = "Server=akctest01.database.windows.net;Database=akctestdb01;uid=DMIuserLogin;password=DmiLogin34!DK;Trusted_Connection=false";

        public void GetAllFromParameters()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Select * from parameters", conn);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        while (reader.Read())
                        {
                            var number = reader["Number"].ToString();
                            var desc = reader["Description"].ToString();
                            var Parid = reader["ParameterId"].ToString();
                            var unit = reader["Unit"].ToString();

                            Console.WriteLine(number + " " + desc + " " + Parid + " " + unit);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void GetTemperatureFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Observed, Value from Observations where StationId=06079 AND Isvalid=1 AND ParameterId='temp_mean_past1h' Order by Observed", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\farti\Source\Repos\weatherPredictor\actualdata.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{ 0}, {1}",
                                    reader["Observed"], reader["Value"]);
                    }

                }




            }
        }
        public void GetAlarmsFromAlarms()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select AlarmCount, DateKey from Alarms where StationId=06079 AND Isvalid=1 Order by StationId", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\farti\Source\Repos\weatherPredictor\AlarmData.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{ 0}, {1}, {2}",
                                    reader["DateKey"], reader["AlarmCount"], reader["StationId"]);
                    }

                }




            }
        }
        public void SaveCustomDataset()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Observed, max(case when Observations.ParameterId= 'temp_mean_past1h' then Observations.Value end) as TempMean, max(case when Observations.ParameterId= 'humidity_past1h' then Observations.Value end) as Humidty, max(case when Observations.ParameterId= 'pressure_at_sea' then Observations.Value end) as Pressure, max(case when Observations.ParameterId= 'temp_min_past1h' then Observations.Value end) as TempMin,	max(case when Observations.ParameterId= 'temp_max_past1h' then Observations.Value end) as TempMax from Observations	where StationId=06123 Group by Observed	order by Observed", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();

                    using (StreamWriter writer = new StreamWriter(@"C:\Users\farti\Source\Repos\weatherPredictor\HourlyValues.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}",
                                    reader["Observed"], reader["TempMean"], reader["Humidty"], reader["Pressure"], reader["TempMin"], reader["TempMax"]);
                    }

                }




            }
        }
        public void SavedailyDatasetWithDate()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT Datekey, AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean, AVG(ALL case when Observations.ParameterId= 'humidity_past1h' then Observations.Value end) as Humidty, AVG(ALL case when Observations.ParameterId= 'pressure_at_sea' then Observations.Value end) as Pressure, min(ALL case when Observations.ParameterId= 'temp_min_past1h' then Observations.Value end) as TempMin, max(ALL case when Observations.ParameterId= 'temp_max_past1h' then Observations.Value end) as TempMax FROM Observations where StationId=06123 Group by DateKey order by Datekey", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();

                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Asmus\Source\Repos\fartillucas\weatherPredictor\dailyValues.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}",
                                    reader["Datekey"], reader["TempMean"], reader["Humidty"], reader["Pressure"], reader["TempMin"], reader["TempMax"]);
                    }

                }




            }
        }
        public void SavedailyDataset()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean, AVG(ALL case when Observations.ParameterId= 'humidity_past1h' then Observations.Value end) as Humidty, AVG(ALL case when Observations.ParameterId= 'pressure_at_sea' then Observations.Value end) as Pressure, min(ALL case when Observations.ParameterId= 'temp_min_past1h' then Observations.Value end) as TempMin, max(ALL case when Observations.ParameterId= 'temp_max_past1h' then Observations.Value end) as TempMax FROM Observations where StationId=06123 Group by DateKey order by Datekey", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();

                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Asmus\Source\Repos\fartillucas\weatherPredictor\dailyValues2.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{0}, {1}, {2}, {3}, {4}",
                                     reader["TempMean"], reader["Humidty"], reader["Pressure"], reader["TempMin"], reader["TempMax"]);
                    }

                }




            }
        }
        public void SaveAlarmDataset()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT MAX(Alarm.Alarms) as Alarms, AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean, AVG(ALL case when Observations.ParameterId= 'humidity_past1h' then Observations.Value end) as Humidty, AVG(ALL case when Observations.ParameterId= 'pressure_at_sea' then Observations.Value end) as Pressure, min(ALL case when Observations.ParameterId= 'temp_min_past1h' then Observations.Value end) as TempMin, max(ALL case when Observations.ParameterId= 'temp_max_past1h' then Observations.Value end) as TempMax FROM Observations JOIN (SELECT Alarms.DateKey, SUM(ALL Alarms.AlarmCount) as Alarms From Alarms Where Alarms.StationId = 06123 Group by Alarms.DateKey) as Alarm ON Alarm.DateKey = Observations.DateKey Where Observations.StationId = 06123 Group by Alarm.DateKey order by Alarm.Datekey", conn);

                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();

                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Asmus\Source\Repos\fartillucas\weatherPredictor\AlarmValues.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}",
                                     reader["Alarms"], reader["TempMean"], reader["Humidty"], reader["Pressure"], reader["TempMin"], reader["TempMax"]);
                    }

                }




            }
        }
        public void GetHumidityFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Observed, Value from Observations where StationId=06079 AND Isvalid=1 AND ParameterId='humidity_past1h' Order by Observed", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Michael\source\repos\consoletester\actualhumdata.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{0}, {1}",
                                    reader["Observed"], reader["Value"]);
                    }

                }




            }
        }
        public void GetPressureFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Observed, Value from Observations where StationId=06079 AND Isvalid=1 AND ParameterId='pressure_at_sea' Order by Observed", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Michael\source\repos\consoletester\actualPresdata.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{0}, {1}",
                                    reader["Observed"], reader["Value"]);
                    }

                }




            }
        }
        public void GetMinTempFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Observed, Value from Observations where StationId=06079 AND Isvalid=1 AND ParameterId='temp_min_past1h' Order by Observed", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Michael\source\repos\consoletester\actualMinsdata.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{0}, {1}",
                                    reader["Observed"], reader["Value"]);
                    }

                }




            }
        }

        public void GetMaxTempFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Observed, Value from Observations where StationId=06079 AND Isvalid=1 AND ParameterId='temp_max_past1h' Order by Observed", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Michael\source\repos\consoletester\actualMaxdata.csv"))
                    {
                        while (reader.Read())

                            writer.WriteLine("{0}, {1}",
                                    reader["Observed"], reader["Value"]);
                    }

                }




            }
        }


    }
}
