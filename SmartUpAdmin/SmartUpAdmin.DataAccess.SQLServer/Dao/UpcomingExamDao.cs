namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class UpcomingExamDao
    {
        private static UpcomingExamDao? instance;

        private UpcomingExamDao()
        {
        }

        public static UpcomingExamDao GetInstance()
        {
            if (instance == null)
            {
                instance = new UpcomingExamDao();
            }
            return instance;
        }

       
    }
}
