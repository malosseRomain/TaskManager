using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;

namespace TaskMaster.ViewModels
{
    public partial class ModifyTaskViewModel : ObservableObject, IQueryAttributable
    {
        private readonly AppDbContext _context;

        [ObservableProperty]
        private TaskItem task;

        public List<TaskCategory> Categories { get; } = Enum.GetValues(typeof(TaskCategory)).Cast<TaskCategory>().ToList();
        public List<TaskPriority> Priorities { get; } = Enum.GetValues(typeof(TaskPriority)).Cast<TaskPriority>().ToList();
        public List<TaskMaster.Models.TaskStatus> Statuses { get; } = 
            Enum.GetValues(typeof(TaskMaster.Models.TaskStatus))
                .Cast<TaskMaster.Models.TaskStatus>()
                .ToList();

        public ModifyTaskViewModel(AppDbContext context)
        {
            _context = context;
            Task = new TaskItem();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("taskId", out var value) && value is int taskId)
            {
                Task = _context.Tasks.FirstOrDefault(t => t.Id_Task == taskId);
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (Task == null || Task.Id_Task == 0)
            {
                await Shell.Current.DisplayAlert("Erreur", "Tâche invalide", "OK");
                return;
            }

            try
            {
                var existingTask = await _context.Tasks.FindAsync(Task.Id_Task);
                if (existingTask == null)
                {
                    await Shell.Current.DisplayAlert("Erreur", "Tâche introuvable", "OK");
                    return;
                }

                existingTask.Titre = Task.Titre;
                existingTask.Description = Task.Description;
                existingTask.Echeance = Task.Echeance;
                existingTask.Categorie = Task.Categorie;
                existingTask.Priorite = Task.Priorite;
                existingTask.Statut = Task.Statut;

                await _context.SaveChangesAsync();
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", $"Erreur lors de la sauvegarde : {ex.Message}", "OK");
            }
        }
    }
}
