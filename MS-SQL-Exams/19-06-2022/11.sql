CREATE FUNCTION udf_GetVolunteersCountFromADepartment (@VolunteersDepartment VARCHAR(50))
RETURNS INT AS
BEGIN
		DECLARE @Result INT = (
		   SELECT COUNT(*) FROM Volunteers AS v
		   LEFT JOIN VolunteersDepartments AS vd ON v.DepartmentId = vd.Id
		   WHERE vd.DepartmentName = @VolunteersDepartment
	   )
	   RETURN @Result
END