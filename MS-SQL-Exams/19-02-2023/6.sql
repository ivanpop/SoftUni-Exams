SELECT
	b.Id,
	b.[Name],
	b.YearPublished,
	c.Name
FROM Boardgames AS b
LEFT JOIN Categories AS c ON b.CategoryId = c.Id
WHERE CategoryId IN (6, 8)
ORDER BY YearPublished DESC