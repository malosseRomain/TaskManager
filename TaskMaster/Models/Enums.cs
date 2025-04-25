namespace TaskMaster.Models
{
    public enum TaskStatus
    {
        Afaire,
        EnCours,
        Terminee,
        Annulee
    }

    public enum TaskPriority
    {
        Basse,
        Moyenne,
        Haute,
        Critique
    }

    public enum TaskCategory
    {
        Travail,
        Personnel,
        Urgent,
        Important
    }

    public class StatusDisplay
    {
        public string Value { get; set; }
        public string DisplayText { get; set; }

        public StatusDisplay(string value, string displayText)
        {
            Value = value;
            DisplayText = displayText;
        }

        public static List<StatusDisplay> GetStatusDisplays()
        {
            return new List<StatusDisplay>
            {
                new StatusDisplay(TaskStatus.Afaire.ToString(), "À faire"),
                new StatusDisplay(TaskStatus.EnCours.ToString(), "En cours"),
                new StatusDisplay(TaskStatus.Terminee.ToString(), "Terminée"),
                new StatusDisplay(TaskStatus.Annulee.ToString(), "Annulée")
            };
        }
    }
} 