using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskMaster.Models;
using Microsoft.EntityFrameworkCore;
using TaskMaster.Data;

namespace TaskMaster.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetAllTasksAsync()
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Auteur)
                    .Include(t => t.Realisateur)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Log l'erreur si nécessaire
                return new List<TaskItem>();
            }
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            try
            {
                return await _context.Tasks
                    .Include(t => t.Auteur)
                    .Include(t => t.Realisateur)
                    .FirstOrDefaultAsync(t => t.Id_Task == id);
            }
            catch (Exception ex)
            {
                // Log l'erreur si nécessaire
                return null;
            }
        }

        public async Task<bool> AddTaskAsync(TaskItem task)
        {
            try
            {
                _context.Tasks.Add(task);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Log l'erreur si nécessaire
                return false;
            }
        }

        public async Task<bool> UpdateTaskAsync(TaskItem task)
        {
            try
            {
                _context.Tasks.Update(task);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Log l'erreur si nécessaire
                return false;
            }
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null) return false;

                _context.Tasks.Remove(task);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                // Log l'erreur si nécessaire
                return false;
            }
        }
    }
}
