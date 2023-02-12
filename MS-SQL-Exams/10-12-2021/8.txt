SELECT 
	a.Id,
	a.Manufacturer,
	a.FlightHours,
	COUNT(f.Id) AS FlightDestinationsCount,
	ROUND(AVG(f.TicketPrice), 2)
FROM Aircraft AS a
LEFT JOIN FlightDestinations AS f ON a.Id = f.AircraftId
GROUP BY a.Id, a.Manufacturer, a.FlightHours
HAVING COUNT(f.Id) >= 2
ORDER BY COUNT(f.Id) DESC, a.Id