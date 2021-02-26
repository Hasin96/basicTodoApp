using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using ToDo.Models;

namespace TodoAcceptanceTests
{
    public class TodoDriver
    {
        private Database _db;
        private List<Todo> _todos;
        private Todo _todo;


        private readonly string _url = "https://localhost:5001/Todos";

        public TodoDriver()
        {
            _db = new Database();

            _todos = new List<Todo>()
            {
                new Todo() { Description = "test" },
                new Todo() { Description = "test1"}
            };

            _todo = new Todo() { Description = "Description" };
        }

        public void ShowsAllTodos()
        {
            _db.AddRange(_todos);

            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(_url);
                IList<IWebElement> results = driver.FindElements(By.CssSelector("#todos li"));

                Assert.IsTrue(results.Count == 2);
                Assert.AreEqual(_todos.Count, results.Count);
                StringAssert.Contains(_todos[0].Description, results[0].GetAttribute("textContent"));
                StringAssert.Contains(_todos[1].Description, results[1].GetAttribute("textContent"));
            }

            _db.Reset();
        }

        public void ShowsNewlyCreatedTodos()
        {
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

            _db.Reset();
        }
    }
}
