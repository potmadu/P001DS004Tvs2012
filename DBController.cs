using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Windows.Forms;

namespace Sample3
{
    class DBController
    {

        private static string server_name, database_name, table_name;

        public DBController() {

            server_name = @"UA-PC\SQLEXPRESS";
            database_name = "FileSystemDB";
            table_name = "VideoTable";

        }

        private static bool test_connection()
        {
            bool result = false;
            SqlConnection SqlConnection = connect_database();
            if (SqlConnection != null)
            {
                result = true;
            }
            return result;
        }

        private static SqlConnection connect_database()
        {

            SqlConnection sqlConnection1 = new SqlConnection(@"Data Source=" + server_name + ";Initial Catalog = " + database_name + "; Integrated Security = True");
            try
            {
                sqlConnection1.Open();
            }
            catch (SqlException e)
            {
                MessageBox.Show("Cannot connect to Database Server: "+e);
                Environment.Exit(0);
            }
            return sqlConnection1;

        }

        private static void disconnect_database(SqlConnection sqlconnection)
        {
            if (sqlconnection != null)
            {
                sqlconnection.Close();
            }
        }

        public List<string> list_video()
        {
            List<string> list_video = new List<string>();

            using (TransactionScope transactionScope = new TransactionScope())
            {
                SqlConnection sqlConnection = connect_database();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = String.Format("SELECT PkId,Id,Description FROM {0}", table_name);

                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            list_video.Add(reader.GetValue(2).ToString());
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Couldn't fetch videos : " + e);
                    }
                }
                disconnect_database(sqlConnection);
                transactionScope.Complete();
            }

            return list_video;
        }

        public string retrieve_video(string video_name)
        {
            string video_location="";

            using (TransactionScope transactionScope = new TransactionScope())
            {
                SqlConnection sqlConnection = connect_database();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = sqlConnection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = String.Format("SELECT FileData.PathName() As Path, GET_FILESTREAM_TRANSACTION_CONTEXT() As TransactionContext FROM {0} WHERE description=@video_name", table_name);
                    command.Parameters.AddWithValue("@video_name", video_name);

                    try
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            string filePath = (string)reader["Path"];
                            byte[] transactionContext = (byte[])reader["TransactionContext"];
                            SqlFileStream sqlFileStream = new SqlFileStream(filePath, transactionContext, FileAccess.Read);
                            byte[] video_data = new byte[sqlFileStream.Length];
                            sqlFileStream.Read(video_data, 0, Convert.ToInt32(sqlFileStream.Length));
                            sqlFileStream.Close();

                            string filename = @"C:\Users\UA\Documents\Project\Database Systems\Program\temp\" + video_name;
                            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Write);
                            fs.Write(video_data, 0, video_data.Length);
                            fs.Flush();
                            fs.Close();

                            video_location = filename;

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("{0}", e);
                    }
                    disconnect_database(sqlConnection);
                    transactionScope.Complete();
                }
            }

            return video_location;

        }

    }
}
