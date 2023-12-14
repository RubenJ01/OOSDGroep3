using Microsoft.Data.SqlClient;

namespace SmartUp.Core.Methods
{
    public class Methods
    {
        public void CloseConnection(SqlConnection sqlConnection)
        {
            sqlConnection.Close();
        }
    }
}
