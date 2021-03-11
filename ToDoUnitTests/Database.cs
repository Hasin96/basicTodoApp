using System;
using System.Collections.Generic;
using System.Text;
using ToDo;
using ToDo.Models;

namespace TodoAcceptanceTests
{
    public class Database
    {
        private TodoContext _context = new TodoContext();
        public void Add(Todo todo)
        {
            _context.Add(todo);
            _context.SaveChanges();
        }

        public void AddRange(List<Todo> todos)
        {
            _context.AddRange(todos);
            _context.SaveChanges();
        }

        public void Reset()
        {
            _context.RemoveRange(_context.Todos);
            _context.SaveChanges();
        }
    }
}
