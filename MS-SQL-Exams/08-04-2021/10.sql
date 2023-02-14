SELECT 
	CASE
		WHEN e.FirstName + ' ' + e.LastName IS NULL THEN 'None'
		ELSE e.FirstName + ' ' + e.LastName
	END AS Employee,
	CASE
		WHEN d.[Name] IS NULL THEN 'None'
		ELSE d.[Name]
	END AS Department,
	c.[Name] AS Category,
	r.[Description],
	FORMAT(r.OpenDate, 'dd.MM.yyyy') AS OpenDate,
	s.[Label] AS [Status],
	u.[Name] AS [User]
	FROM Reports AS r
LEFT JOIN Employees AS e ON r.EmployeeId = e.Id
LEFT JOIN Departments AS d ON e.DepartmentId = d.Id
LEFT JOIN Categories AS c ON r.CategoryId = c.Id
LEFT JOIN [Status] AS s ON r.StatusId = s.Id
LEFT JOIN Users AS u ON r.UserId = u.Id
ORDER BY e.FirstName DESC, e.LastName DESC, d.[Name], c.[Name], r.[Description], r.OpenDate, s.[Label], u.Username