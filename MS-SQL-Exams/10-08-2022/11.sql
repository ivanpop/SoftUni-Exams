CREATE FUNCTION udf_GetTouristsCountOnATouristSite (@Site VARCHAR(100)) 
RETURNS INT AS
BEGIN
		DECLARE @result INT = (
			SELECT COUNT(*) FROM Sites AS s
			LEFT JOIN SitesTourists AS st ON s.Id = st.SiteId
			LEFT JOIN Tourists AS t ON st.TouristId = t.Id
			WHERE s.Name = @Site
		)
		RETURN @result
END