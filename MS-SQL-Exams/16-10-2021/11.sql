CREATE FUNCTION udf_ClientWithCigars(@name VARCHAR(30)) 
RETURNS INT AS
BEGIN
		RETURN  (
			SELECT COUNT(*) FROM Clients AS c
			LEFT JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
			LEFT JOIN Cigars AS cig ON cc.CigarId = cig.Id
			WHERE c.FirstName = @name
		)
END