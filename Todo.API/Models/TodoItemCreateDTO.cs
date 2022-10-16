namespace TodoApi.Models
{
    #region snippet
    /// <summary>DTO задачи</summary>
    public class TodoItemCreateDTO
    {
        /// <summary>наименование</summary>
        public string Name { get; set; }
        /// <summary>выполнено</summary>
        public bool IsComplete { get; set; }
    }
    #endregion
}
