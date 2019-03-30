USE TheaterDB;

GO

CREATE PROCEDURE [dbo].[sp_InsertPerson] -- для добавления сотрудника
	@Name nvarchar(50),
	@Surname nvarchar (50),
	@Patronymic nvarchar (50),
	@Gender char,
	@Birthday date,
	@Mobile varchar(12),
	@NameOfRole nvarchar (50),
	@Experience int,
	@On_holiday bit,
	@Salary money,
	@Bankcard_number varchar(16)

AS

INSERT INTO [Personal] ([Name], [Surname], [Patronymic], [Gender], [Birthday], [Mobile], [NameOfRole], [Experience], [On_holiday], [Salary], [Bankcard_number])
    VALUES (@Name, @Surname, @Patronymic, @Gender, @Birthday, @Mobile, @NameOfRole, @Experience, @On_holiday, @Salary, @Bankcard_number)

GO

CREATE PROCEDURE [dbo].[sp_InsertShedule] -- для добавления расписания
	@ID_Spec int out,
	@DateSpec date

AS
INSERT INTO [Shedule] (ID_Spectacle, Data_Time)
    VALUES (@ID_Spec, @DateSpec)

GO

CREATE PROCEDURE [dbo].[sp_InsertTicket] -- для добавления билета
	@ID_Spec int out,
	@Price money,
	@Count int

AS
INSERT INTO [Ticket] ([ID_Shedule], [Price], [Count])
    VALUES (@ID_Spec, @Price, @Count)

GO

CREATE PROCEDURE [dbo].[sp_InsertPersonToSpectacle] -- для добавления актеров к спетаклю
	@ID_Actor int out,
	@ID_Spec int out

AS
INSERT INTO [Peromance] ([ID_Person], [ID_Spectacle])
    VALUES (@ID_Actor, @ID_Spec)

GO

CREATE PROCEDURE [dbo].[ob_PersonalSelectAll] -- для выборки всех записей сотрудников
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRANSACTION

	SELECT * FROM   [dbo].[Personal] 
	
	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_SpectacleSelectAll] -- для выборки всех записей спектаклей
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRANSACTION

	SELECT * FROM [dbo].[Spectacle]
	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_SheduleSelectAll] -- для выборки всех записей распиания
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRANSACTION

	SELECT [Spectacle].[ID_Spectacle], [Spectacle].[NameOfSpectacle], [Shedule].[Data_Time] FROM [dbo].[Shedule] INNER JOIN Spectacle ON [Spectacle].ID_Spectacle = [Shedule].ID_Spectacle
	
	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_ActorToSpectacleSelectAll] -- для выборки всех сотрудников в спектакле !!!
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRANSACTION

	SELECT [Spectacle].[NameOfSpectacle], [Personal].[Name], [Personal].[Surname], [Personal].[Patronymic], [Personal].[NameOfRole] FROM [dbo].[Spectacle] INNER JOIN [Peromance] ON [Peromance].[ID_Spectacle] = [Spectacle].[ID_Spectacle] INNER JOIN [Personal] ON [Peromance].[ID_Person] = [Personal].[ID_Person]
	
	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_TicketSelectAll] -- для выборки всех записей билетов
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  

	BEGIN TRANSACTION

	--SELECT [Ticket].[ID_Ticket], [Spectacle].[NameOfSpectacle], [Ticket].[Price], [Ticket].[Count] FROM [dbo].[Ticket] INNER JOIN Spectacle ON [Ticket].[ID_Ticket] = [Spectacle].[ID_Spectacle]
	SELECT [Ticket].[ID_Ticket], [Spectacle].[NameOfSpectacle], [Ticket].[Price], [Ticket].[Count] FROM [dbo].[Ticket] INNER JOIN [Shedule] ON [Ticket].ID_Shedule = [Shedule].ID_Shedule INNER JOIN [Spectacle] ON [Shedule].[ID_Spectacle] = [Spectacle].[ID_Spectacle]

	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_PersobnalDeleteAll] -- для удаления всех записей
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	DELETE FROM [dbo].[Personal]

	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_PersonalDeleteItem] -- для удаления одного сотрудника
