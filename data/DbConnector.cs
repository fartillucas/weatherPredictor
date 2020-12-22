using System;
using System.Collections.Generic;
using System.Data.SqlClient;


namespace WeatherPredictorComponent.data
{
    public class DbConnector
    {

        string ConnectionString = "Server=akctest01.database.windows.net;Database=akctestdb01;uid=DMIuserLogin;password=DmiLogin34!DK;Trusted_Connection=false";

        internal string[] GetDailyHumidityFromObservations(string StationId)
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
        internal string[] GetDailyPressureFromObservations(string StationId)
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
        internal string[] GetDailyMinTempFromObservations(string StationId)
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

        internal string[] GetDailyMaxTempFromObservations(string StationId)
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
        internal string[] GetDailyMeanTemperatureReading(string StationId)
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

        internal void SaveDaliyArimaForecast(string StationId, double TempMean, int Humidity, double Pressure, double TempMin, double TempMax, int ForecastDay)
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


        internal List<string> GetAllStationIds()
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

        internal int GetRegionIdFromStationId(string StationId)
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

        internal string[] GetDailyPressureFromObservationsByRegionId(int RegionId)
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

        internal string[] GetDailyMeanTemperatureReadingByRegionId(int RegionId)
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
        internal string[] GetDailyMaxTempFromObservationsByRegionId(int RegionId)
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
        internal string[] GetDailyMinTempFromObservationsByRegionId(int RegionId)
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
        internal string[] GetDailyHumidityFromObservationsByRegionId(int RegionId)
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

