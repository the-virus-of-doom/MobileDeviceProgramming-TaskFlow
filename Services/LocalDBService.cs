using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Models;

namespace TaskFlow.Services
{
    public class LocalDBService
    {
        private const string DB_NAME = "taskflow_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDBService()
        {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            Debug.WriteLine("====== DB Location: " + Path.Combine(FileSystem.AppDataDirectory, DB_NAME).ToString());
            _connection.CreateTableAsync<UserTask>();
        }

        #region UserTask
        private UserTask EnforceUser(int userId, UserTask userTask)
        {
            // force any UserTask to be using the correct userId
            // to prevent them from being assigned to the wrong user
            userTask.UserId = userId;
            return userTask;
        }

        public async Task<List<UserTask>> GetUserTasks(int userId)
        {
            return await _connection.Table<UserTask>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        public async Task<UserTask> GetById(int userId, int id)
        {
            return await _connection.Table<UserTask>()
                .Where(x => x.UserId == userId)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task Create(int userId, UserTask userTask)
        {
            await _connection.InsertAsync(EnforceUser(userId, userTask));
        }

        public async Task Update(int userId, UserTask userTask)
        {
            await _connection.UpdateAsync(EnforceUser(userId, userTask));
        }

        public async Task Delete(UserTask userTask)
        {
            // it would be nice to validate the current user, 
            // but this may be beyond the scope for now
            // ...unless we add another service layer that handles that
            await _connection.DeleteAsync(userTask);
        }
        #endregion UserTask

    }
}
