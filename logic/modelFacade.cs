using WeatherPredictorComponent.aquaintance;
using System;


namespace WeatherPredictorComponent.logic
{
    public class ModelFacade : IModelFacade
    {
        private static ModelFacade instance;
        private model model;
        private ModelFacade()
        {
            model = new model();
        }

        public static ModelFacade getInstance()
        {
            if (instance == null)
            {
                instance = new ModelFacade();
            }
            return instance;
        }

        [Obsolete]
        public string[] CreateArimaModelWithForecast(string[] data, int daysToForecast, int p, int d, int q, string StationId)
        {
            return model.CreateArimaModelWithForecast(data, daysToForecast, p, d, q, StationId);
        }

    }
}
