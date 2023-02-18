SELECT
	DISTINCT SUBSTRING(t.name,CHARINDEX(' ', t.name), LEN(t.name)) AS LastName,
	t.Nationality,
	t.Age,
	t.PhoneNumber
FROM Sites AS s
JOIN SitesTourists AS st ON s.Id = st.SiteId
JOIN Tourists AS t ON st.TouristId = t.Id
JOIN Categories AS c ON s.CategoryId = c.Id
WHERE c.Name = 'History and archaeology'
ORDER BY LastName ASC