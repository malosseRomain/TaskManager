# TaskMaster

Projet de développement avancé R6.A.05

## Organisation des fichiers

```
/TaskMaster
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
├── /TaskMaster (Projet MAUI)       # Interface utilisateur (Frontend)
│   ├── /Views                       # Pages MAUI (XAML) représentant l'interface utilisateur
│   ├── /ViewModels                  # ViewModels pour chaque page
│   ├── /Resources                   # Ressources (images, styles, etc.)
│   ├── /Converters                  # Converters pour la liaison de données
│   └── /App.xaml.cs                 # Point d'entrée de l'application
│
├── /Properties                      # Contient les fichiers de configuration (ne pas toucher)
├── /Platforms                       # Contient des spécificités de la plateforme (iOS, Android, etc.)
├── /TaskMaster.sln                 # Fichier de solution
└── /obj                             # Dossier de compilation (ne pas toucher)
```

## Base de données

### MCD

[![](https://mermaid.ink/img/pako:eNqtVE2P2jAQ_SuWzyxi-Uxy2wYqtSt2ER-XCgkZMg3uEpuOx2q3wH-vE0hKMVQc6pM97834zYxmdnylE-ARB-xLkaLI5oq5M32aPLP9_uFB79hk9mFRvCO20oqEVMYnxa_D4eBl6jhrcQnv92w2GYxzfwRBkCyW7zcpwhiZKschfeSUkS9oltYab4QajV8_D-JczBI2WqWmClYit2UdeQWwO97zIxWxT8liZgDZ6PmP3RBKlbIXnXm2EYK6Yh5kQm58skv7h8bkCBxKGaVcX8kI9Tege7X0waxQbklq5UWKi-Qtso9nsRJnIpkB67tLwahcK3FFxX1lU2HerumaSkK4T9k_fvfgwWoNQq3OIoOyGYsdlGqUl_YRSmelS_OEBFnym0XyuwUiMF6aT9Yr2gkZg9hII27Bp86VSFXOasz8kk7s8j9U9Wqi503zJJWj5yuKdZZBvgsQrqmK3Z5w393d0fPxulKyv9XxGk9RJjwitFDjGaAbKffkhcw5pzVkMOeRuyYC3-Z8rnKfrVBftM5KN9Q2XfPoq9gY97LbXOFpBVYUUAlgrK0iHoVFBB7t-E8eBa16s_HY7obt5mMr6PaaNf7Oo8dGvR2GYdDttoNOJ-z0DjX-q_iyUQ96nYY7DmuGQTNs1TgkkjQOj-u32MKH31YOqSQ?type=png)](https://mermaid.live/edit#pako:eNqtVE2P2jAQ_SuWzyxi-Uxy2wYqtSt2ER-XCgkZMg3uEpuOx2q3wH-vE0hKMVQc6pM97834zYxmdnylE-ARB-xLkaLI5oq5M32aPLP9_uFB79hk9mFRvCO20oqEVMYnxa_D4eBl6jhrcQnv92w2GYxzfwRBkCyW7zcpwhiZKschfeSUkS9oltYab4QajV8_D-JczBI2WqWmClYit2UdeQWwO97zIxWxT8liZgDZ6PmP3RBKlbIXnXm2EYK6Yh5kQm58skv7h8bkCBxKGaVcX8kI9Tege7X0waxQbklq5UWKi-Qtso9nsRJnIpkB67tLwahcK3FFxX1lU2HerumaSkK4T9k_fvfgwWoNQq3OIoOyGYsdlGqUl_YRSmelS_OEBFnym0XyuwUiMF6aT9Yr2gkZg9hII27Bp86VSFXOasz8kk7s8j9U9Wqi503zJJWj5yuKdZZBvgsQrqmK3Z5w393d0fPxulKyv9XxGk9RJjwitFDjGaAbKffkhcw5pzVkMOeRuyYC3-Z8rnKfrVBftM5KN9Q2XfPoq9gY97LbXOFpBVYUUAlgrK0iHoVFBB7t-E8eBa16s_HY7obt5mMr6PaaNf7Oo8dGvR2GYdDttoNOJ-z0DjX-q_iyUQ96nYY7DmuGQTNs1TgkkjQOj-u32MKH31YOqSQ)
