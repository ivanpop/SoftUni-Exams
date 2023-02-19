CREATE FUNCTION udf_CreatorWithBoardgames(@name NVARCHAR(30)) 
RETURNS INT AS
BEGIN
	RETURN (
		SELECT COUNT(*) FROM Creators AS c
		JOIN CreatorsBoardgames AS cb ON c.Id = cb.CreatorId
		LEFT JOIN Boardgames AS b ON cb.BoardgameId = b.Id
		WHERE c.FirstName = @name
	)
END