SELECT
	Alarms.DateKey,
	sum(ALL case when Alarms.DateKey=Alarms.DateKey then Alarms.AlarmCount end) as Alarms,
	AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean, 
	AVG(ALL case when Observations.ParameterId= 'humidity_past1h' then Observations.Value end) as Humidty, 
	AVG(ALL case when Observations.ParameterId= 'pressure_at_sea' then Observations.Value end) as Pressure, 
	min(ALL case when Observations.ParameterId= 'temp_min_past1h' then Observations.Value end) as TempMin, 
	max(ALL case when Observations.ParameterId= 'temp_max_past1h' then Observations.Value end) as TempMax 
	FROM Observations Right JOIN Alarms ON (Alarms.DateKey = Observations.DateKey AND Alarms.StationId = Observations.StationId)
	Where Observations.StationId = 06123
	Group by Alarms.DateKey 
	order by Alarms.Datekey