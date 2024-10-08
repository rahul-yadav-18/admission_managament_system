using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace MyProject.Models
{
    public class DBManager
    {
        public SqlConnection connection = new SqlConnection("Data Source=LAPTOP-LLLN3B5A\\SQLEXPRESS;Initial Catalog=IndeedLearning;Integrated Security=True");
        public int ExecuteInsertUpdateDelete(string query)
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            int result = command.ExecuteNonQuery();
            connection.Close();
            return result;
        }
        public DataTable ExecuteSelect(string query)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

    }
}