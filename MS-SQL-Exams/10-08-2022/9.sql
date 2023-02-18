SELECT 
	t.Name,
	t.Age,
	t.PhoneNumber,
	t.Nationality,
	IIF(b.Name IS NULL, '(no bonus prize)', b.Name)
FROM Tourists AS t
LEFT JOIN TouristsBonusPrizes AS tb ON t.Id = tb.TouristId
LEFT JOIN BonusPrizes AS b ON tb.BonusPrizeId = b.Id
ORDER BY t.Name