namespace LearningWpfProject.Helper
{
    public enum TaskState
    {
        All,
        New,
        InProgress,
        Done
    }

    public static class TaskStateHelper
    {
        public static IEnumerable<TaskState> GetAllValues => Enum.GetValues<TaskState>()
            .Cast<TaskState>();

        public static IEnumerable<TaskState> GetValues => Enum
           .GetValues<TaskState>()
           .Skip(1)
           .Cast<TaskState>();
    }

}
