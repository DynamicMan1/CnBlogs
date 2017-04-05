using CnBlogs.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using CnBlogs.Core.Utils;
using System.Data.Common;

namespace CnBlogs.Core.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string connStr;

        public UserRepository(IRepositorySettings settings)
        {
            connStr = settings.ConnStr;
        }

        public void Insert(User user)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("insert into User(Email,UserName,DisplayName,Password,CreateTime,LastModifiedTime,LastLoginTime,LoginTimes,IsActivate,IsBlogApply) values(@Email,@UserName,@DisplayName,@Password,@CreateTime,@LastModifiedTime,@LastLoginTime,@LoginTimes,@IsActivate,@IsBlogApply)", conn);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@UserName", user.UserName);
                command.Parameters.AddWithValue("@DisplayName", user.DisplayName);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@CreateTime", user.CreateTime);
                command.Parameters.AddWithValue("@LastModifiedTime", user.LastModifiedTime);
                command.Parameters.AddWithValue("@LastLoginTime", user.LastLoginTime);
                command.Parameters.AddWithValue("@LoginTimes", user.LoginTimes);
                command.Parameters.AddWithValue("@IsActivate", user.IsActivate);
                command.Parameters.AddWithValue("@IsBlogApply", user.IsBlogApply);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void DeleteById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("delete from User where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IEnumerable<User> SelectAll()
        {
            List<User> result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from User", conn);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                    result = new List<User>();
                while (reader.Read())
                {
                    var user = new User();
                    user.Id = reader.GetInt32(0);
                    user.Email = reader.GetString(1);
                    user.UserName = reader.GetString(2);
                    user.DisplayName = reader.GetString(3);
                    user.Password = reader.GetString(4);
                    user.CreateTime = DateTime.Parse(reader.GetString(5));
                    user.LastModifiedTime = DateTime.Parse(reader.GetString(6));
                    user.LastLoginTime = DateTime.Parse(reader.GetString(7));
                    user.LoginTimes = reader.GetInt32(8);
                    user.IsActivate = reader.GetBoolean(9);
                    user.IsBlogApply = reader.GetBoolean(10);
                    result.Add(user);
                }
                conn.Close();
            }
            return result;
        }

        public User SelectById(int id)
        {
            User result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from User where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = new User();
                    result.Id = reader.GetInt32(0);
                    result.Email = reader.GetString(1);
                    result.UserName = reader.GetString(2);
                    result.DisplayName = reader.GetString(3);
                    result.Password = reader.GetString(4);
                    result.CreateTime = DateTime.Parse(reader.GetString(5));
                    result.LastModifiedTime = DateTime.Parse(reader.GetString(6));
                    result.LastLoginTime = DateTime.Parse(reader.GetString(7));
                    result.LoginTimes = reader.GetInt32(8);
                    result.IsActivate = reader.GetBoolean(9);
                    result.IsBlogApply = reader.GetBoolean(10);
                }
                conn.Close();
            }
            return result;
        }

        public User SelectByUserName(string userName)
        {
            User result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from User where UserName=@userName", conn);
                command.Parameters.AddWithValue("@userName", userName);
                using (DbDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = new User();
                        result.Id = reader.GetInt32(0);
                        result.Email = reader.GetString(1);
                        result.UserName = reader.GetString(2);
                        result.DisplayName = reader.GetString(3);
                        result.Password = reader.GetString(4);
                        result.CreateTime = DateTime.Parse(reader.GetString(5));
                        result.LastModifiedTime = DateTime.Parse(reader.GetString(6));
                        result.LastLoginTime = DateTime.Parse(reader.GetString(7));
                        result.LoginTimes = reader.GetInt32(8);
                        result.IsActivate = reader.GetBoolean(9);
                        result.IsBlogApply = reader.GetBoolean(10);
                    }
                }
                conn.Close();
            }
            return result;
        }

        public User SelectByEmail(string email)
        {
            User result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from User where Email=@email", conn);
                command.Parameters.AddWithValue("@email", email);
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = new User();
                    result.Id = reader.GetInt32(0);
                    result.Email = reader.GetString(1);
                    result.UserName = reader.GetString(2);
                    result.DisplayName = reader.GetString(3);
                    result.Password = reader.GetString(4);
                    result.CreateTime = DateTime.Parse(reader.GetString(5));
                    result.LastModifiedTime = DateTime.Parse(reader.GetString(6));
                    result.LastLoginTime = DateTime.Parse(reader.GetString(7));
                    result.LoginTimes = reader.GetInt32(8);
                    result.IsActivate = reader.GetBoolean(9);
                    result.IsBlogApply = reader.GetBoolean(10);
                }
                conn.Close();
            }
            return result;
        }

        public void UpdateStatus(int id, int status)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("update User set IsActivate=@status,LastModifiedTime=@LastModifiedTime where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@LastModifiedTime", DateTime.Now);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void UpdateLoginTimes(int id, int times)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("update User set LoginTimes=@LoginTimes,LastLoginTime=@LastLoginTime where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@LoginTimes", times);
                command.Parameters.AddWithValue("@LastLoginTime", DateTime.Now);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void AddLoginTimes(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("update User set LoginTimes=LoginTimes+1,LastLoginTime=@LastLoginTime where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@LastLoginTime", DateTime.Now);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void UpdatePassword(int id, string password)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("update User set Password=@password,LastModifiedTime=@LastModifiedTime where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@LastModifiedTime", DateTime.Now);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void UpdateBlogApply(int id, bool isBlogApply)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("update User set IsBlogApply=@isBlogApply,LastModifiedTime=@LastModifiedTime where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@isBlogApply", isBlogApply);
                command.Parameters.AddWithValue("@LastModifiedTime", DateTime.Now);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
