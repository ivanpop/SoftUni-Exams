CREATE PROCEDURE usp_AnnualRewardLottery(@TouristName VARCHAR(30))  AS
BEGIN
		DECLARE @count INT = (
			SELECT COUNT(*) FROM Tourists AS t
			LEFT JOIN SitesTourists AS st ON t.Id = st.TouristId
			LEFT JOIN Sites AS s ON st.SiteId = s.Id
			WHERE t.Name = @TouristName
		)
		
		DECLARE @reward VARCHAR(15) 

		IF @count >= 100
			SET @reward = 'Gold badge'
		ELSE IF @count >= 50
			SET @reward = 'Silver badge'
		ELSE IF @count >= 25
			SET @reward = 'Bronze badge'

		UPDATE Tourists
		SET Reward = @reward
		WHERE [Name] = @TouristName

		SELECT [Name], Reward FROM Tourists
		WHERE [Name] = @TouristName
END