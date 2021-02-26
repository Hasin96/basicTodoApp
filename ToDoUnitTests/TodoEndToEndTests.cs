using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.Transactions;
using ToDo;
using ToDo.Models;
using System.Threading.Tasks;
using System.Threading;

namespace TodoAcceptanceTests
{
    [TestFixture]
    public class TodoEndToEndTests
    {
        private TodoDriver _driver = new TodoDriver();
        private TodoContext _context = new TodoContext();

        public TodoEndToEndTests()
        {
          
        }

        [SetUp]
        public void SetUp()
        {
            _context.Todos.RemoveRange(_context.Todos);
            _context.SaveChanges();
        }

        [Test]
        public void GetAllTodo()
        {
            _driver.ShowsAllTodos();
        }

        [Test]
        public void AddTwoNewTodos()
        {
            _driver.ShowsNewlyCreatedTodos();
        }

        [Test]
        public void AddNullTodo_InputShouldShowRedMark()
        {
            _driver.ShowsErrorWhenAddingAnEmptyTodo();
        }

        [Test]
        public void AddTodoAndMarkComplete_ShouldHaveCrossClass()
        {
            _driver.ShowsLineAccrossCompletedTodo();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Todos.RemoveRange(_context.Todos);
            _context.SaveChanges();
        }
    }
}
