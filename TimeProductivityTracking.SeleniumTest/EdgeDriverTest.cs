using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using System;

namespace TimeProductivityTracking.SeleniumTest
{
    [TestClass]
    public class EdgeDriverTest
    {
        // In order to run the below test(s), 
        // please follow the instructions from https://docs.microsoft.com/en-us/microsoft-edge/webdriver-chromium
        // to install Microsoft Edge WebDriver.

        private EdgeDriver _driver;

        [TestInitialize]
        public void EdgeDriverInitialize()
        {
            // Initialize edge driver 
            var options = new EdgeOptions
            {
                PageLoadStrategy = PageLoadStrategy.Normal
            };
            var service = EdgeDriverService.CreateDefaultService(@"C:\Users\ceren\source\repos\TimeProductivityTracking\TimeProductivityTracking.SeleniumTest",
                "msedgedriver.exe");
            _driver = new EdgeDriver(service,options);
        }

        [TestMethod]
        public void LoginTest()
        {
            
            _driver.Url = "https://secmanagement.azurewebsites.net/Identity/Account/Login";

            _driver.FindElement(By.Id("Input_Email")).SendKeys("contractor@sec.ie");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Admin1234!");

            //submit the login form
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            // Wait until the greeting or dashboard is present
            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            var greeting = wait.Until(driver => driver.FindElement(By.CssSelector("a.nav-link.text-dark")));

            Assert.IsTrue(greeting.Text.Contains("Hello contractor@sec.ie"));
        }

        [TestMethod]
        public void LogoutButton_WorksCorrectly()
        {
            _driver.Url = "https://secmanagement.azurewebsites.net/Identity/Account/Login";

            // Login first
            _driver.FindElement(By.Id("Input_Email")).SendKeys("contractor@sec.ie");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Admin1234!");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // Wait for the logout button (form submit button)
            var logoutButton = wait.Until(driver =>
                driver.FindElement(By.CssSelector("form[action*='/Account/Logout'] button[type='submit']")));

            Assert.IsNotNull(logoutButton, "Logout button not found.");

           
            logoutButton.Click();

            // Wait for redirect to login page
            wait.Until(driver => driver.Url.Contains("/Identity/Account/Login"));

            // Assert login form visible again
            Assert.IsTrue(_driver.FindElement(By.Id("Input_Email")).Displayed);


        }


        [TestMethod]
        public void NavigationTest_ContractorPagesAccessibleAfterLogin()
        {
            _driver.Url = "https://secmanagement.azurewebsites.net/Identity/Account/Login";

            // Login
            _driver.FindElement(By.Id("Input_Email")).SendKeys("contractor@sec.ie");
            _driver.FindElement(By.Id("Input_Password")).SendKeys("Admin1234!");
            _driver.FindElement(By.CssSelector("button[type='submit']")).Click();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));

            // 1. Navigate to /Productivities/Create
            _driver.Navigate().GoToUrl("https://secmanagement.azurewebsites.net/Productivities/Create");
                                       
            wait.Until(d => d.Title.Contains("Create"));
            Assert.IsTrue(_driver.PageSource.Contains("Create Productivities"));

            // 2. Navigate to /Productivities/Index
            _driver.Navigate().GoToUrl("https://secmanagement.azurewebsites.net/Productivities");
    
            wait.Until(d => d.PageSource.Contains("Select Month"));
            Assert.IsTrue(_driver.PageSource.Contains("Select Month"));

            // 3. Navigate to /Productivities/ChartByContractor
            _driver.Navigate().GoToUrl("https://secmanagement.azurewebsites.net/Productivities/ChartByContractor");
            wait.Until(d => d.PageSource.Contains("Contractor") || d.FindElements(By.TagName("select")).Count > 0);
            Assert.IsTrue(_driver.PageSource.Contains("Contractor"));

            // 4. Navigate to /Productivities/Invoices
            _driver.Navigate().GoToUrl("https://secmanagement.azurewebsites.net/Invoices");
            wait.Until(d => d.PageSource.Contains("Contractor") || d.FindElements(By.TagName("select")).Count > 0);
            Assert.IsTrue(_driver.PageSource.Contains("Contractor"));

            // 4. Navigate to /Productivities/Generate Invoices
            _driver.Navigate().GoToUrl("https://secmanagement.azurewebsites.net/ProductivitySummaryViewModels");
            wait.Until(d => d.PageSource.Contains("Contractor") || d.FindElements(By.TagName("select")).Count > 0);
            Assert.IsTrue(_driver.PageSource.Contains("Contractor"));
        }

        [TestCleanup]
        public void EdgeDriverCleanup()
        {
            _driver.Quit();
        }
    }
}
