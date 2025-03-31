# TaskManager
Projet de développement avancé R6.A.05

## Organisation des fichiers

```
/TaskManager
│
├── /Data                           # Logique backend (accès à la DB, services, etc.)
│   ├── /Repositories               # Repositories pour interagir avec la base de données
│   ├── /DbContext.cs               # Configuration de la base de données avec EF Core
│   ├── /Migrations                 # Dossier pour les migrations de la base de données
│   └── /Services                   # Services métiers (CRUD, logique métier)
│
├── /Models                          # Contient les entités de la base de données (EF Core)
│   ├── /Task.cs                    # Entité Task
│   ├── /User.cs                    # Entité User
│   └── /SubTask.cs                 # Entité SubTask
│
├── /TaskManager (Projet MAUI)       # Interface utilisateur (Frontend)
│   ├── /Views                       # Pages MAUI (XAML) représentant l'interface utilisateur
│   ├── /ViewModels                  # ViewModels pour chaque page
│   ├── /Resources                   # Ressources (images, styles, etc.)
│   ├── /Converters                  # Converters pour la liaison de données
│   └── /App.xaml.cs                 # Point d'entrée de l'application
│
├── /Properties                      # Contient les fichiers de configuration (ne pas toucher)
├── /Platforms                       # Contient des spécificités de la plateforme (iOS, Android, etc.)
├── /TaskManager.sln                 # Fichier de solution
└── /obj                             # Dossier de compilation (ne pas toucher)
```
