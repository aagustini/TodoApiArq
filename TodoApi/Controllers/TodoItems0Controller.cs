using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades.Modelos;
using Entidades.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersistenceLayer.Repositorisos;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [Route("Todos")]
    [ApiController]
    public class TodoItems0Controller : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItems0Controller(TodoContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _context.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(todoItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            if (id != todoItemDTO.Id)
            {
                return BadRequest();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = todoItemDTO.Name;
            todoItem.IsComplete = todoItemDTO.IsComplete;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoItemDTO.IsComplete,
                Name = todoItemDTO.Name
            };


            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(long id) =>
             _context.TodoItems.Any(e => e.Id == id);

        private static TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new TodoItemDTO
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }

    //// GET: Todos/
    //[HttpGet]
    //[HttpGet("listagem")]
    //public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
    //{
    //    return await _context.TodoItems.ToListAsync();
    //}

    //// GET: Todos/5
    //[HttpGet("{id}")]
    //public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
    //{
    //    var todoItem = await _context.TodoItems.FindAsync(id);

    //    if (todoItem == null)
    //    {
    //        return NotFound();
    //    }

    //    return todoItem;
    //}

    //// PUT: api/TodoItems/5
    //// To protect from overposting attacks, enable the specific properties you want to bind to, for
    //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    //[HttpPut("{id}")]
    //public async Task<IActionResult> PutTodoItem(long id, TodoItem todoItem)
    //{
    //    if (id != todoItem.Id)
    //    {
    //        return BadRequest();
    //    }

    //    _context.Entry(todoItem).State = EntityState.Modified;

    //    try
    //    {
    //        await _context.SaveChangesAsync();
    //    }
    //    catch (DbUpdateConcurrencyException)
    //    {
    //        if (!TodoItemExists(id))
    //        {
    //            return NotFound();
    //        }
    //        else
    //        {
    //            throw;
    //        }
    //    }

    //    return NoContent();
    //}

    //// POST: api/TodoItems
    //// To protect from overposting attacks, enable the specific properties you want to bind to, for
    //// more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    //[HttpPost]
    //public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
    //{
    //    _context.TodoItems.Add(todoItem);
    //    await _context.SaveChangesAsync();

    //    return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
    //}

    //// DELETE: api/TodoItems/5
    //[HttpDelete("{id}")]
    //public async Task<ActionResult<TodoItem>> DeleteTodoItem(long id)
    //{
    //    var todoItem = await _context.TodoItems.FindAsync(id);
    //    if (todoItem == null)
    //    {
    //        return NotFound();
    //    }

    //    _context.TodoItems.Remove(todoItem);
    //    await _context.SaveChangesAsync();

    //    return todoItem;
    //}

    //private bool TodoItemExists(long id)
    //{
    //    return _context.TodoItems.Any(e => e.Id == id);
    //}
    //}
}
