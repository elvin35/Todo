using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DAL.Models;


namespace Todo.DAL.Repositories
{
    /// <summary>Репозиторий для работы с задачами</summary>
    public interface ITodoRepository
    {
        /// <summary>Получить список задач</summary>
        public IEnumerable<TodoItem> Get();

        /// <summary>Получить список задач</summary>
        public Task<List<TodoItem>> GetAsync();

        /// <summary>Получить задачу</summary>
        /// <param name="id">идентификатор задачи</param>
        public Task<TodoItem> GetAsync(long id);

        /// <summary>Добавить задачу</summary>
        /// <param name="item">объект задачи</param>
        public void Add(TodoItem item);

        /// <summary>Обновить задачу</summary>
        /// <param name="todoItem">объект задачи</param>
        public void Update(TodoItem todoItem);

        /// <summary>Удалить задачу</summary>
        /// <param name="todoItem">объект задачи</param>
        public void Remove(TodoItem todoItem);

        /// <summary>Сохранить изменения контекста</summary>
        public Task SaveChangesAsync();
    }
}
