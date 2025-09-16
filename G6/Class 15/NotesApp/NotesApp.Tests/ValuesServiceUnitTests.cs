using NotesApp.Services.Implementation;

namespace NotesApp.Tests
{
	[TestClass]
	public class ValuesServiceUnitTests
	{
		private readonly ValuesService _service;

		public ValuesServiceUnitTests()
		{
			_service = new ValuesService();
		}

		[TestMethod]
		public void SumPositiveNumbers_should_return_null_on_negative_values()
		{
			//Arange
			int num1 = -2; //we are testing the case when we have negative input
			int num2 = 3;

			//Act
			int? result = _service.SumPositiveNumbers(num1, num2); //here we call the method that we are testing

			//Assert
			Assert.IsNull(result); //we expect our result to be null

		}

		[TestMethod]
		public void SumPositiveNumber_should_return_positiveNumber_on_positive_inputs()
		{
			//Arange
			int num1 = 2;
			int num2 = 3;
			int expectedResult = 5;


			//Act
			int? result = _service.SumPositiveNumbers(num1, num2);

			//Assert
			Assert.AreEqual(expectedResult, result); //we check if the result that we expected is the result that we got
		}

		[TestMethod]
		public void IsFirstNumberLarger_should_return_true()
		{
			//Arange
			int num1 = 3;
			int num2 = 2;

			//Act
			bool result = _service.IsFirstNumberLarger(num1, num2);

			//Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsFirstNumberLarger_should_return_false()
		{
			//Arange
			int num1 = 3;
			int num2 = 2;

			//Act
			bool result = _service.IsFirstNumberLarger(num2, num1);

			//Assert
			Assert.IsFalse(result);
		}
	}
}