-- Création de la base de données
CREATE DATABASE IF NOT EXISTS taskmaster;
USE taskmaster;

-- Création des tables

-- Table des personnes
CREATE TABLE Persons (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nom VARCHAR(100) NOT NULL,
    Prenom VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL
);

-- Table des tâches principales
CREATE TABLE TaskItems (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT NOT NULL,
    DateCreation DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Echeance DATETIME NULL,
    Statut ENUM('Afaire', 'EnCours', 'Terminee', 'Annulee') NOT NULL,
    Priorite ENUM('Basse', 'Moyenne', 'Haute', 'Critique') NOT NULL,
    AuteurId INT NOT NULL,
    RealisateurId INT NOT NULL,
    Categorie VARCHAR(100) NOT NULL,
    FOREIGN KEY (AuteurId) REFERENCES Persons(Id),
    FOREIGN KEY (RealisateurId) REFERENCES Persons(Id)
);

-- Table des sous-tâches
CREATE TABLE SubTasks (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TaskItemId INT NOT NULL,
    Title VARCHAR(255) NOT NULL,
    Statut ENUM('Afaire', 'EnCours', 'Terminee', 'Annulee') NOT NULL,
    Echeance DATETIME NULL,
    FOREIGN KEY (TaskItemId) REFERENCES TaskItems(Id) ON DELETE CASCADE
);

-- Table des commentaires
CREATE TABLE Comments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    TaskItemId INT NOT NULL,
    AuteurId INT NOT NULL,
    Date DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Contenu TEXT NOT NULL,
    FOREIGN KEY (TaskItemId) REFERENCES TaskItems(Id) ON DELETE CASCADE,
    FOREIGN KEY (AuteurId) REFERENCES Persons(Id)
);

-- Table des étiquettes
CREATE TABLE TaskItemEtiquettes (
    TaskItemId INT NOT NULL,
    Etiquette VARCHAR(100) NOT NULL,
    PRIMARY KEY (TaskItemId, Etiquette),
    FOREIGN KEY (TaskItemId) REFERENCES TaskItems(Id) ON DELETE CASCADE
);

-- Insertion des personnes
INSERT INTO Persons (Nom, Prenom, Email) VALUES
('Dupont', 'Jean', 'jean.dupont@example.com'),
('Martin', 'Sophie', 'sophie.martin@example.com'),
('Bernard', 'Pierre', 'pierre.bernard@example.com'),
('Petit', 'Marie', 'marie.petit@example.com'),
('Robert', 'Thomas', 'thomas.robert@example.com');

-- Insertion des tâches principales
INSERT INTO TaskItems (Title, Description, DateCreation, Echeance, Statut, Priorite, AuteurId, RealisateurId, Categorie) VALUES
('Mise à jour du site web', 'Mettre à jour le contenu et les images du site principal', '2024-03-01', '2024-03-15', 'Afaire', 'Moyenne', 1, 2, 'Développement'),
('Préparation réunion client', 'Préparer la présentation et les documents pour la réunion du 20 mars', '2024-03-05', '2024-03-19', 'EnCours', 'Haute', 2, 3, 'Administratif'),
('Développement nouvelle fonctionnalité', 'Implémenter le système de notifications', '2024-03-10', '2024-03-25', 'Afaire', 'Haute', 3, 4, 'Développement'),
('Revue de code', 'Faire une revue complète du code de la dernière version', '2024-03-12', '2024-03-18', 'EnCours', 'Basse', 4, 1, 'Qualité'),
('Migration base de données', 'Migrer les données vers le nouveau serveur', '2024-03-15', '2024-03-30', 'Afaire', 'Haute', 5, 3, 'Infrastructure'),
('Documentation API', 'Mettre à jour la documentation de l''API REST', '2024-03-18', '2024-03-22', 'Afaire', 'Moyenne', 1, 5, 'Documentation');

-- Insertion des sous-tâches
INSERT INTO SubTasks (TaskItemId, Title, Statut, Echeance) VALUES
(1, 'Mettre à jour les images du slider', 'Afaire', '2024-03-10'),
(1, 'Corriger les liens brisés', 'Terminee', '2024-03-12'),
(2, 'Préparer l''agenda de la réunion', 'EnCours', '2024-03-15'),
(2, 'Créer la présentation PowerPoint', 'Afaire', '2024-03-18'),
(3, 'Concevoir le système de notification', 'Afaire', '2024-03-15'),
(3, 'Implémenter les notifications par email', 'Afaire', '2024-03-20'),
(4, 'Vérifier les normes de codage', 'EnCours', '2024-03-15'),
(4, 'Tester les cas limites', 'Afaire', '2024-03-17'),
(5, 'Sauvegarder la base actuelle', 'Terminee', '2024-03-20'),
(5, 'Migrer les données', 'Afaire', '2024-03-25'),
(6, 'Documenter les endpoints', 'Afaire', '2024-03-20'),
(6, 'Ajouter des exemples d''utilisation', 'Afaire', '2024-03-22');

-- Insertion des commentaires
INSERT INTO Comments (TaskItemId, AuteurId, Date, Contenu) VALUES
(1, 2, '2024-03-02', 'Les nouvelles images sont prêtes à être uploadées'),
(1, 1, '2024-03-03', 'J''ai commencé la mise à jour du contenu'),
(2, 3, '2024-03-06', 'L''agenda est en cours de préparation'),
(3, 4, '2024-03-11', 'Le design du système de notification est validé'),
(4, 5, '2024-03-13', 'Première passe de revue de code effectuée'),
(5, 1, '2024-03-16', 'La sauvegarde est planifiée pour demain'),
(6, 2, '2024-03-19', 'J''ai commencé la documentation des endpoints principaux');

-- Insertion des étiquettes
INSERT INTO TaskItemEtiquettes (TaskItemId, Etiquette) VALUES
(1, 'Frontend'),
(1, 'Maintenance'),
(2, 'Client'),
(2, 'Présentation'),
(3, 'Backend'),
(3, 'Nouvelle fonctionnalité'),
(4, 'Qualité'),
(4, 'Code review'),
(5, 'Infrastructure'),
(5, 'Migration'),
(6, 'Documentation'),
(6, 'API'); 