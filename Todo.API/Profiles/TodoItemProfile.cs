using AutoMapper;
using Todo.DAL.Models;
using TodoApi.Models;


namespace Todo.API.Profiles
{
    /// <summary>Маппер для TodoItem</summary>
    public class TodoItemProfile : Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemDTO>().ReverseMap();
            CreateMap<TodoItem, TodoItemCreateDTO>().ReverseMap();
        }
    }
}
