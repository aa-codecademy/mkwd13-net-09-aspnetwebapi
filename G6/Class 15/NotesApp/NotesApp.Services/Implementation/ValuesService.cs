namespace NotesApp.Services.Implementation
{
	public class ValuesService
	{
		public int? SumPositiveNumbers(int num1, int num2)
		{
			if(num1<0 || num2 < 0)
			{
				return null;
			}

			return num1 + num2;
		}

		public bool IsFirstNumberLarger(int num1, int num2)
		{
			return num1 > num2;
		}
	}
}
