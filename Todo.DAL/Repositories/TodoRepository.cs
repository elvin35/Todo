using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Contexts.Models;
using Todo.DAL.Models;


namespace Todo.DAL.Repositories
{
    /// <inheritdoc/>
    public class TodoRepository : ITodoRepository
    {
        private readonly TodoContext _context;


        public TodoRepository(TodoContext context) =>
            _context = context;

        /// <inheritdoc/>
        public IEnumerable<TodoItem> Get() =>
            _context.TodoItems.AsEnumerable();

        /// <inheritdoc/>
        public async Task<List<TodoItem>> GetAsync() =>
            await _context.TodoItems.ToListAsync();

        /// <inheritdoc/>
        public async Task<TodoItem> GetAsync(long id) =>
            await _context.TodoItems.FindAsync(id);

        /// <inheritdoc/>
        public void Add(TodoItem todoItem) =>
            _context.TodoItems.Add(todoItem);

        /// <inheritdoc/>
        public void Update(TodoItem todoItem) =>
            _context.Update(todoItem);

        /// <inheritdoc/>
        public void Remove(TodoItem todoItem) =>
            _context.Remove(todoItem);

        /// <inheritdoc/>
        public async Task SaveChangesAsync() =>
            await _context.SaveChangesAsync();
    }
}
