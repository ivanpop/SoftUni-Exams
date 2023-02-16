SELECT
	o.[Name] + '-' + a.[Name] AS OwnersAnimals,
	o.PhoneNumber,
	ac.CageId
FROM Animals AS a
JOIN Owners AS o ON o.Id = a.OwnerId
JOIN AnimalsCages AS ac ON a.Id = ac.AnimalId
JOIN AnimalTypes AS ant ON a.AnimalTypeId = ant.Id
WHERE ant.AnimalType = 'Mammals'
ORDER BY o.[Name], a.[Name] DESC