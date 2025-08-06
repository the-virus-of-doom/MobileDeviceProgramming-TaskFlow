using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    internal class LocalDBService
    {
        private const string DB_NAME = "taskflow_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDBService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<UserTask>();
        }

        #region UserTask
        public async Task<List<UserTask>> GetUserTasks(int userId)
        {
            return await _connection.Table<UserTask>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        public async Task<UserTask> GetById(int id)
        {
            return await _connection.Table<UserTask>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task Create(UserTask userTask)
        {
            await _connection.InsertAsync(userTask);
        }

        public async Task Update(UserTask userTask)
        {
            await _connection.UpdateAsync(userTask);
        }

        public async Task Delete(UserTask userTask)
        {
            await _connection.DeleteAsync(userTask);
        }
        #endregion UserTask

    }
}
