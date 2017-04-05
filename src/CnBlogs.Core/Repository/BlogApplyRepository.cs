using CnBlogs.Core.Entities;
using CnBlogs.Core.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CnBlogs.Core.Repository
{
    public class BlogApplyRepository : IBlogApplyRepository
    {
        private readonly string connStr;

        public BlogApplyRepository(IRepositorySettings settings)
        {
            connStr = settings.ConnStr;
        }

        public void Insert(BlogApply blogApply)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("insert into BlogApply(UserId,Reason,RealName,Position,Unit,Interest,IsRead,LastModifiedTime,CreateTime) values(@UserId,@Reason,@RealName,@Position,@Unit,@Interest,@IsRead,@LastModifiedTime,@CreateTime)", conn);
                command.Parameters.AddWithValue("@UserId", blogApply.UserId);
                command.Parameters.AddWithValue("@RealName", blogApply.RealName);
                command.Parameters.AddWithValue("@Reason", blogApply.Reason);
                command.Parameters.AddWithValue("@Position", blogApply.Position);
                command.Parameters.AddWithValue("@Unit", blogApply.Unit);
                command.Parameters.AddWithValue("@Interest", blogApply.Interest);
                command.Parameters.AddWithValue("@IsRead", blogApply.IsRead);
                command.Parameters.AddWithValue("@LastModifiedTime", blogApply.LastModifiedTime);
                command.Parameters.AddWithValue("@CreateTime", blogApply.CreateTime);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void DeleteById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("delete from BlogApply where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IEnumerable<BlogApply> SelectAll()
        {
            List<BlogApply> result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from BlogApply", conn);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                    result = new List<BlogApply>();
                while (reader.Read())
                {
                    var blogApply = new BlogApply();
                    blogApply.Id = reader.GetInt32(0);
                    blogApply.UserId = reader.GetInt32(1);
                    blogApply.Reason = reader.GetString(2);
                    blogApply.RealName = reader[3] == DBNull.Value ? null : reader[3].ToString();
                    blogApply.Position = reader[4] == DBNull.Value ? null : reader[4].ToString();
                    blogApply.Unit = reader[5] == DBNull.Value ? null : reader[5].ToString();
                    blogApply.Interest = reader[6] == DBNull.Value ? null : reader[6].ToString();
                    blogApply.IsRead = reader.GetBoolean(7);
                    blogApply.LastModifiedTime = reader.GetDateTime(8);
                    blogApply.CreateTime = reader.GetDateTime(9);
                    result.Add(blogApply);
                }
                conn.Close();
            }
            return result;
        }

        public BlogApply SelectById(int id)
        {
            BlogApply result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from BlogApply where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = new BlogApply();
                    result.Id = reader.GetInt32(0);
                    result.UserId = reader.GetInt32(1);
                    result.Reason = reader.GetString(2);
                    result.RealName = reader[3] == DBNull.Value ? null : reader[3].ToString();
                    result.Position = reader[4] == DBNull.Value ? null : reader[4].ToString();
                    result.Unit = reader[5] == DBNull.Value ? null : reader[5].ToString();
                    result.Interest = reader[6] == DBNull.Value ? null : reader[6].ToString();
                    result.IsRead = reader.GetBoolean(7);
                    result.LastModifiedTime = reader.GetDateTime(8);
                    result.CreateTime = reader.GetDateTime(9);
                }
                conn.Close();
            }
            return result;
        }

        public IEnumerable<BlogApply> SelectAllNoReader()
        {
            List<BlogApply> result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from BlogApply where IsRead=0 order by CreateTime", conn);
                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        result = new List<BlogApply>();
                    while (reader.Read())
                    {
                        var blogApply = new BlogApply();
                        blogApply.Id = reader.GetInt32(0);
                        blogApply.UserId = reader.GetInt32(1);
                        blogApply.Reason = reader.GetString(2);
                        blogApply.RealName = reader[3] == DBNull.Value ? null : reader[3].ToString();
                        blogApply.Position = reader[4] == DBNull.Value ? null : reader[4].ToString();
                        blogApply.Unit = reader[5] == DBNull.Value ? null : reader[5].ToString();
                        blogApply.Interest = reader[6] == DBNull.Value ? null : reader[6].ToString();
                        blogApply.IsRead = reader.GetBoolean(7);
                        blogApply.LastModifiedTime = reader.GetDateTime(8);
                        blogApply.CreateTime = reader.GetDateTime(9);
                        result.Add(blogApply);
                    }
                }
                conn.Close();
            }
            return result;
        }

        public void UpdateIsRead(int id, bool isRead)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("update BlogApply set IsRead=@IsRead,LastModifiedTime=@LastModifiedTime where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@IsRead", isRead);
                command.Parameters.AddWithValue("@LastModifiedTime", DateTime.Now);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IEnumerable<BlogApply> SelectByUserId(int userId)
        {
            List<BlogApply> result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from BlogApply where UserId=@UserId order by CreateTime desc", conn);
                command.Parameters.AddWithValue("@UserId", userId);
                using (DbDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                        result = new List<BlogApply>();
                    while (reader.Read())
                    {
                        var blogApply = new BlogApply();
                        blogApply.Id = reader.GetInt32(0);
                        blogApply.UserId = reader.GetInt32(1);
                        blogApply.Reason = reader.GetString(2);
                        blogApply.RealName = reader[3] == DBNull.Value ? null : reader[3].ToString();
                        blogApply.Position = reader[4] == DBNull.Value ? null : reader[4].ToString();
                        blogApply.Unit = reader[5] == DBNull.Value ? null : reader[5].ToString();
                        blogApply.Interest = reader[6] == DBNull.Value ? null : reader[6].ToString();
                        blogApply.IsRead = reader.GetBoolean(7);
                        blogApply.LastModifiedTime = reader.GetDateTime(8);
                        blogApply.CreateTime = reader.GetDateTime(9);
                        result.Add(blogApply);
                    }
                }
                conn.Close();
            }
            return result;
        }
    }
}