@id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	DELETE FROM [dbo].[Personal] WHERE [ID_Person] = @id

	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_SpectacleDeleteItem] -- для удаления одного спектакль
@id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	DELETE FROM [dbo].[Spectacle] WHERE [ID_Spectacle] = @id

	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_SheduleDeleteItem] -- для удаления одного расписания
@id int,
@DateSpec date
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	DELETE FROM [dbo].[Shedule] WHERE [ID_Spectacle] = @id AND [Data_Time] = @DateSpec

	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_TicketDeleteItem] -- для удаления одного сотрудника
@id int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	DELETE FROM [dbo].[Ticket] WHERE [ID_Ticket] = @id

	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_ActorFromSpectacleDeleteItem] -- для удаления одного сотрудника
@idA int,
@idSP int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	DELETE FROM [dbo].[Peromance] WHERE [ID_Person] = @idA AND [ID_Spectacle] = @idSP

	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_PersonalSearchItem] -- для поиска сотрудника по ФИО
	@Name nvarchar(50),
	@Surname nvarchar (50),
	@Patronymic nvarchar (50)
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	SELECT * FROM [dbo].[Personal] WHERE [Name] = @Name AND [Surname] = @Surname AND [Patronymic] = @Patronymic

	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_SpectacleSearchItem] -- для поиска спектаклей по названию и метки
	@NameOfSpectacle nvarchar(50),
	@AccountingSpectacle char
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	SELECT * FROM [dbo].[Spectacle] WHERE [NameOfSpectacle] = @NameOfSpectacle AND [AccountingSpectacle] = @AccountingSpectacle
	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_SheduleSearchItem] -- для поиска расписаний по дате
	@DateSpec date
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	SELECT [Spectacle].[NameOfSpectacle], [Shedule].[Data_Time] FROM [dbo].[Shedule] INNER JOIN [Spectacle] ON [Spectacle].ID_Spectacle = [Shedule].ID_Spectacle WHERE [Data_Time] = @DateSpec
	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_ACTSpecSearchItem] -- 12345678654324678
	@idSP int
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	SELECT [Spectacle].[NameOfSpectacle], [Personal].[Name], [Personal].[Surname], [Personal].[Patronymic], [Personal].[NameOfRole] FROM [dbo].[Personal] INNER JOIN [Peromance] ON [Peromance].[ID_Person] = [Personal].[ID_Person] INNER JOIN [Spectacle] ON [Peromance].[ID_Spectacle] = @idSP
	COMMIT
GO

CREATE PROCEDURE [dbo].[ob_PersonalComboBoxItem] -- для вывода сотрудников в combobox
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	SELECT [dbo].[Personal].[ID_Person], [dbo].[Personal].[Name], [dbo].[Personal].[Surname], [dbo].[Personal].[Patronymic] FROM [dbo].[Personal]

	COMMIT

GO

CREATE PROCEDURE [dbo].[ob_SpectacleComboBoxItem] -- для вывода спектаклей в combobox
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	SELECT [dbo].[Spectacle].[ID_Spectacle], [dbo].Spectacle.[NameOfSpectacle] FROM [dbo].[Spectacle]

	COMMIT

GO

CREATE PROCEDURE [dbo].[ob_TicketComboBoxItem] -- для вывода билетов в combobox
AS 
	SET NOCOUNT ON 
	SET XACT_ABORT ON  
	
	BEGIN TRANSACTION

	--SELECT [Spectacle].[NameOfSpectacle] FROM [dbo].[Ticket] INNER JOIN [Shedule] ON [Ticket].ID_Shedule = [Shedule].ID_Shedule INNER JOIN [Spectacle] ON [Shedule].[ID_Spectacle] = [Spectacle].[ID_Spectacle]
	SELECT [Shedule].[ID_Shedule], [Spectacle].[NameOfSpectacle] FROM [dbo].[Shedule] INNER JOIN [Spectacle] ON [Shedule].[ID_Spectacle] = [Spectacle].[ID_Spectacle]

	COMMIT

GO

