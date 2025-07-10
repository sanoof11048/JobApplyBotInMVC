using Dapper;
using JobApplyBotInMVC.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace JobApplyBotInMVC.Data
{
    public class AppDbContext
    {
        private readonly string _connectionString;

        public AppDbContext()
        {
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
        }

        public async Task SaveApplicationAsync(JobApplicationRequest request)
        {
            using var connection = new SqliteConnection(_connectionString);
            string sql = @"INSERT INTO Applications (CompanyName, Contact, Position, Medium, ResumePath)
                           VALUES (@CompanyName, @Contact, @Position, @Medium, @ResumePath);";
            await connection.ExecuteAsync(sql, request);
        }

        public async Task InitializeDatabaseAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            await connection.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Applications (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CompanyName TEXT,
                    Contact TEXT NOT NULL,
                    Position TEXT NOT NULL,
                    Medium TEXT NOT NULL,
                    ResumePath TEXT
                );
            ");
        }

        public async Task<IEnumerable<JobApplicationRequest>> GetAllApplicationsAsync()
        {
            using var connection = new SqliteConnection(_connectionString);
            string sql = "SELECT * FROM Applications ORDER BY Id DESC";
            return await connection.QueryAsync<JobApplicationRequest>(sql);
        }

    }
}
