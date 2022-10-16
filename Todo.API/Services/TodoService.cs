using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.DAL.Models;
using Todo.DAL.Repositories;
using Todo.API.Filters;


namespace Todo.API.Services
{
    /// <inheritdoc/>
    public class TodoService : ITodoService
    {
        private readonly ITodoRepository _todoRepository;
        private readonly ILogger<ITodoService> _log;

        public TodoService(ITodoRepository todoRepository, ILogger<TodoService> logger)
        {
            _todoRepository = todoRepository;
            _log = logger;
        }

        /// <inheritdoc/>
        public async Task<TodoItem> GetAsync(long id)
        {
            //_log.LogInformation($"GET({id}) Hello todoService");
            return await _todoRepository.GetAsync(id);
        }

        /// <inheritdoc/>
        public Task<List<TodoItem>> GetAsync() =>
            _todoRepository.GetAsync();

        /// <inheritdoc/>
        public async Task AddAsync(TodoItem todoItem)
        {
            _todoRepository.Add(todoItem);
            await _todoRepository.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(TodoItem todoItem)
        {
            var item = await _todoRepository.GetAsync(todoItem.Id);
            if (item == null)
                throw new ArgumentOutOfRangeException("Id", "Не найдена задача с указанным Id");

            item.Name = todoItem.Name;
            item.IsComplete = todoItem.IsComplete;
            try
            {
                await _todoRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception) when (!TodoItemExists(todoItem.Id))
            {
                throw new ArgumentOutOfRangeException("Ошибка параллелизма! Не найдена задача с указанным Id", exception);
            }
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(long id)
        {
            var todoItem = await _todoRepository.GetAsync(id);
            if (todoItem == null)
                throw new ArgumentOutOfRangeException("Id", "Не найдена задача с указанным Id");

            _todoRepository.Remove(todoItem);
            await _todoRepository.SaveChangesAsync();
        }

        private bool TodoItemExists(long id) =>
            _todoRepository.Get().Any(e => e.Id == id);
    }
}
