namespace LearningWpfProject.Helper
{
    public enum TaskState
    {
        New,
        InProgress,
        Done
    }

    public static class TaskStateHelper
    {
        public static IEnumerable<TaskState> GetValues => Enum.GetValues<TaskState>().Cast<TaskState>();
    }

}
