using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Remote;

namespace AppiumTask
{
    [TestFixture]
    public class UnitTest1
    {
        AppiumDriver<AndroidElement> driver;
        DesiredCapabilities capabilities;

        string check;
        string textRadioButton;
        string textAttr;
        string[,] testData;

        [OneTimeSetUp]
        public void BeforeAllMethods()
        {
            capabilities = new DesiredCapabilities();
            capabilities.SetCapability(MobileCapabilityType.App, @"D:\Demo\ApiDemos-debug.apk");
            capabilities.SetCapability(MobileCapabilityType.DeviceName, "emulator-5554");
            capabilities.SetCapability(MobileCapabilityType.Udid, "emulator-5554");
            capabilities.SetCapability(MobileCapabilityType.PlatformVersion, "6.0.0");
            capabilities.SetCapability(MobileCapabilityType.PlatformName, "Android");
            capabilities.SetCapability(MobileCapabilityType.FullReset, "false");


            check = "true";
            textRadioButton = "Lunch";
            textAttr = "You have selected: 2131296531";

            testData = new[,] { 
                { "true", "Snack", "You have selected: 2131296259" },
                { "true", "Breakfast", "You have selected: 2131296533" },
                { "true", "Lunch", "You have selected: 2131296531" },
                { "true", "Dinner", "You have selected: 2131296534"},
                { "true", "All of them", "You have selected: 2131296535"}
            };
        
        }

        [SetUp]
        public void SetUp()
        {
            driver = new AndroidDriver<AndroidElement>(new Uri("http://127.0.0.1:4723/wd/hub"), capabilities);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(50));

            driver.Swipe(0, 0, 0, 5, 0);
            driver.FindElement(By.Id("Views")).Click();
            Thread.Sleep(1000);
            //
            driver.Swipe(0, 0, 0, 2, 0);
            driver.Swipe(0, 0, 0, 2, 0);
            driver.FindElement(By.Id("Radio Group")).Click();
        }

        
        [Test]
        public void TestOpenRadioGroup()
        {
            Thread.Sleep(2000);
            Assert.AreEqual(check, driver.FindElement(By.Id("Lunch")).GetAttribute("checked"));
            Assert.AreEqual(textRadioButton, driver.FindElement(By.Id("Lunch")).GetAttribute("text"));
            Assert.AreEqual(textAttr, driver.FindElement(By.Id("You have selected: (none)")).GetAttribute("text"));
            //
            Thread.Sleep(2000);
            driver.CloseApp();
        }

        private static readonly object[] RadioButtons =
        {
            new object[] { "true", "Snack", "You have selected: 2131296259" },
            new object[] { "true", "Breakfast", "You have selected: 2131296533" },
            new object[] { "true", "Lunch", "You have selected: 2131296531" },
            new object[] { "true", "Dinner", "You have selected: 2131296534" },
            new object[] { "true", "All of them", "You have selected: 2131296535" }
        };

        [Test]
        [TestCaseSource(nameof(RadioButtons))]
        public void TestSwitched(string checks, string textButton, string textView)
        {
            
            IWebElement webElement = driver.FindElement(By.XPath("//android.widget.RadioButton[@text='" +textButton+ "']"));
             webElement.Click();
             Thread.Sleep(2000);

             Assert.AreEqual(check, webElement.GetAttribute("checked"));
             Assert.AreEqual(textButton, webElement.GetAttribute("text"));
             Assert.AreEqual(textView, driver.FindElement(By.Id("You have selected: (none)")).GetAttribute("text"));
               

        }

        [Test]
        public void TestSwitchedAll()
        {
            for(int i = 0; i < 5; i++)
            {
                IWebElement webElement = driver.FindElement(By.XPath("//android.widget.RadioButton[@text='" + testData[i,1] + "']"));
                webElement.Click();
                Thread.Sleep(2000);

                Assert.AreEqual(check, webElement.GetAttribute("checked"));
                Assert.AreEqual(testData[i,1], webElement.GetAttribute("text"));
                Assert.AreEqual(testData[i,2], driver.FindElement(By.Id("You have selected: (none)")).GetAttribute("text"));  
            }
        }

        [Test]
        public void TestButtonClear()
        {
            driver.FindElement(By.Id("Clear")).Click();
            Assert.AreEqual("false", driver.FindElement(By.Id("io.appium.android.apis:id/snack")).GetAttribute("checked"));
            Assert.AreEqual("false", driver.FindElement(By.Id("Breakfast")).GetAttribute("checked"));
            Assert.AreEqual("false", driver.FindElement(By.Id("Lunch")).GetAttribute("checked"));
            Assert.AreEqual("false", driver.FindElement(By.Id("Dinner")).GetAttribute("checked"));
            Assert.AreEqual("false", driver.FindElement(By.Id("All of them")).GetAttribute("checked"));
            Assert.AreEqual("You have selected: (none)", driver.FindElement(By.Id("You have selected: (none)")).GetAttribute("text"));//expected, actual
        }

    }
}


