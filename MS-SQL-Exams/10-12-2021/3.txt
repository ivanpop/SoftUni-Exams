UPDATE Aircraft
SET Condition = 'A'
WHERE Condition IN ('C','B') AND Year >= 2013 AND (FlightHours <= 100 OR FlightHours IS NULL)