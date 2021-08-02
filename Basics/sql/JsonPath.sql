SELECT
    COUNT(items.ItemId) TotalItems,
	AVG(items.SizeInKb) AverageItemSizeInKb
FROM
(
	SELECT 
		output.ItemId,
		DATALENGTH(output.ItemJson) AS SizeInBytes,
		DATALENGTH(output.ItemJson) / 1024.0 AS SizeInKb
	FROM 
	(
		SELECT 
			ItemId,
			(
				SELECT CUST.*,
					(
						SELECT * FROM ORDERS WHERE CustomerId = CUST.Id FOR JSON AUTO
					) orders
				FROM CUSTOMERS CUST
				--WHERE itm.ItemId = item.ItemId
				FOR JSON AUTO
			) ItemJson
		FROM [dbo].[Item] item
		--WHERE CompanyId =  8130523
	) output
) items