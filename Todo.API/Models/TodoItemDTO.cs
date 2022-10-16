namespace TodoApi.Models
{
    #region snippet
    /// <summary>DTO задачи</summary>
    public class TodoItemDTO
    {
        /// <summary>идентификатор</summary>
        public long Id { get; set; }
        /// <summary>наименование</summary>
        public string Name { get; set; }
        /// <summary>выполнено</summary>
        public bool IsComplete { get; set; }
    }
    #endregion
}
