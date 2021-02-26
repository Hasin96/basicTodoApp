using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private TodoContext _context  = new TodoContext();

        [HttpPost]
        public async Task<ActionResult<Todo>> Post(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }
    }
}
