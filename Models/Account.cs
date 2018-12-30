using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel; 

using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace mymvc.Models
{
    public class Account
    {
        public Account(string conStr)
        {
            connectionString=conStr;
        }
        private  string connectionString;
        public bool InsertUser(User sm)
        {
            SqlConnection con =new SqlConnection(connectionString);

            string query = "INSERT INTO Account (Email,Password) VALUES('" + sm.Email + "','" + sm.Password  + "'); SELECT SCOPE_IDENTITY();";
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            int i = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            if (i >= 1)
                return true;
            else
                return false;
        }

        public bool IsNew(string eml)
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "SELECT Email From Account WHERE Email ='"+eml+"'";
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            var obj = cmd.ExecuteScalar();
            con.Close();
            if(obj==null)
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

        public bool Confirm(string eml)
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "SELECT Confirm From Account WHERE Email ='"+eml+"'";
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            var obj = cmd.ExecuteScalar();
            con.Close();
 
            if(Convert.IsDBNull(obj))
            {
                return false;
            }
            else
            {
                return (bool)obj;
            }
        }
        public void UpdateConfirm(string userId)
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "UPDATE Account SET Confirm = 1 WHERE UserId ="+userId;
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            var obj = cmd.ExecuteScalar();
            con.Close();
        }

        public bool PasswordMatch(string eml, string pas)
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "SELECT Password From Account WHERE Email ='"+eml+"'";
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            var obj = cmd.ExecuteScalar();
            con.Close();
            
            return (string)obj==pas;          
        }
      public Int32 GetUserId(string eml)
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "SELECT UserId From Account WHERE Email  ='"+eml+"'";
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            var obj = cmd.ExecuteScalar();
            con.Close();
            return (Int32)obj;          
        }

      public int DeleteUser(string userId)
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "DELETE From Account WHERE UserId ="+userId;
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            int ret=cmd.ExecuteNonQuery();
            con.Close();
            return ret;              
        }

      public User GetUser(string userId)
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "SELECT * From Account WHERE UserId ="+userId;
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            User tUser=GetRead((IDataRecord)reader);
            reader.Close();
            con.Close();
            return tUser;          
        }
        public int UpdateUser(string userId, string items)
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "UPDATE Account SET "+items+" WHERE UserId ="+userId;
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            int ret=cmd.ExecuteNonQuery();
            con.Close();
            return ret;          
        }



        public List<User> GetAllUsers()
        {
            SqlConnection con =new SqlConnection(connectionString);
            string query = "SELECT * From Account";
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<User> tAllUsers=new List<User>();
            // Call Read before accessing data.
            while (reader.Read())
            {
                tAllUsers.Add(GetRead((IDataRecord)reader));
            }

            // Call Close when done reading.
            reader.Close();
            con.Close();
            return tAllUsers;
        }

        private User GetRead(IDataRecord record)
        {
            User tUser=new User();
            tUser.UserId=record.GetInt32(0).ToString();
            tUser.Email=record.GetString(1);
            tUser.Password=record.GetString(2);
            if(record.IsDBNull(3))
            {
                tUser.Confirm=false;
            }
            else{
                tUser.Confirm = record.GetBoolean (3);
            }

            return tUser;  
        }
    }
}