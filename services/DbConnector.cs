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
                SqlCommand cmd = new SqlCommand("select Observed, Value from Observations where StationId=06119 AND Isvalid=1 AND ParameterId='temp_mean_past1h'", conn);


                {
                    conn.Open();
                    using SqlDataReader reader = cmd.ExecuteReader();
                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Michael\source\repos\consoletester\actualdata.csv"))
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
