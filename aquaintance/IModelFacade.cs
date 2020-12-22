namespace WeatherPredictorComponent.aquaintance
{
    interface IModelFacade
    {
        public string[] CreateArimaModelWithForecast(string[] data, int daysToForecast, int p, int d, int q, string StationId);

    }
}
