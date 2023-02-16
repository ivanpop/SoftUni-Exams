SELECT TOP (5)
	o.[Name],
	COUNT(a.[Name])
	FROM Owners AS o
LEFT JOIN Animals AS a ON o.Id = a.OwnerId
GROUP BY o.[Name]
ORDER BY COUNT(a.[Name]) DESC, o.[Name]