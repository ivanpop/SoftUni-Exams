SELECT TOP(5)
	c.[Name],
	COUNT(c.[Name])
FROM Reports AS r
LEFT JOIN Categories AS c ON r.CategoryId = c.Id
GROUP BY c.[Name]
ORDER BY COUNT(c.[Name]) DESC, c.[Name]