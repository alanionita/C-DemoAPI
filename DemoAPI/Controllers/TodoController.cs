using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using DemoAPI.Models;
using System.Linq;

namespace DemoAPI.Controllers
{
    [Route("api/todos")]
    public class TodoController : Controller
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                _context.TodoItems.Add(new TodoItem { Name = "Start-up todo" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id) 
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (item == null) 
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest("must provide a body for the response");
            }
            if (item.Name == null) 
            {
                return BadRequest("todos must have a name field");     
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }
    }
}