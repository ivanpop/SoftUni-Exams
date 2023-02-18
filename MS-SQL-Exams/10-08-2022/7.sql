SELECT l.Province, l.Municipality, l.[Name], COUNT(s.Name) FROM Sites AS s
LEFT JOIN Locations AS l ON s.LocationId = l.Id
GROUP BY l.Province, l.Municipality, l.[Name]
HAVING Province = 'Sofia'
ORDER BY COUNT(s.Name) DESC, l.Name