SELECT FullName, Country, ZIP, CigarPrice FROM (
	SELECT
		c.FirstName + ' ' + c.LastName AS FullName,
		a.Country AS Country,
		a.ZIP AS ZIP,
		'$' + CAST(cig.PriceForSingleCigar AS VARCHAR) AS CigarPrice,
		DENSE_RANK() OVER (PARTITION BY c.Id ORDER BY cig.PriceForSingleCigar DESC)
			  AS [Rankings]
	FROM Clients AS c
	LEFT JOIN Addresses AS a ON c.AddressId = a.Id
	LEFT JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
	LEFT JOIN Cigars AS cig ON cc.CigarId = cig.Id
	WHERE ISNUMERIC(a.ZIP) = 1
	) AS Tab
WHERE Rankings = 1
ORDER BY FullName