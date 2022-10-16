using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;
using Todo.API.Services;
using Todo.DAL.Models;
using TodoApi.Models;


namespace TodoApi.Controllers
{
    /// <summary>API для работы с задачами</summary>
    [Route("api/[controller]")] 
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoService _todoService;
        private readonly IMapper _mapper;

        /// <summary></summary>
        public TodoItemsController(ITodoService todoService, IMapper mapper)
        {
            _todoService = todoService;
            _mapper = mapper;
        }

        /// <summary>Получить все задачи</summary>
        /// <response code="200">Задачи получены</response>
        /// <response code="500">Внутренняя ошибка</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            var todoItems = await _todoService.GetAsync();
            return _mapper.Map<List<TodoItem>, List<TodoItemDTO>>(todoItems);
        }

        /// <summary>Получить задачу</summary>
        /// <param name="id">идентификатор задачи</param>
        /// <response code="200">Задача получена</response>
        /// <response code="404">Задача не найдена</response>
        /// <response code="500">Внутренняя ошибка</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _todoService.GetAsync(id);
            if (todoItem == null)
                return NotFound();

            return _mapper.Map<TodoItemDTO>(todoItem);
        }

        /// <summary>Обновить задачу</summary>
        /// <param name="id">идентификатор задачи</param>
        /// <param name="todoItemDTO">объект задачи с данными для обновления</param>
        /// <response code="204">Задача обновлена</response>
        /// <response code="400">Некорректный запрос: входные идентификаторы не совпадают</response>
        /// <response code="404">Задача не найдена</response>
        /// <response code="500">Внутренняя ошибка</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
                return BadRequest();

            try
            {
                var todoItem = _mapper.Map<TodoItem>(todoItemDTO);
                await _todoService.UpdateAsync(todoItem);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            return NoContent();
        }

        /// <summary>Создать задачу</summary>
        /// <param name="todoItemDTO">объект задачи</param>
        /// <response code="201">Задача создана</response>
        /// <response code="500">Внутренняя ошибка</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemCreateDTO todoItemDTO)
        {
            var todoItem = _mapper.Map<TodoItem>(todoItemDTO);
            await _todoService.AddAsync(todoItem);

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                _mapper.Map<TodoItemDTO>(todoItem)
            );
        }

        /// <summary>Удалить задачу</summary>
        /// <param name="id">идентификатор задачи</param>
        /// <response code="204">Задача удалена</response>
        /// <response code="404">Задача не найдена</response>
        /// <response code="500">Внутренняя ошибка</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            try
            {
                await _todoService.RemoveAsync(id);
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
