using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RandomService
    {
        public int? Sum(int num1, int num2)
        {
            int sum = num1 + num2;
            if (num1 > 0 && num2 > 0 && sum < 0) return null;
            return sum;
        }

        public bool isFirstNumberLarger(int num1, int num2) 
        {
            return num1 > num2;
        }

        public string GetDigitName(int num)
        {
            List<string> digitNames = new List<string>()
            {
                "Zero", "One", "Two", "Three", "Fourt"
            };
            return digitNames[num];
        }
    }
}
