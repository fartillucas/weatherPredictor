using consoletester.aquaintance;
using System.Collections.Generic;

namespace consoletester.data
{
    class databaseFacade : IDatabaseFacade
    {
        private static databaseFacade instance;
        private DbConnector dbConnector;

        private databaseFacade()
        {
            dbConnector = new DbConnector();
        }
        public static databaseFacade getInstance()
        {
            if (instance == null)
            {
                instance = new databaseFacade();
            }
            return instance;
        }

        public List<string> GetAllStationIds()
        {
            return dbConnector.GetAllStationIds();
        }

        public string[] GetDailyHumidityFromObservations(string stationId)
        {
            return dbConnector.GetDailyHumidityFromObservations(stationId);
        }

        public string[] GetDailyHumidityFromObservationsByRegionId(int RegionId)
        {
            return dbConnector.GetDailyHumidityFromObservationsByRegionId(RegionId);
        }

        public string[] GetDailyMaxTempFromObservations(string StationId)
        {
            return dbConnector.GetDailyMaxTempFromObservations(StationId);
        }

        public string[] GetDailyMaxTempFromObservationsByRegionId(int RegionId)
        {
            return dbConnector.GetDailyMaxTempFromObservationsByRegionId(RegionId);
        }

        public string[] GetDailyMeanTemperatureReading(string StationId)
        {
            return dbConnector.GetDailyMeanTemperatureReading(StationId);
        }

        public string[] GetDailyMeanTemperatureReadingByRegionId(int RegionId)
        {
            return dbConnector.GetDailyMeanTemperatureReadingByRegionId(RegionId);
        }

        public string[] GetDailyMinTempFromObservations(string StationId)
        {
            return dbConnector.GetDailyMinTempFromObservations(StationId);
        }

        public string[] GetDailyMinTempFromObservationsByRegionId(int RegionId)
        {
            return dbConnector.GetDailyMinTempFromObservationsByRegionId(RegionId);
        }

        public string[] GetDailyPressureFromObservations(string StationId)
        {
            return dbConnector.GetDailyPressureFromObservations(StationId);
        }

        public string[] GetDailyPressureFromObservationsByRegionId(int RegionId)
        {
            return dbConnector.GetDailyPressureFromObservationsByRegionId(RegionId);
        }

        public int GetRegionIdFromStationId(string StationId)
        {
            return dbConnector.GetRegionIdFromStationId(StationId);
        }

        public void SaveDaliyArimaForecast(string StationId, double TempMean, int Humidity, double Pressure, double TempMin, double TempMax, int ForecastDay)
        {
            dbConnector.SaveDaliyArimaForecast(StationId, TempMean, Humidity, Pressure, TempMin, TempMax, ForecastDay);
        }
    }
}
