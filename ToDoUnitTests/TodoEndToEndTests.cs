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
        public void AddNewTodo()
        {
            _driver.ShowsNewlyCreatedTodo();
        }

        [Test]
        public void AddTwoNewTodo()
        {
            var _todo = new Todo() { Description = "Description" };
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://localhost:5001/Todos");
                driver.FindElement(By.CssSelector("#todo")).SendKeys(_todo.Description);
                driver.FindElement(By.CssSelector("form")).Submit();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
                wait.Until(driver => driver.FindElement(By.CssSelector("#todos li:first-child")).Displayed);

                IWebElement newTodo = driver.FindElement(By.CssSelector("#todos li:first-child"));

                Assert.IsNotNull(newTodo);
                StringAssert.Contains(_todo.Description, newTodo.GetAttribute("textContent"));

                driver.FindElement(By.CssSelector("#todo")).SendKeys("test");
                driver.FindElement(By.CssSelector("form")).Submit();

                wait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
                wait.Until(driver => driver.FindElement(By.CssSelector("#todos li:first-child")).Displayed);

                newTodo = driver.FindElement(By.CssSelector("#todos li:first-child"));

                Assert.IsNotNull(newTodo);
                StringAssert.Contains("test", newTodo.GetAttribute("textContent"));
            }

            _context.Todos.RemoveRange(_context.Todos);
            _context.SaveChanges();
        }

        [Test]
        public void AddNullTodo_InputShouldShowRedMark()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://localhost:5001/Todos");
                driver.FindElement(By.CssSelector("form")).Submit();

                IWebElement result = driver.FindElement(By.CssSelector(".form-error"));

                Assert.IsNotNull(result);
            }
        }

        [Test]
        public void AddTodoAndMarkComplete_ShouldHaveCrossClass()
        {
            var _todo = new Todo() { Description = "Description" };
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl("https://localhost:5001/Todos");
                driver.FindElement(By.CssSelector("#todo")).SendKeys(_todo.Description);
                driver.FindElement(By.CssSelector("form")).Submit();

                _context.Todos.RemoveRange(_context.Todos);
                _context.SaveChanges();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(driver => driver.FindElement(By.CssSelector("#todos li:first-child")).Displayed);

                IWebElement newTodo = driver.FindElement(By.CssSelector("#todos li:first-child span"));

                IWebElement toggleCrossBtn = driver.FindElement(By.CssSelector(".toggleCrossBtn"));
                toggleCrossBtn.Click();
                
                Assert.IsTrue(newTodo.GetAttribute("class").Contains("complete"));
            }
        }

        [TearDown]
        public void TearDown()
        {
            _context.Todos.RemoveRange(_context.Todos);
            _context.SaveChanges();
        }
    }
}
