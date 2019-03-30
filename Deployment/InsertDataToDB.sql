USE TheaterDB;

GO

CREATE PROCEDURE [dbo].[insertDB] -- внесение данных в таблицу сотрудников

AS

BEGIN
DECLARE 
@name nvarchar(50),
@surname nvarchar (50),
@patr nvarchar (50),
@gender char,
@birth date,
@mobile varchar(12),
@nameOfrole nvarchar (50),
@exp int,
@on_holiday bit,
@salary money,
@bank_number varchar(16),
@count int;

SET @count = 100000;
	SET @gender = 'M';
	SET @birth = GETDATE();
	SET @mobile = '375445804011';
	SET @bank_number = '0000111100001111';
	SET @on_holiday = 'false';
	SET @nameOfrole = 'actor';


WHILE @count > 0
    BEGIN
	SET @name = CONVERT (nvarchar(50), @count);
	SET @surname = CONVERT (nvarchar(50), @count);
	SET @patr = CONVERT (nvarchar(50), @count);
	SET @exp = @count;
	SET @salary = @count;

        INSERT INTO [Personal] VALUES(@name, @surname, @patr, @gender, @birth, @mobile, @nameOfrole, @exp, @on_holiday, @salary, @bank_number)
        SET @count = @count - 1
    END;
END;

COMMIT

GO