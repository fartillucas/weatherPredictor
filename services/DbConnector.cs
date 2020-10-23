using CsvHelper;
using Extreme.Mathematics.LinearAlgebra;
using Extreme.Statistics;
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
        public string[] rawdata;
        string ConnectionString = "Server=akctest01.database.windows.net;Database=akctestdb01;uid=DMIuserLogin;password=DmiLogin34!DK;Trusted_Connection=false";

        public string[] SavedailyDataset()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand("SELECT AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean, AVG(ALL case when Observations.ParameterId= 'humidity_past1h' then Observations.Value end) as Humidty, AVG(ALL case when Observations.ParameterId= 'pressure_at_sea' then Observations.Value end) as Pressure, min(ALL case when Observations.ParameterId= 'temp_min_past1h' then Observations.Value end) as TempMin, max(ALL case when Observations.ParameterId= 'temp_max_past1h' then Observations.Value end) as TempMax FROM Observations where StationId=06123 Group by DateKey order by Datekey", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("TempMean,humidity,Pressure,TempMin,TempMax");


                    {
                        while (reader.Read())
                        {
                            dataset.Add($"{reader["TempMean"].ToString().Replace(',', '.')},{reader["humidity"].ToString().Replace(',', '.')},{reader["Pressure"].ToString().Replace(',', '.')},{reader["TempMin"].ToString().Replace(',', '.')},{reader["TempMax"].ToString().Replace(',', '.')}");

                        }
                        reader.Close();
                        myarray = dataset.ToArray();
                    }
                    return myarray;
                }




            }
        }
        public string[] SaveAlarmDataset()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand("SELECT MAX(Alarm.Alarms) as Alarms, AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean, AVG(ALL case when Observations.ParameterId= 'humidity_past1h' then Observations.Value end) as Humidty, AVG(ALL case when Observations.ParameterId= 'pressure_at_sea' then Observations.Value end) as Pressure, min(ALL case when Observations.ParameterId= 'temp_min_past1h' then Observations.Value end) as TempMin, max(ALL case when Observations.ParameterId= 'temp_max_past1h' then Observations.Value end) as TempMax FROM Observations JOIN (SELECT Alarms.DateKey, SUM(ALL Alarms.AlarmCount) as Alarms From Alarms Where Alarms.StationId = 06123 Group by Alarms.DateKey) as Alarm ON Alarm.DateKey = Observations.DateKey Where Observations.StationId = 06123 Group by Alarm.DateKey order by Alarm.Datekey", conn);

                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("Alarms,TempMean,Humidity,Pressure,TempMin,TempMax");


                    {
                        while (reader.Read())
                        {
                            dataset.Add($"{reader["Alarms"].ToString().Replace(',', '.')},{reader["TempMean"].ToString().Replace(',', '.')},{reader["Humidity"].ToString().Replace(',', '.')},{reader["Pressure"].ToString().Replace(',', '.')},{reader["TempMin"].ToString().Replace(',', '.')},{reader["TempMax"].ToString().Replace(',', '.')}");

                        }
                        reader.Close();
                        myarray = dataset.ToArray();

                    }
                    return myarray;

                }




            }
        }
        public string[] GetDailyHumidityFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand("SELECT DateKey, AVG(ALL case when Observations.ParameterId='humidity_past1h' then Observations.Value end) as Humidity From Observations where StationId = 06123 group by DateKey order by DateKey", conn);
                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,Humidity");

                    {
                        while (reader.Read())
                        {
                            dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["Humidity"].ToString().Replace(',', '.')}");

                        }
                        reader.Close();
                        myarray = dataset.ToArray();
                    }
                    return myarray;
                }
            }
        }
        public string[] GetDailyPressureFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand("SELECT DateKey, AVG(ALL case when Observations.ParameterId='pressure_at_sea' then Observations.Value end) as Pressure From Observations where StationId = 06123 group by DateKey order by DateKey", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,Pressure");

                    {
                        while (reader.Read())
                        {
                            dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["Pressure"].ToString().Replace(',', '.')}");

                        }

                        reader.Close();
                        myarray = dataset.ToArray();
                    }
                    return myarray;
                }




            }
        }
        public string[] GetDailyMinTempFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand("SELECT DateKey, AVG(ALL case when Observations.ParameterId='temp_min_past1h' then Observations.Value end) as TempMin From Observations where StationId = 06123 group by DateKey order by DateKey", conn);

                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,TempMin");
                    {
                        while (reader.Read())
                        {
                            dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["TempMin"].ToString().Replace(',', '.')}");
                        }
                        reader.Close();
                        myarray = dataset.ToArray();
                    }
                    return myarray;
                }
            }
        }

        public string[] GetDailyMaxTempFromObservations()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand("SELECT DateKey, AVG(ALL case when Observations.ParameterId='temp_max_past1h' then Observations.Value end) as TempMax From Observations where StationId = 06123 group by DateKey order by DateKey", conn);

                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,TempMax");

                    {
                        while (reader.Read())
                        {
                            dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["TempMax"].ToString().Replace(',', '.')}");
                        }
                        reader.Close();
                        myarray = dataset.ToArray();
                    }
                    return myarray;
                }
            }
        }
        public string[] GetDailyMeanTemperatureReading()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                List<string> dataset = new List<string>();
                string[] myarray;

                SqlCommand cmd = new SqlCommand("SELECT DateKey, AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean From Observations where StationId = 06123 group by DateKey order by DateKey", conn);


                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,TempMean");

                    while (reader.Read())
                    {
                        dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["TempMean"].ToString().Replace(',', '.')}");
                    }
                    reader.Close();
                    myarray = dataset.ToArray();
                }

                return myarray;


            }
        }


    }
}
