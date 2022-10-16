using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Todo.API.Services;
using Todo.DAL.Models;
using Todo.DAL.Repositories;
using Xunit;


namespace Todo.Tests
{
    public class TodoServiceTests
    {
        private readonly IFixture _fixture;
        private readonly ITodoService _todoService;
        private readonly Mock<ITodoRepository> _todoRepository;


        public TodoServiceTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _todoRepository = _fixture.Freeze<Mock<ITodoRepository>>();
            _todoService = _fixture.Build<ITodoService>().Create<TodoService>();
        }

        #region Update testing 
        [Fact]
        public async void TodoItem_Update_NotFoundById_ThrowArgumentOutOfRangeException()
        {
            // Arrange
            var todoItem = _fixture.Build<TodoItem>()
                .Without(t => t.Secret)
                .Create();

            _todoRepository
                .Setup(r => r.GetAsync(todoItem.Id))
                .Returns(Task.FromResult<TodoItem>(null));

            // Act            
            Func<Task> Act = async () => { await _todoService.UpdateAsync(todoItem); };

            // Assert
            await Act.Should()
                .ThrowAsync<ArgumentOutOfRangeException>()
                .WithParameterName("Id")
                .WithMessage("Не найдена задача с указанным Id*");
        }


        [Fact]
        public async void TodoItem_Update_CannotSave_ThrowArgumentOutOfRangeException()
        {
            // Arrange
            var todoItem = _fixture.Build<TodoItem>()
                .Without(t => t.Secret)
                .Create();

            _todoRepository
                .Setup(r => r.GetAsync(todoItem.Id))
                .Returns(Task.FromResult(todoItem));

            _todoRepository
                .Setup(r => r.SaveChangesAsync())
                .Throws<DbUpdateConcurrencyException>();

            _todoRepository
                .Setup(r => r.Get())
                .Returns(Enumerable.Empty<TodoItem>());

            // Act
            Func<Task> Act = async () => { await _todoService.UpdateAsync(todoItem); };

            // Assert
            await Act.Should()
                .ThrowExactlyAsync<ArgumentOutOfRangeException>()
                .WithMessage("Ошибка параллелизма! Не найдена задача с указанным Id");
        }


        [Fact]
        public async void TodoItem_Update_CannotSave_ThrowDbUpdateConcurrencyException()
        {
            // Arrange
            var todoItem = _fixture.Build<TodoItem>()
                .Without(t => t.Secret)
                .Create();

            _todoRepository
                .Setup(r => r.GetAsync(todoItem.Id))
                .Returns(Task.FromResult(todoItem));

            _todoRepository
                .Setup(r => r.SaveChangesAsync())
                .Throws<DbUpdateConcurrencyException>();

            _todoRepository
                .Setup(r => r.Get())
                .Returns(Enumerable.Empty<TodoItem>().Append(todoItem));

            // Act
            Func<Task> Act = async () => { await _todoService.UpdateAsync(todoItem); };

            // Assert
            await Act.Should()
                .ThrowExactlyAsync<DbUpdateConcurrencyException>();
        }

        [Fact]
        public async void TodoItem_Update_Ok()
        {
            // Arrange
            var todoItem = _fixture.Build<TodoItem>()
                .Without(t => t.Secret)
                .Create();

            var todoItemChanged = _fixture.Build<TodoItem>()
                .With(t => t.Id, todoItem.Id)
                .Without(t => t.Secret)
                .Create();

            _todoRepository
                .Setup(r => r.GetAsync(todoItem.Id))
                .Returns(Task.FromResult(todoItem));

            _todoRepository
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            Func<Task> Act = async () => { await _todoService.UpdateAsync(todoItemChanged); };

            // Assert
            await Act.Should().NotThrowAsync();
            todoItem.Name.Should().Be(todoItemChanged.Name);
            todoItem.IsComplete.Should().Be(todoItemChanged.IsComplete);
        }
        #endregion

        #region Remove testing
        [Fact]
        public async void TodoItem_Remove_NotFoundById_ThrowArgumentOutOfRangeException()
        {
            // Arrange
            _todoRepository
                .Setup(r => r.GetAsync(It.IsAny<long>()))
                .Returns(Task.FromResult<TodoItem>(null));

            // Act
            Func<Task> Act = async () => { await _todoService.RemoveAsync(It.IsAny<long>()); };

            // Assert
            await Act.Should()
                .ThrowAsync<ArgumentOutOfRangeException>()
                .WithParameterName("Id")
                .WithMessage("Не найдена задача с указанным Id*");
        }
        #endregion
    }
}
