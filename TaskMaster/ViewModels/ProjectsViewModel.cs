using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskMaster.Models;
using TaskMaster.Data;
using TaskMaster.Services;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace TaskMaster.ViewModels
{
    public partial class ProjectsViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ObservableCollection<Projet> projets = new();

        [ObservableProperty]
        private string nouveauProjetNom;

        [ObservableProperty]
        private string nouveauProjetDescription;

        public ProjectsViewModel(AppDbContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
            ChargerProjets();
        }

        private async void ChargerProjets()
        {
            var currentUser = _authService.CurrentUser;
            if (currentUser == null) return;

            var projets = await _context.Projets
                .Where(p => p.Id_Createur == currentUser.Id_User)
                .ToListAsync();

            Projets.Clear();
            foreach (var projet in projets)
            {
                Projets.Add(projet);
            }
        }

        [RelayCommand]
        public async Task CreerProjetAsync()
        {
            try
            {
                var currentUser = _authService.CurrentUser;
                if (currentUser == null)
                {
                    await Shell.Current.DisplayAlert("Erreur", "Vous devez être connecté pour créer un projet", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(NouveauProjetNom))
                {
                    await Shell.Current.DisplayAlert("Erreur", "Le nom du projet est requis", "OK");
                    return;
                }

                var projet = new Projet
                {
                    Nom = NouveauProjetNom,
                    Description = NouveauProjetDescription,
                    DateCreation = DateTime.Now,
                    Id_Createur = currentUser.Id_User
                };

                _context.Projets.Add(projet);
                await _context.SaveChangesAsync();

                Projets.Add(projet);

                // Réinitialiser les champs
                NouveauProjetNom = string.Empty;
                NouveauProjetDescription = string.Empty;

                await Shell.Current.DisplayAlert("Succès", "Projet créé avec succès !", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erreur", ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task NaviguerVersProjet(Projet projet)
        {
            if (projet == null) return;
            await Shell.Current.GoToAsync($"//ProjectDetailsPage?projectId={projet.Id_Projet}");
        }
    }
} 