SELECT
	c.LastName,
	CEILING(AVG(b.Rating)),
	p.Name
FROM Creators AS c
JOIN CreatorsBoardgames AS cb ON c.Id = cb.CreatorId
LEFT JOIN Boardgames AS b ON cb.BoardgameId = b.Id
LEFT JOIN Publishers AS p ON b.PublisherId = p.Id
WHERE p.Name = 'Stonemaier Games'
GROUP BY c.LastName, p.Name
ORDER BY AVG(b.Rating) DESC
