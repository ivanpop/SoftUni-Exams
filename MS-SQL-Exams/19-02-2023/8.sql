SELECT TOP (5)
	b.Name,
	b.Rating,
	c.Name
FROM Boardgames AS b
LEFT JOIN PlayersRanges AS pr ON b.PlayersRangeId = pr.Id
LEFT JOIN Categories AS c ON b.CategoryId = c.Id
WHERE b.Rating > 7 AND (b.Name LIKE '%a%' OR b.Rating > 7.5) AND pr.PlayersMin BETWEEN 2 AND 5 AND pr.PlayersMax BETWEEN 2 AND 5
ORDER BY b.Name, b.Rating DESC