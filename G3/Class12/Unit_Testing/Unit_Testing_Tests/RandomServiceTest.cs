using Service;

namespace Unit_Testing_Tests
{
    [TestClass]
    public class RandomServiceTest
    {
        private readonly RandomService _randomService;

        public RandomServiceTest(RandomService randomService)
        {
            _randomService = randomService;
        }

        [TestMethod]
        public void Sum_BothNumbersAreValid_PositiveResultNumber()
        {
            //Arrange
            int num1 = 10;
            int num2 = 20;
            int? expectedResult = 30;

            //Act
            int? result = _randomService.Sum(num1, num2);

            //Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Sum_largeNumberIntefers_Null()
        {
            //Arrange
            int num1 = 2111111111;
            int num2 = 2111111111;

            //Act
            int? result = _randomService.Sum(num1, num2);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void isFirstNumberLarger_FirstNumberISLarger_True()
        {
            //Arrange
            int num1 = 20;
            int num2 = 12;

            //Act
            bool result = _randomService.isFirstNumberLarger(num1, num2);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetDigitName_FiveDigit_Exception()
        {
            //Arrange
            int num = 5;

            //Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(()=>_randomService.GetDigitName(num));
        }

    }
}
