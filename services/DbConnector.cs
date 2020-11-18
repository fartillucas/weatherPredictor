using CsvHelper;
using Extreme.Mathematics.LinearAlgebra;
using Extreme.Statistics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
        public string[] GetDailyHumidityFromObservations(string StationId)
        {
            List<string> dataset = new List<string>();
            string[] myarray;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand($"SELECT DateKey, AVG(ALL case when Observations.ParameterId='humidity_past1h' then Observations.Value end) as Humidity From Observations where StationId = {StationId.Trim('"')} AND IsValid = 1 group by DateKey order by DateKey", conn);
                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,Humidity");

                    {
                        while (reader.Read())
                        {
                            if (String.Equals(reader["Humidity"].ToString().Replace(',', '.'), ""))
                            {
                                continue;
                            }
                            dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["Humidity"].ToString().Replace(',', '.')}");
                        }
                        reader.Close();
                        myarray = dataset.ToArray();

                    }

                }
                if (myarray.Length == 0 || myarray.Length == 1 || myarray == null)
                {
                    myarray = GetDailyHumidityFromObservationsByRegionId(GetRegionIdFromStationId(StationId));
                }
            }
            return myarray;
        }
        public string[] GetDailyPressureFromObservations(string StationId)
        {
            string[] myarray;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();

                SqlCommand cmd = new SqlCommand($"SELECT DateKey, AVG(ALL case when Observations.ParameterId='pressure_at_sea' then Observations.Value end) as Pressure From Observations where StationId = {StationId.Trim('"')} AND IsValid = 1 group by DateKey order by DateKey", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,Pressure");

                    {
                        while (reader.Read())
                        {
                            if (String.Equals(reader["Pressure"].ToString().Replace(',', '.'), ""))
                            {
                                continue;
                            }
                            dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["Pressure"].ToString().Replace(',', '.')}");
                        }

                        reader.Close();
                        myarray = dataset.ToArray();
                    }

                }

                if (myarray.Length == 0 || myarray.Length == 1 || myarray == null)
                {
                    myarray = GetDailyPressureFromObservationsByRegionId(GetRegionIdFromStationId(StationId));
                }


            }
            return myarray;
        }
        public string[] GetDailyMinTempFromObservations(string StationId)
        {
            List<string> dataset = new List<string>();
            string[] myarray;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                SqlCommand cmd = new SqlCommand($"SELECT DateKey, MIN(ALL case when Observations.ParameterId='temp_min_past1h' then Observations.Value end) as TempMin From Observations where StationId = {StationId.Trim('"')} AND IsValid = 1 group by DateKey order by DateKey", conn);

                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,TempMin");
                    {
                        while (reader.Read())
                        {
                            if (String.Equals(reader["TempMin"].ToString().Replace(',', '.'), ""))
                            {
                                continue;
                            }
                            dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["TempMin"].ToString().Replace(',', '.')}");
                        }
                        reader.Close();
                        myarray = dataset.ToArray();
                    }
                }
                if (myarray.Length == 0 || myarray.Length == 1 || myarray == null)
                {
                    myarray = GetDailyMinTempFromObservationsByRegionId(GetRegionIdFromStationId(StationId));
                }
            }
            return myarray;
        }

        public string[] GetDailyMaxTempFromObservations(string StationId)
        {
            List<string> dataset = new List<string>();
            string[] myarray;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand($"SELECT DateKey, MAX(ALL case when Observations.ParameterId='temp_max_past1h' then Observations.Value end) as TempMax From Observations where StationId = {StationId.Trim('"')} AND IsValid = 1 group by DateKey order by DateKey", conn);

                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,TempMax");

                    {
                        while (reader.Read())
                        {
                            if (String.Equals(reader["TempMax"].ToString().Replace(',', '.'), ""))
                            {
                                continue;
                            }
                            dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["TempMax"].ToString().Replace(',', '.')}");
                        }
                        reader.Close();
                        myarray = dataset.ToArray();
                    }
                }
                if (myarray.Length == 0 || myarray.Length == 1 || myarray == null)
                {
                    myarray = GetDailyMaxTempFromObservationsByRegionId(GetRegionIdFromStationId(StationId));
                }
            }
            return myarray;
        }
        public string[] GetDailyMeanTemperatureReading(string StationId)
        {
            List<string> dataset = new List<string>();
            string[] myarray;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand($"SELECT DateKey, AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean From Observations where StationId = {StationId.Trim('"')} AND IsValid = 1 group by DateKey order by DateKey", conn);
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    dataset.Add("DateKey,TempMean");

                    while (reader.Read())
                    {
                        if (String.Equals(reader["TempMean"].ToString().Replace(',', '.'), ""))
                        {
                            continue;
                        }
                        dataset.Add($"{reader["DateKey"].ToString().Replace(',', '.')},{reader["TempMean"].ToString().Replace(',', '.')}");
                    }
                    reader.Close();
                    myarray = dataset.ToArray();
                }
                if (myarray.Length == 0 || myarray.Length == 1 || myarray == null)
                {
                    myarray = GetDailyMeanTemperatureReadingByRegionId(GetRegionIdFromStationId(StationId));
                }
            }
            return myarray;
        }

        public void SaveDaliyArimaForecast(string StationId, double TempMean, int Humidity, double Pressure, double TempMin, double TempMax, int ForecastDay)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand($"INSERT INTO ForecastsFromArima(StationId, TempMean, Humidity, Pressure, TempMin, TempMax, ForecastDay) Values(@StationId, @TempMean, @Humidity, @Pressure, @TempMin, @TempMax, @ForecastDay)", conn);
                cmd.Parameters.AddWithValue("@StationId", StationId);
                cmd.Parameters.AddWithValue("@TempMean", TempMean);
                cmd.Parameters.AddWithValue("@Humidity", Humidity);
                cmd.Parameters.AddWithValue("@Pressure", Pressure);
                cmd.Parameters.AddWithValue("@TempMin", TempMin);
                cmd.Parameters.AddWithValue("@TempMax", TempMax);
                cmd.Parameters.AddWithValue("@ForecastDay", ForecastDay);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void saveLocalDataset()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select Observed, Value from Observations where StationId=06079 AND Isvalid=1 AND ParameterId='humidity_past1h' Order by Observed", conn);
                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                using (StreamWriter writer = new StreamWriter(@"C:\Users\Michael\source\repos\consoletester\Csvs\humidity.csv"))
                {
                    while (reader.Read())

                        writer.WriteLine("{0},{1}",

                        reader["Observed"], reader["Value"].ToString().Replace(',', '.'));
                }
            }
        }

        public List<string> GetAllStationIds()
        {
            List<string> returnList = new List<string>();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT distinct stationId FROM Observations", conn);
                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    returnList.Add((string)reader["StationId"]);
                }
            }
            return returnList;
        }

        public int GetRegionIdFromStationId(string StationId)
        {
            int regionId = 0;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand($"SELECT DISTINCT RegionId FROM Stations WHERE Id={StationId.Trim('"')}", conn);
                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    regionId = Convert.ToInt32(reader["RegionId"].ToString());
                }
            }
            return regionId;
        }

        public string[] GetDailyPressureFromObservationsByRegionId(int RegionId)
        {
            string[] myarray;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();

                SqlCommand cmd = new SqlCommand($"SELECT DateKey, AVG(ALL case when Observations.ParameterId='pressure_at_sea' then Observations.Value end) as Pressure From Observations where RegionId = {RegionId} AND IsValid = 1 group by DateKey order by DateKey", conn);

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

                }

            }
            return myarray;
        }

        public string[] GetDailyMeanTemperatureReadingByRegionId(int RegionId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;

                SqlCommand cmd = new SqlCommand($"SELECT DateKey, AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean From Observations where RegionId = {RegionId} AND IsValid = 1 group by DateKey order by DateKey", conn);
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
        public string[] GetDailyMaxTempFromObservationsByRegionId(int RegionId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand($"SELECT DateKey, MAX(ALL case when Observations.ParameterId='temp_max_past1h' then Observations.Value end) as TempMax From Observations where RegionId = {RegionId} AND IsValid = 1 group by DateKey order by DateKey", conn);
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
        public string[] GetDailyMinTempFromObservationsByRegionId(int RegionId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand($"SELECT DateKey, MIN(ALL case when Observations.ParameterId='temp_min_past1h' then Observations.Value end) as TempMin From Observations where RegionId = {RegionId} AND IsValid = 1 group by DateKey order by DateKey", conn);
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
        public string[] GetDailyHumidityFromObservationsByRegionId(int RegionId)
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                List<string> dataset = new List<string>();
                string[] myarray;
                SqlCommand cmd = new SqlCommand($"SELECT DateKey, AVG(ALL case when Observations.ParameterId='humidity_past1h' then Observations.Value end) as Humidity From Observations where RegionId = {RegionId} AND IsValid = 1 group by DateKey order by DateKey", conn);
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
    }

}

