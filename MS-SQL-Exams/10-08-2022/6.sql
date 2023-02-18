SELECT s.Name, l.Name, s.Establishment, c.Name FROM Sites AS s
LEFT JOIN Locations AS l ON s.LocationId = l.Id
LEFT JOIN Categories AS c ON s.CategoryId = c.Id
ORDER BY c.Name DESC, l.Name, s.Name