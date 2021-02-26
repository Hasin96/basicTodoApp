using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
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

        private readonly string _url = "https://localhost:5001/Todos";

        public TodoDriver()
        {
            _db = new Database();

            _todos = new List<Todo>()
            {
                new Todo() { Description = "test" },
                new Todo() { Description = "test1"}
            };
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
    }
}
