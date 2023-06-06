using air_project;
using air_project.pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        ChangeFlight flight = new ChangeFlight();

        [TestMethod]
        public void TestAdd()
        {
            flight.AddMyTicket(4, 6);
        }

        [TestMethod]
        public void TestDelete()
        {
            flight.DeleteFlight(1);
        }

        [TestMethod]
        public void TestUpdate()
        {
            DateTime depdate = new DateTime(2023, 6, 11, 13, 30, 0);
            DateTime arrdate = new DateTime(2023, 6, 11, 16, 30, 0);
            flight.UpdateFlight(4, 1, depdate, 2, arrdate, 36, 21300);
        }
    }
}
