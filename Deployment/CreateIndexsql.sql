

CREATE INDEX indPer ON [Personal] ([Name], [Surname]);  -- ������ �� ����� � �������
CREATE INDEX indHoliday ON [Personal] ([On_holiday]);   -- ������ �� �������

DROP INDEX indPer ON [Personal]; -- ��� ������������
DROP INDEX indHoliday ON [Personal];
SELECT * FROM [Personal] WHERE [On_holiday] = 'true';
SELECT * FROM Personal;
EXEC [dbo].[ob_PersonalSearchItem] '1', '1', '1';
