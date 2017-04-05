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
    public class EmailRepository : IEmailRepository
    {
        private readonly string connStr;

        public EmailRepository(IRepositorySettings settings)
        {
            connStr = settings.ConnStr;
        }

        public void Insert(Email email)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("insert into Email(UserId,CreateTime,PrivateKeyJson,PublicKeyJson,ActionType) values(@UserId,@CreateTime,@PrivateKeyJson,@PublicKeyJson,@ActionType)", conn);
                command.Parameters.AddWithValue("@UserId", email.UserId);
                command.Parameters.AddWithValue("@CreateTime", email.CreateTime);
                command.Parameters.AddWithValue("@PrivateKeyJson", email.PrivateKeyJson);
                command.Parameters.AddWithValue("@PublicKeyJson", email.PublicKeyJson);
                command.Parameters.AddWithValue("@ActionType", email.ActionType);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void DeleteById(int id)
        {
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("delete from Email where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public IEnumerable<Email> SelectAll()
        {
            List<Email> result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from Email", conn);
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                    result = new List<Email>();
                while (reader.Read())
                {
                    var email = new Email();
                    email.Id = reader.GetInt32(0);
                    email.UserId = reader.GetInt32(1);
                    email.CreateTime = DateTime.Parse(reader.GetString(2));
                    email.PrivateKeyJson = reader.GetString(3);
                    email.PublicKeyJson = reader.GetString(4);
                    email.ActionType = reader.GetInt16(5);
                }
                conn.Close();
            }
            return result;
        }

        public Email SelectById(int id)
        {
            Email result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from Email where Id=@id", conn);
                command.Parameters.AddWithValue("@id", id);
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = new Email();
                    result.Id = reader.GetInt32(0);
                    result.UserId = reader.GetInt32(1);
                    result.CreateTime = DateTime.Parse(reader.GetString(2));
                    result.PrivateKeyJson = reader.GetString(3);
                    result.PublicKeyJson = reader.GetString(4);
                    result.ActionType = reader.GetInt16(5);
                }
            }
            return result;
        }

        public Email SelectLastByUserId(int userId, int actionType)
        {
            Email result = null;
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var command = new MySqlCommand("select * from Email where UserId=@UserId and ActionType=@ActionType order by CreateTime desc limit 1", conn);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@ActionType", actionType);
                DbDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result = new Email();
                    result.Id = reader.GetInt32(0);
                    result.UserId = reader.GetInt32(1);
                    result.CreateTime = DateTime.Parse(reader.GetString(2));
                    result.PrivateKeyJson = reader.GetString(3);
                    result.PublicKeyJson = reader.GetString(4);
                    result.ActionType = reader.GetInt16(5);
                }
            }
            return result;
        }
    }
}
