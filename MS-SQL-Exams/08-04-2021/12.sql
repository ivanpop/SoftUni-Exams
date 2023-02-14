CREATE PROCEDURE usp_AssignEmployeeToReport(@EmployeeId INT, @ReportId INT)  AS
BEGIN
	DECLARE @empDepartment INT = (
		SELECT Employees.DepartmentId FROM Employees
		WHERE Employees.Id = @EmployeeId
	)

	DECLARE @depCat INT = (
		SELECT c.DepartmentId FROM Reports AS r
		LEFT JOIN Categories AS c ON r.CategoryId = c.Id
		WHERE r.Id = @ReportId
	)

	IF @empDepartment <> @depCat
			THROW 50001, 'Employee doesn''t belong to the appropriate department!', 1;
	ELSE
		BEGIN
			UPDATE Reports
			SET EmployeeId = @EmployeeId
			WHERE Id = @ReportId
		END
END