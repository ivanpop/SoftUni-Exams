SELECT ap.AirportName, f.Start, f.TicketPrice, p.FullName, a.Manufacturer, a.Model FROM FlightDestinations AS f
LEFT JOIN Airports AS ap ON f.AirportId = ap.Id
LEFT JOIN Passengers AS p ON f.PassengerId = p.Id
LEFT JOIN Aircraft AS a ON f.AircraftId = a.Id
WHERE (DATEPART(HOUR, f.Start) BETWEEN 6 AND 20) AND f.TicketPrice > 2500
ORDER BY a.Model