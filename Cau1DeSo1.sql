﻿-- câu 1 a đề 1
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE DsNhanVienTrongNgay 	
	(@date datetime)
AS
BEGIN
	
	SELECT Employees.EmployeeID, Employees.LastName, Employees.FirstName, sum (UnitPrice*Quantity*(1-Discount)) as DoanhThuTrongNgay
	from Employees , [Order Details], Orders
	where Employees.EmployeeID = Orders.EmployeeID
		and [Order Details].OrderID = Orders.OrderID
		and OrderDate = @date
		group by  Employees.EmployeeID, Employees.LastName, Employees.FirstName
END
GO
exec DsNhanVienTrongNgay '1996-7-5'

--câu 1 b đề 1
alter PROCEDURE DoanhThuTrongKhoangThoiGian
	-- Add the parameters for the stored procedure here
	@dateBegin datetime,
	@dateEnd datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
		select e.EmployeeID,e.FirstName, e.LastName, SUM(((c.Quantity * c.UnitPrice) * (1 - c.Discount))) as doanhThu
	from Employees e INNER JOIN Orders o ON e.EmployeeID = o.EmployeeID
		 INNER JOIN [Order Details] c ON o.OrderID = c.OrderID
	where o.OrderDate between @dateBegin and @dateEnd 
	group by e.EmployeeID,e.FirstName, e.LastName
END
GO
exec DoanhThuTrongKhoangThoiGian '1996-07-04', '1996-07-18'


