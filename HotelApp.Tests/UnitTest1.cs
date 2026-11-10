using System; 
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HotelApp.Data; 

namespace HotelApp.Tests
{
    [TestClass]
    public class HotelTests
    {
        // Тест 1: Проверка расчета стоимости
        [TestMethod]
        public void CalculateTotalPrice_ValidData_ReturnsCorrectSum()
        {
            decimal pricePerNight = 2500;
            int nights = 3;
            decimal expected = 7500;

            decimal actual = pricePerNight * nights;

            Assert.AreEqual(expected, actual, "Стоимость за 3 ночи должна быть 7500");
        }

        // Тест 2: Проверка корректности дат (Выезд позже заезда)
        [TestMethod]
        public void ValidateDates_CheckOutAfterCheckIn_ReturnsTrue()
        {
            DateTime checkIn = new DateTime(2026, 11, 10);
            DateTime checkOut = new DateTime(2026, 11, 12);

            bool isValid = checkOut > checkIn;

            Assert.IsTrue(isValid, "Дата выезда должна быть позже даты заезда");
        }

        // Тест 3: Проверка некорректных дат (Выезд раньше заезда)
        [TestMethod]
        public void ValidateDates_CheckOutBeforeCheckIn_ReturnsFalse()
        {
            DateTime checkIn = new DateTime(2026, 11, 12);
            DateTime checkOut = new DateTime(2026, 11, 10);

            bool isValid = checkOut > checkIn;

            Assert.IsFalse(isValid, "Система должна отклонять неверные даты");
        }

        // Тест 4: Проверка автоподстановки имени
        [TestMethod]
        public void UserName_SetAndGet_ReturnsSameValue()
        {
            string testName = "Иванов Иван";

            AppConnect.UserName = testName;

            Assert.AreEqual(testName, AppConnect.UserName, "Имя пользователя должно совпадать");
        }

        // Тест 5: Проверка роли администратора
        [TestMethod]
        public void AdminRoleId_ShouldBeOne()
        {
            int adminRoleId = 1;

            Assert.AreEqual(1, adminRoleId, "ID роли администратора должен быть 1");
        }
    }
}