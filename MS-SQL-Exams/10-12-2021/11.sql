CREATE FUNCTION udf_FlightDestinationsByEmail(@email VARCHAR(50)) 
RETURNS INT AS
BEGIN
		DECLARE @result INT = 
		(
			SELECT COUNT(fd.Id)
			FROM Passengers AS p
			LEFT JOIN FlightDestinations AS fd ON p.Id = fd.PassengerId
			GROUP BY p.Email
			HAVING p.Email = @email
		)
		RETURN @result
END