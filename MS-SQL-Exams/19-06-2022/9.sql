SELECT
	v.[Name],
	v.PhoneNumber,
	CASE
		WHEN SUBSTRING(v.[Address], 1, 5) = 'Sofia' THEN SUBSTRING(v.[Address], 8, LEN(v.[Address]))
		WHEN SUBSTRING(v.[Address], 1, 6) = ' Sofia' THEN SUBSTRING(v.[Address], 10, LEN(v.[Address]))
	END AS [Address]
	FROM Volunteers AS v
LEFT JOIN VolunteersDepartments AS vd ON v.DepartmentId = vd.Id
WHERE vd.DepartmentName = 'Education program assistant' AND v.[Address] LIKE '%Sofia%'
ORDER BY v.[Name]