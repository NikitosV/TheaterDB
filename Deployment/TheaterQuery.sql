CREATE DATABASE TheaterDB;
Use TheaterDB;

go
CREATE TABLE [Personal]                                 --Таблица персонала
(
[ID_Person] int constraint PK_Person primary key IDENTITY (1, 1),
[Name] nvarchar(50),
[Surname] nvarchar (50),
[Patronymic] nvarchar (50),
[Gender] char default 'M' check([Gender] = 'M' or [Gender] ='W'),
[Birthday] date,
[Mobile] varchar(12),
[NameOfRole] nvarchar (50),
[Experience] int,
[On_holiday] bit default 'false',
[Salary] money,
[Bankcard_number] varchar(16)
)



CREATE TABLE [Spectacle]                              --Таблица спектаклей
(
[ID_Spectacle] int constraint PK_Spectacle primary key IDENTITY (1, 1),
[NameOfSpectacle] nvarchar (50),
[Description] nvarchar (200),
[AccountingSpectacle] char default 'R' check([AccountingSpectacle] = 'R' or [AccountingSpectacle] = 'E')
)

CREATE TABLE [Peromance]                              --Таблица представлений дял связи многии ко многим
(
[ID_Person] int constraint FK_PerfomancePersonSpectacle foreign key references [Personal](ID_Person),
[ID_Spectacle] int constraint FK_PerfomanceSpectaclePerson foreign key references [Spectacle](ID_Spectacle)
)

CREATE TABLE [Shedule]                              --Таблица расписание
(
[ID_Shedule] int constraint PK_Shedule primary key IDENTITY (1, 1),
[ID_Spectacle] int constraint FK_Spectacle foreign key references [Spectacle](ID_Spectacle),
[Data_Time] date
)

CREATE TABLE [Ticket]                              --Таблица билетов
(
[ID_Ticket] int constraint PK_Ticket primary key IDENTITY (1, 1),
[ID_Shedule] int constraint FK_TicketShedule foreign key references [Shedule](ID_Shedule) NOT NULL,
[Price] money,
[Count] int
)

DROP TABLE [Peromance];
DROP TABLE [Personal];
DROP TABLE [Ticket];                              --Удаление всего
DROP TABLE [Shedule];
DROP TABLE [Spectacle];
