SELECT 
p.FullName,
COUNT(f.Id) AS CountOfAircraft,
SUM(f.TicketPrice) AS TotalPayed
FROM Passengers AS p
LEFT JOIN FlightDestinations AS f ON p.Id = f.PassengerId
GROUP BY p.FullName
HAVING SUBSTRING(p.FullName, 2, 1) = 'a' AND COUNT(f.Id) > 1
ORDER BY p.FullName