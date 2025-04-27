using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using TaskMaster.Models;
using TaskMaster.Data;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using TaskMaster.Views;
using TaskMaster.Services;
using System.Diagnostics;

namespace TaskMaster.ViewModels
{
    public partial class TaskDetailsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private TaskItem _task;

        [ObservableProperty]
        private bool _hasSubTasks;

        [ObservableProperty]
        private bool _hasComments;

        public TaskDetailsViewModel(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("taskId", out var value) && value is int taskId)
            {
                // Charger la tâche avec toutes ses relations
                Task = _context.Tasks
                    .Include(t => t.Auteur)
                    .Include(t => t.Realisateur)
                    .Include(t => t.Projet)
                    .Include(t => t.SousTaches)
                    .Include(t => t.Commentaires)
                        .ThenInclude(c => c.Auteur)
                    .FirstOrDefault(t => t.Id_Task == taskId);

                HasSubTasks = Task?.SousTaches?.Any() ?? false;
                HasComments = Task?.Commentaires?.Any() ?? false;
            }
        }

        public async Task LoadTaskAsync(int taskId)
        {
            Task = await _context.Tasks
                .Include(t => t.SousTaches)
                .Include(t => t.Commentaires)
                    .ThenInclude(c => c.Auteur)
                .Include(t => t.Auteur)
                .Include(t => t.Realisateur)
                .Include(t => t.Projet)
                .FirstOrDefaultAsync(t => t.Id_Task == taskId);

            if (Task != null)
            {
                HasSubTasks = Task.SousTaches?.Any() ?? false;
                HasComments = Task.Commentaires?.Any() ?? false;
            }

            Debug.WriteLine($"Auteur: {Task?.Auteur?.DisplayName}");
            Debug.WriteLine($"Realisateur: {Task?.Realisateur?.DisplayName}");
            Debug.WriteLine($"Projet: {Task?.Projet?.Nom}");
        }
    }
}
