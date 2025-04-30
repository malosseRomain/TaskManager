# TaskMaster

Projet de développement avancé R6.A.05 - Application de gestion de tâches multiplateforme

## Description

TaskMaster est une application de gestion de tâches développée avec .NET MAUI, permettant aux utilisateurs de gérer efficacement leurs tâches et projets. L'application suit une architecture MVVM et utilise Entity Framework Core avec MySQL pour la persistance des données.

## Fonctionnalités principales

- 🔐 Authentification et gestion des utilisateurs
- 📝 Création, modification et suppression de tâches
- 📋 Gestion des sous-tâches, étiquettes et commentaires
- 📊 Suivi de l'évolution des tâches via différents statuts
- 📅 Visualisation des tâches par priorité, échéance ou catégorie
- 👥 Attribution de tâches à d'autres utilisateurs
- 📂 Organisation des tâches par projets

## Architecture technique

### Backend

- **Base de données**: MySQL 8.0
- **ORM**: Entity Framework Core
- **Pattern**: MVVM (Model-View-ViewModel)
- **Framework**: .NET MAUI

### Modèle de données

L'application utilise les entités suivantes :

- `TaskItem`: Tâche principale avec titre, description, dates, statut, priorité, etc.
- `SubTask`: Sous-tâches associées à une tâche principale
- `User`: Utilisateurs de l'application
- `Projet`: Projets regroupant des tâches
- `Commentaire`: Commentaires sur les tâches

### Statuts des tâches

- À faire
- En cours
- Terminée
- Annulée

### Priorités

- Basse
- Moyenne
- Haute
- Critique

## Organisation du projet

```
/TaskMaster
│
├── /Data                           # Logique backend (accès à la DB, services, etc.)
│   ├── /Repositories               # Repositories pour interagir avec la base de données
│   ├── /DbContext.cs               # Configuration de la base de données avec EF Core
│   ├── /Migrations                 # Dossier pour les migrations de la base de données
│   └── /Services                   # Services métiers (CRUD, logique métier)
│
├── /Models                         # Entités de la base de données
│   ├── /TaskItem.cs               # Entité principale des tâches
│   ├── /User.cs                   # Entité utilisateur
│   ├── /SubTask.cs                # Entité sous-tâche
│   ├── /Projet.cs                 # Entité projet
│   └── /Commentaire.cs            # Entité commentaire
│
├── /TaskMaster (Projet MAUI)      # Interface utilisateur
│   ├── /Views                     # Pages MAUI (XAML)
│   ├── /ViewModels                # ViewModels pour chaque page
│   ├── /Resources                 # Ressources (images, styles)
│   └── /Converters                # Converters pour la liaison de données
```

## Installation et configuration

1. Cloner le dépôt
2. Configurer la base de données MySQL (voir docker-compose.yaml)
3. Exécuter les migrations Entity Framework
4. Lancer l'application

## Développement

Le projet suit une méthodologie Agile/Scrum avec des sprints hebdomadaires. La gestion du projet est assurée via GitHub Boards.

## Technologies utilisées

- .NET MAUI pour l'interface utilisateur multiplateforme
- Entity Framework Core avec MySQL (Pomelo.EntityFrameworkCore.MySql)
- CommunityToolkit.MVVM pour l'implémentation du pattern MVVM
- GitHub pour le versionnement et la gestion de projet

## Implémentation des fonctionnalités

### Authentification et gestion des utilisateurs

- Inscription et connexion via `AuthService`
- Hachage sécurisé des mots de passe avec BCrypt
- Gestion des sessions utilisateur
- Validation des champs de formulaire

### Gestion des tâches

- CRUD complet des tâches via `TaskService`
- Système de sous-tâches avec gestion des statuts
- Attribution des tâches à des utilisateurs (auteur et réalisateur)
- Association des tâches à des projets
- Gestion des étiquettes et commentaires

### Interface utilisateur

