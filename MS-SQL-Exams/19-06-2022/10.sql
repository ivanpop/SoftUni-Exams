SELECT 
	a.[Name], 
	YEAR(a.BirthDate) AS BirthYear,
	ant.AnimalType
FROM Animals AS a
LEFT JOIN AnimalTypes AS ant ON a.AnimalTypeId = ant.Id
WHERE 
	a.OwnerId IS NULL 
	AND a.AnimalTypeId <> 3
	AND a.BirthDate > '2018-01-01'
ORDER BY a.[Name]