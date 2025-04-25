using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using TaskMaster.Models;
using TaskMaster.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskMaster.ViewModels
{
    public partial class TaskDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly AppDbContext _context;

        [ObservableProperty]
        private TaskItem task;

        [ObservableProperty]
        private bool hasSubTasks;

        public TaskDetailsViewModel(AppDbContext context)
        {
            _context = context;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("taskId", out var value) && value is int taskId)
            {
                // Charger la tâche avec toutes ses relations
                Task = _context.Tasks
                    .Include(t => t.Auteur)
                    .Include(t => t.Realisateur)
                    .Include(t => t.SousTaches)
                    .FirstOrDefault(t => t.Id_Task == taskId);

                HasSubTasks = Task?.SousTaches?.Any() ?? false;
            }
        }
    }
}
