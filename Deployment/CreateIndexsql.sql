

CREATE INDEX indPer ON [Personal] ([Name], [Surname]);  -- индекс по имени и фамилии
CREATE INDEX indHoliday ON [Personal] ([On_holiday]);   -- индекс по гендеру

DROP INDEX indPer ON [Personal]; -- для тестирования
DROP INDEX indHoliday ON [Personal];
SELECT * FROM [Personal] WHERE [On_holiday] = 'true';
SELECT * FROM Personal;
EXEC [dbo].[ob_PersonalSearchItem] '1', '1', '1';
