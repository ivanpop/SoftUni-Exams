SELECT s.Name, l.Name, l.Municipality, l.Province, s.Establishment FROM Sites AS s
LEFT JOIN Locations AS l ON s.LocationId = l.Id
WHERE s.[Name] NOT LIKE '[BMD]%' AND s.Establishment LIKE '%BC'
ORDER BY s.Name