using SmartUp.DataAccess.SQLServer.Dao;

string abb = CourseDao.GetSemesterAbbreviation("Web Development and Design");

Console.WriteLine(abb);