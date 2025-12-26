using System.Data;
using Microsoft.Data.SqlClient;
using TodoApp.Models;
using Microsoft.Extensions.Configuration;

namespace TodoApp.ADO.NET_Repository
{
    public class TodoRepository
    {

        private readonly string _connectionString;

        public TodoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<Todo> GetAll()
        {
            var todos = new List<Todo>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Todos";
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    todos.Add(new Todo
                    {
                        Id = (int)reader["Id"],
                        Task = reader["Task"].ToString(),
                        IsCompleted = (bool)reader["IsCompleted"]
                    });
                }
            }
            return todos;
        }

        public void Add(Todo todo)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO Todos (Task, IsCompleted) VALUES (@Task, @IsCompleted)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Task", todo.Task);
                cmd.Parameters.AddWithValue("@IsCompleted", todo.IsCompleted);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStatus(int id, bool isCompleted)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE Todos SET IsCompleted=@IsCompleted WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@IsCompleted", isCompleted);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM Todos WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
