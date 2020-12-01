using System;
using System.Collections.Generic;
using System.Text;

namespace consoletester.aquaintance
{
    interface IDatabaseFacade
    {
        public string[] GetDailyHumidityFromObservations(string stationId);
        public string[] GetDailyPressureFromObservations(string StationId);

        public string[] GetDailyMinTempFromObservations(string StationId);

        public string[] GetDailyMaxTempFromObservations(string StationId);

        public string[] GetDailyMeanTemperatureReading(string StationId);

        public void SaveDaliyArimaForecast(string StationId, double TempMean, int Humidity, double Pressure, double TempMin, double TempMax, int ForecastDay);

        public List<string> GetAllStationIds();

        public int GetRegionIdFromStationId(string StationId);

        public string[] GetDailyPressureFromObservationsByRegionId(int RegionId);

        public string[] GetDailyMeanTemperatureReadingByRegionId(int RegionId);

        public string[] GetDailyMaxTempFromObservationsByRegionId(int RegionId);

        public string[] GetDailyMinTempFromObservationsByRegionId(int RegionId);

        public string[] GetDailyHumidityFromObservationsByRegionId(int RegionId);
    }
}
