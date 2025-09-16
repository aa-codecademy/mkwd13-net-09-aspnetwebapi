using System.Data;

namespace NotesApp.Helpers
{
    public static class ValidationHelper
    {
        public static void ValidateRequiredStringColumnLength(string value, string field, int maxNumOfChars)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DataException($"{field} is required field!");
            }

            if (value.Length > maxNumOfChars)
            {
                throw new DataException($"{field} can not contain more than {maxNumOfChars} chars");
            }
        }

        public static void ValidateColumnLength(string value, string field, int maxNumOfChars)
        {
            var length = value == null ? 0 : value.Length;

            if (length > maxNumOfChars)
            {
                throw new DataException($"{field} can not contain more than {maxNumOfChars} chars");
            }
        }
    }
}
 