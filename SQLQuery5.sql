﻿SELECT
	MAX(Alarm.Alarms) Alarms,
	AVG(ALL case when Observations.ParameterId='temp_mean_past1h' then Observations.Value end) as TempMean, 
	AVG(ALL case when Observations.ParameterId= 'humidity_past1h' then Observations.Value end) as Humidty, 
	AVG(ALL case when Observations.ParameterId= 'pressure_at_sea' then Observations.Value end) as Pressure, 
	min(ALL case when Observations.ParameterId= 'temp_min_past1h' then Observations.Value end) as TempMin, 
	max(ALL case when Observations.ParameterId= 'temp_max_past1h' then Observations.Value end) as TempMax 
	FROM Observations 
	JOIN 
		(
		SELECT
		Alarms.DateKey,
		SUM(ALL Alarms.AlarmCount) as Alarms
		From Alarms
		Where Alarms.StationId = 06123
		Group by Alarms.DateKey
		) as Alarm
	ON Alarm.DateKey = Observations.DateKey
	Where Observations.StationId = 06123
	Group by Alarm.DateKey 
	order by Alarm.Datekey