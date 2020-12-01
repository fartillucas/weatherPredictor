using consoletester.aquaintance;
using System;
using System.Collections.Generic;
using System.Text;

namespace consoletester.logic
{
    public class modelFacade : IModelFacade
    {
        private static modelFacade instance;
        private model model;
        private modelFacade()
        {
            model = new model();
        }

        public static modelFacade getInstance()
        {
            if (instance == null)
            {
                instance = new modelFacade();
            }
            return instance;
        }

        [Obsolete]
        public string[] CreateArimaModelWithForecast(string[] data, int daysToForecast, int p, int d, int q, string StationId)
        {
            return instance.CreateArimaModelWithForecast(data, daysToForecast, p, d, q, StationId);
        }

    }
}