- Navigation entre les pages avec `NavigationService`
- Filtrage et tri des tâches par :
  - Titre
  - Priorité
  - Échéance
  - Catégorie
- Affichage des statuts avec des converters
- Interface responsive adaptée aux différentes plateformes

### Gestion des projets

- Création et visualisation des projets
- Association des tâches aux projets
- Vue détaillée des projets avec leurs tâches associées

### Fonctionnalités de base

- Création, modification et suppression de tâches
- Gestion des sous-tâches
- Système de commentaires
- Attribution des tâches à d'autres utilisateurs
- Organisation des tâches par projets

## Base de données

### MCD

[![](https://mermaid.ink/img/pako:eNqtVE2P2jAQ_SuWzyxi-Uxy2wYqtSt2ER-XCgkZMg3uEpuOx2q3wH-vE0hKMVQc6pM97834zYxmdnylE-ARB-xLkaLI5oq5M32aPLP9_uFB79hk9mFRvCO20oqEVMYnxa_D4eBl6jhrcQnv92w2GYxzfwRBkCyW7zcpwhiZKschfeSUkS9oltYab4QajV8_D-JczBI2WqWmClYit2UdeQWwO97zIxWxT8liZgDZ6PmP3RBKlbIXnXm2EYK6Yh5kQm58skv7h8bkCBxKGaVcX8kI9Tege7X0waxQbklq5UWKi-Qtso9nsRJnIpkB67tLwahcK3FFxX1lU2HerumaSkK4T9k_fvfgwWoNQq3OIoOyGYsdlGqUl_YRSmelS_OEBFnym0XyuwUiMF6aT9Yr2gkZg9hII27Bp86VSFXOasz8kk7s8j9U9Wqi503zJJWj5yuKdZZBvgsQrqmK3Z5w393d0fPxulKyv9XxGk9RJjwitFDjGaAbKffkhcw5pzVkMOeRuyYC3-Z8rnKfrVBftM5KN9Q2XfPoq9gY97LbXOFpBVYUUAlgrK0iHoVFBB7t-E8eBa16s_HY7obt5mMr6PaaNf7Oo8dGvR2GYdDttoNOJ-z0DjX-q_iyUQ96nYY7DmuGQTNs1TgkkjQOj-u32MKH31YOqSQ?type=png)](https://mermaid.live/edit#pako:eNqtVE2P2jAQ_SuWzyxi-Uxy2wYqtSt2ER-XCgkZMg3uEpuOx2q3wH-vE0hKMVQc6pM97834zYxmdnylE-ARB-xLkaLI5oq5M32aPLP9_uFB79hk9mFRvCO20oqEVMYnxa_D4eBl6jhrcQnv92w2GYxzfwRBkCyW7zcpwhiZKschfeSUkS9oltYab4QajV8_D-JczBI2WqWmClYit2UdeQWwO97zIxWxT8liZgDZ6PmP3RBKlbIXnXm2EYK6Yh5kQm58skv7h8bkCBxKGaVcX8kI9Tege7X0waxQbklq5UWKi-Qtso9nsRJnIpkB67tLwahcK3FFxX1lU2HerumaSkK4T9k_fvfgwWoNQq3OIoOyGYsdlGqUl_YRSmelS_OEBFnym0XyuwUiMF6aT9Yr2gkZg9hII27Bp86VSFXOasz8kk7s8j9U9Wqi503zJJWj5yuKdZZBvgsQrqmK3Z5w393d0fPxulKyv9XxGk9RJjwitFDjGaAbKffkhcw5pzVkMOeRuyYC3-Z8rnKfrVBftM5KN9Q2XfPoq9gY97LbXOFpBVYUUAlgrK0iHoVFBB7t-E8eBa16s_HY7obt5mMr6PaaNf7Oo8dGvR2GYdDttoNOJ-z0DjX-q_iyUQ96nYY7DmuGQTNs1TgkkjQOj-u32MKH31YOqSQ)
