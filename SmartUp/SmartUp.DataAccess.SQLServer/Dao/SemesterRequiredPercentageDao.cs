using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartUp.DataAccess.SQLServer.Dao
{
    public class SemesterRequiredPercentageDao
    {
        private static SemesterRequiredPercentageDao instance;

        private SemesterRequiredPercentageDao()
        {
        }

        public static SemesterRequiredPercentageDao GetInstance()
        {
            if (instance == null)
            {
                instance = new SemesterRequiredPercentageDao();
            }
            return instance;
        }


    }
}
