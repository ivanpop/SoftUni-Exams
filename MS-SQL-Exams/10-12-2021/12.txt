CREATE PROCEDURE usp_SearchByAirportName @airportName VARCHAR(70) AS
BEGIN       
		SELECT 
		ap.AirportName,
		p.FullName,
		CASE 
			WHEN fd.TicketPrice <= 400 THEN 'Low'
			WHEN fd.TicketPrice >= 401 AND fd.TicketPrice <= 1500 THEN 'Medium'
			WHEN fd.TicketPrice >= 1501 THEN 'High'
		END AS LevelOfTickerPrice,
		ac.Manufacturer,
		ac.Condition,
		ait.TypeName
		FROM Airports AS ap
		JOIN FlightDestinations AS fd ON ap.Id = fd.AirportId
		JOIN Passengers AS p ON fd.PassengerId = p.Id
		JOIN Aircraft AS ac ON fd.AircraftId = ac.Id
		JOIN AircraftTypes AS Ait ON ac.TypeId = Ait.Id
		WHERE ap.AirportName = @airportName
		ORDER BY ac.Manufacturer, p.FullName		
END