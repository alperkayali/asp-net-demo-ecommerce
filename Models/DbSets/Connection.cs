using Microsoft.Data.SqlClient;

namespace AspNetCore_WebAPP_MVC_PROJE.Models.DbSets
{
    public class Connection
    {
        public static SqlConnection GetConnect
        {
            get
            {
                SqlConnection sqlConnection = new SqlConnection("Server=A;Database=eCOMMERCE;Trusted_Connection=True;TrustServerCertificate=True");
                return sqlConnection;
            }
        }
    }
}
