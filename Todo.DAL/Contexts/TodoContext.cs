using Microsoft.EntityFrameworkCore;
using Todo.DAL.Models;


namespace Todo.Contexts.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; }


        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }
    }
}