CREATE PROCEDURE [dbo].[ob_SpectacleInsert] -- для внесения спектаклей
	@NameOfSpectacle nvarchar(50),
	@Description nvarchar (200),
	@AccountingSpectacle char

AS
INSERT INTO [Spectacle] ([NameOfSpectacle], [Description], [AccountingSpectacle])
    VALUES (@NameOfSpectacle, @Description, @AccountingSpectacle)

	COMMIT

GO

CREATE PROCEDURE [exportXML] -- экспорт
AS
	SELECT  [Name], [Surname] , [Patronymic], [Gender], [Birthday], [Mobile], [NameOfRole], [Experience], [On_holiday]
	FROM [dbo].[Personal]
	FOR XML PATH('Person'), ROOT('Personals');

GO
--DBCC CHECKIDENT ('[dbo].[Personal]', RESEED, 0);
CREATE PROCEDURE [importXML] -- импорт xml таблица [Personal]
@xml XML = NULL
AS
Select  @xml  = 
CONVERT(XML,bulkcolumn,2) FROM OPENROWSET(BULK 'D:\3_1kurs\Oracle\Курсач\TheaterBD\xmlBD.xml',SINGLE_BLOB) AS X
SET IDENTITY_INSERT [dbo].[Personal] OFF
SET ARITHABORT ON
		INSERT INTO [dbo].[Personal]([Name], [Surname] , [Patronymic], [Gender], [Birthday], [Mobile], [NameOfRole], [Experience], [On_holiday]) 
			SELECT 
		--P.value('ID_Personal[1]', 'int') AS [ID_Personal],
		P.value('Name[1]','nvarchar(50)') AS [Name],
		P.value('Surname[1]','nvarchar(50)') AS [Surname],
		P.value('Patronymic[1]','nvarchar(50)') AS [Patronymic],
		P.value('Gender[1]','char') AS [Gender],
		P.value('Birthday[1]','date') AS [Birthday],
		P.value('Mobile[1]','varchar(12)') AS [Mobile],
		P.value('NameOfRole[1]','nvarchar(50)') AS [NameOfRole],
		P.value('Experience[1]','int') AS [Experience],
		P.value('On_holiday[1]','bit') AS [On_holiday]
		--P.value('Salary[1]','money') AS [Salary],
		--P.value('Bankcard_number[1]','varchar(16)') AS [Bankcard_number]
			FROM @xml.nodes('//Personals/Person') PropertyFeed(P)
	COMMIT
GO

exec [exportXML] ;
exec [importXML];


SELECT * FROM [dbo].[Personal];
SELECT * FROM [dbo].[Spectacle];
SELECT * FROM [dbo].[Shedule];
SELECT * FROM [dbo].[Ticket];
DROP PROCEDURE [importXML];
DROP PROCEDURE [exportXML];
DROP PROCEDURE insertDB;
DROP PROCEDURE [dbo].[ob_PersonalSelectAll];
DROP PROCEDURE [dbo].[ob_TicketSelectAll]
DROP PROCEDURE [dbo].[sp_InsertPerson];
DROP PROCEDURE [dbo].[ob_SpectacleInsert];
DROP PROCEDURE [dbo].[ob_SpectacleSelectAll];
DROP PROCEDURE [dbo].[sp_InsertShedule];
DROP PROCEDURE [dbo].[ob_SheduleSelectAll];
DROP PROCEDURE [dbo].[ob_SheduleDeleteItem];
DROP PROCEDURE [dbo].[ob_SheduleSearchItem];
DROP PROCEDURE [dbo].[ob_TicketSelectAll];
DROP PROCEDURE [dbo].[ob_TicketComboBoxItem];
DROP PROCEDURE [dbo].[sp_InsertPersonToSpectacle];
DROP PROCEDURE [dbo].[ob_ActorToSpectacleSelectAll];
DROP PROCEDURE [dbo].[ob_ACTSpecSearchItem] -- 12345678654324678
--DROP PROCEDURE [dbo].[ob_PersonalDeleteItem];
--DROP PROCEDURE[dbo].[ob_PersonalSearchItem]
