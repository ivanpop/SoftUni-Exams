CREATE PROCEDURE usp_SearchByTaste(@taste VARCHAR(20)) AS
BEGIN
        SELECT
			c.CigarName,
			'$' + CAST(c.PriceForSingleCigar AS VARCHAR),
			t.TasteType,
			b.BrandName,
			CAST(s.Length AS VARCHAR) + ' cm',
			CAST(s.RingRange AS VARCHAR) + ' cm'
		FROM Cigars AS c
		LEFT JOIN Tastes AS t ON c.TastId = t.Id
		LEFT JOIN Brands AS b ON c.BrandId = b.Id
		LEFT JOIN Sizes AS s ON c.SizeId = s.Id
		WHERE t.TasteType = @taste
		ORDER BY s.Length, s.RingRange DESC
END