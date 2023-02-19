SELECT 
	c.LastName,
	AVG(s.Length),
	CEILING(AVG(s.RingRange))
FROM Clients AS c
LEFT JOIN ClientsCigars AS cc ON c.Id = cc.ClientId
LEFT JOIN Cigars AS cig ON cc.CigarId = cig.Id
LEFT JOIN Sizes AS s ON cig.SizeId = s.Id
WHERE cig.Id IS NOT NULL
GROUP BY c.LastName
ORDER BY AVG(s.Length) DESC