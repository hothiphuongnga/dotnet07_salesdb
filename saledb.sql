;WITH Numbers AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1
    FROM Numbers
    WHERE n < 10000
)
INSERT INTO dbo.Products (ProductName, Price, Stock)
SELECT
    CHOOSE(
        (n - 1) % 10 + 1,
        N'Laptop Dell',
        N'iPhone',
        N'Chuột Logitech',
        N'Bàn phím Keychron',
        N'Màn hình Samsung',
        N'Tai nghe Sony',
        N'iPad',
        N'Loa JBL',
        N'Sạc Anker',
        N'Ổ cứng Kingston'
    ) + N' Test ' + RIGHT('00000' + CAST(n AS VARCHAR(5)), 5),
    100000 + ABS(CHECKSUM(NEWID()) % 4000) * 10000,
    ABS(CHECKSUM(NEWID()) % 201)
FROM Numbers
OPTION (MAXRECURSION 0);



-- phân trang

-- mỗi trang lấy 20 sản phẩm 
-- trang 1: 1-> 20
-- trang 2: 21 -> 40 20 bỏ qua 20 (pageIndex - 1) * pageSize 
-- trang 3: 41 -> 60 20 bỏ qua 40 = (3 -1)*20
-- trang 4: 61 -> 80 20 bỏ qua 60 = (4-1)*20
-- offset - bỏ qua n  
select top 20 * from Products  order by id desc -- trang 1

select * from Products Where ProductName  like N'%iphone%' ORDER  by id desc 
offset 20 rows fetch next 20 rows only -- trang 2
-- select * from Products ORDER  by id desc 
-- offset (index -1)* pageSize rows fetch next pageSize rows only







