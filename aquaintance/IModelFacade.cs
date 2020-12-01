using System;
using System.Collections.Generic;
using System.Text;

namespace consoletester.aquaintance
{
    interface IModelFacade
    {
        public string[] CreateArimaModelWithForecast(string[] data, int daysToForecast, int p, int d, int q, string StationId);

    }
}
