SELECT 
	c.Id,
	c.FirstName + ' ' + c.LastName,
	c.Email
FROM Creators AS c
LEFT JOIN CreatorsBoardgames AS cb ON c.Id = cb.CreatorId
WHERE cb.BoardgameId IS NULL