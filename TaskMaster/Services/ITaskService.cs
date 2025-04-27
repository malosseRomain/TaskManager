using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMaster.Models;

namespace TaskMaster.Services
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int taskId);
        Task<bool> AddTaskAsync(TaskItem task);
        Task<bool> UpdateTaskAsync(TaskItem task);
        Task<bool> DeleteTaskAsync(int id);
    }
}
