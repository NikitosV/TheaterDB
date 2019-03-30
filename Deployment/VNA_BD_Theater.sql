CREATE DATABASE VNA_THEATER_BD;
Use VNA_THEATER_BD;
go

CREATE TABLE [Role]                                 --Таблица ролей
(
[ID_Role] int constraint PK_Role primary key,
[ID_Person] int,
[NameOfRole] nvarchar (50),
[Experience] int
)

CREATE TABLE [Personal]                                 --Таблица персонала
(
[ID_Person] int constraint PK_Person primary key,
[ID_Role] int constraint FK_RolePerson foreign key references [Role](ID_Role),
[Name] nvarchar(50),
[Surname] nvarchar (50),
[Patronymic] nvarchar (50),
[Gender] char default 'M' check([Gender] = 'M' or [Gender] ='W'),
[Birthday] date,
[Mobile] varchar(13) check (Mobile like '375%'),
[On_holiday] bit default 'true',
[Salary] money,
[Bankcard_number] int
)

CREATE TABLE [Spectacle]                              --Таблица спектаклей
(
[ID_Spectacle] int constraint PK_Spectacle primary key,
[NameOfSpectacle] nvarchar (50),
[Description] nvarchar (200)
)

CREATE TABLE [Peromance]                              --Таблица представлений дял связи многии ко многим
(
[ID_Person] int constraint FK_PerfomancePersonSpectacle foreign key references [Personal](ID_Person),
[ID_Spectacle] int constraint FK_PerfomanceSpectaclePerson foreign key references [Spectacle](ID_Spectacle)
)

CREATE TABLE [Shedule]                              --Таблица расписание
(
[ID_Shedule] int constraint PK_Shedule primary key,
[ID_Spectacle] int constraint FK_Spectacle foreign key references [Spectacle](ID_Spectacle),
[Data_Time] date
)

CREATE TABLE [Ticket]                              --Таблица билетов
(
[ID_Ticket] int constraint PK_Ticket primary key,
[ID_Shedule] int constraint FK_TicketShedule foreign key references [Shedule](ID_Shedule),
[Price] money,
[Count] int
)

DROP TABLE [Ticket];                              --Удаление всего
DROP TABLE [Shedule];
DROP TABLE [Peromance];
DROP TABLE [Spectacle];
DROP TABLE [Personal];
DROP TABLE [Role];