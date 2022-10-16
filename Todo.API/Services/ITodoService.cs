using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.DAL.Models;


namespace Todo.API.Services
{
    /// <summary>Сервис для работы с задачами</summary>
    public interface ITodoService
    {
        /// <summary>Получить задачу</summary>
        /// <param name="id">идентификатор задачи</param>
        public Task<TodoItem> GetAsync(long id);

        /// <summary>Получить список всех задач</summary>
        public Task<List<TodoItem>> GetAsync();

        /// <summary>Добавить задачу</summary>
        /// <param name="todoItem">объект задачи</param>
        public Task AddAsync(TodoItem todoItem);

        /// <summary>Обновить задачу</summary>
        /// <param name="todoItem">объект задачи</param>
        public Task UpdateAsync(TodoItem todoItem);

        /// <summary>Удалить задачу</summary>
        /// <param name="id">идентификатор задачи</param>
        public Task RemoveAsync(long id);
    }
}
