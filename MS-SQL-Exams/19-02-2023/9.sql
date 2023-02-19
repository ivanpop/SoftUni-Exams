SELECT 
	FullName,
	Email,
	Rating
FROM(
	SELECT 
		c.FirstName + ' ' + c.LastName AS FullName,
		c.Email AS Email,
		b.Rating AS Rating,
		DENSE_RANK() OVER (PARTITION BY c.Id ORDER BY b.Rating DESC)
				  AS [Rankings]
	FROM Creators AS c
	LEFT JOIN CreatorsBoardgames cb ON c.Id = cb.CreatorId
	LEFT JOIN Boardgames AS b ON cb.BoardgameId = b.Id
	WHERE SUBSTRING(c.Email, LEN(c.Email) - 3, 4) = '.com'
	) AS Tab
	WHERE Rankings = 1 AND Rating IS NOT NULL
	ORDER BY FullName