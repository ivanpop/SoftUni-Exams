CREATE PROCEDURE usp_SearchByCategory(@category VARCHAR(50))  AS
BEGIN
        SELECT
			b.Name,
			b.YearPublished,
			b.Rating,
			c.Name,
			p.Name,
			CAST(pr.PlayersMin AS VARCHAR)  + ' people',
			CAST(pr.PlayersMax AS VARCHAR)  + ' people'
		FROM Boardgames AS b
		LEFT JOIN Categories AS c ON b.CategoryId = c.Id
		LEFT JOIN Publishers AS p ON b.PublisherId = p.Id
		LEFT JOIN PlayersRanges AS pr ON b.PlayersRangeId = pr.Id
		WHERE c.Name = @category
		ORDER BY p.Name, b.YearPublished DESC
END