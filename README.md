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

## Base de données

### MCD

[![](https://mermaid.ink/img/pako:eNqNVNFu2jAU_RXLzxQFSEKStw4yqUJQVNjLlgl5yV1ijdjMuZbWBf69N0Db0QQxv8Q-5_hcH9txzVOdAY84mKkUuRFlohi19f1qtnlYx3O239_d6ZqtvnzaNCCLWKoVCqmqK8rJ43weL9YkLESnZr9ny_hp9bhovAwIhGzz4_mWUlSVzBVJUZ-kr3VaQouFNp2e9QlomlTIHjK2nL1DFRqpcraWuIUWOoUqNXKHUqt3LqO1oyyBTakzaaJ003FagFDpP66gbMlWKNDiB3BppDYS2yuYkFtOVIv59p3FKH9bQITqMuG9RbCGcn6eXRJPILayEh_Yw-nzdtb1rc3pjHEl9uHy2Oor29jOTdeN6twIdnY_34L_OuiFLlvY0oDqgONSyO25EO_x3MiMR2gs9HgJhkga8mPRhGMBJSQ8om4mzK-EJ6qZsxPqq9bl6zSjbV7w6KfYVjSyu2YPzn_gG0prycBMtFXIo4HvHk14VPM_PApG_aEzcP3QHQ5GgT8e9vgziZy-G4Zh4Ptu4HmhNz70-N9jVacfjD2H2mjseaR3yA0yidrMTw_A8R04vAAWDzT_?type=png)](https://mermaid.live/edit#pako:eNqNVNFu2jAU_RXLzxQFSEKStw4yqUJQVNjLlgl5yV1ijdjMuZbWBf69N0Db0QQxv8Q-5_hcH9txzVOdAY84mKkUuRFlohi19f1qtnlYx3O239_d6ZqtvnzaNCCLWKoVCqmqK8rJ43weL9YkLESnZr9ny_hp9bhovAwIhGzz4_mWUlSVzBVJUZ-kr3VaQouFNp2e9QlomlTIHjK2nL1DFRqpcraWuIUWOoUqNXKHUqt3LqO1oyyBTakzaaJ003FagFDpP66gbMlWKNDiB3BppDYS2yuYkFtOVIv59p3FKH9bQITqMuG9RbCGcn6eXRJPILayEh_Yw-nzdtb1rc3pjHEl9uHy2Oor29jOTdeN6twIdnY_34L_OuiFLlvY0oDqgONSyO25EO_x3MiMR2gs9HgJhkga8mPRhGMBJSQ8om4mzK-EJ6qZsxPqq9bl6zSjbV7w6KfYVjSyu2YPzn_gG0prycBMtFXIo4HvHk14VPM_PApG_aEzcP3QHQ5GgT8e9vgziZy-G4Zh4Ptu4HmhNz70-N9jVacfjD2H2mjseaR3yA0yidrMTw_A8R04vAAWDzT_)
