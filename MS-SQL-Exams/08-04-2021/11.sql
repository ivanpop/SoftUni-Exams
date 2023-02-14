CREATE FUNCTION udf_HoursToComplete(@StartDate DATETIME, @EndDate DATETIME)
RETURNS INT AS
BEGIN
        DECLARE @result INT

		IF @StartDate IS NULL
        BEGIN
                SET @result = 0
        END
        ELSE IF @EndDate IS NULL
        BEGIN
                SET @result = 0
        END
        ELSE
        BEGIN
                SET @result = DATEDIFF(hour, @StartDate, @EndDate) 
        END
        RETURN @result
END