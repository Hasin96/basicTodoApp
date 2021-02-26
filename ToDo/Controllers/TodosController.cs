using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo.Models;

namespace ToDo.Controllers
{
    public class TodosController : Controller
    {
        private TodoContext _context = new TodoContext();
        public async Task<IActionResult> Index()
        {
            return View(await _context.Todos.ToListAsync());
        }

    }
}
