namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class CourseDao
    {
        private static CourseDao? instance;
        private CourseDao() 
        {
        }

        public static CourseDao getInstance()
        {
            if (instance == null)
            {
                instance = new CourseDao();
            } 
            return instance;
        }
    }
}
