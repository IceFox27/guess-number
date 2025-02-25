namespace GuessNumber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в игру \"Угадай Число\"!");

            string randomNumber = GetRandomNumberString();

            Console.WriteLine("Я сгенерировал 4-х значное число, цифры в нём не повторяются. " +
                "Попробуй угадать его. " +
                "Вводи число только с неповтояющимися цифрами!");
        }

        static string GetRandomNumberString()
        {
            Random random = new Random();

            int firstDigit = random.Next(0, 10);
            List<int> digits = new List<int> { firstDigit };

            while (digits.Count != 4)
            {
                int digit = random.Next(0, 10);

                if (!digits.Contains(digit))
                {
                    digits.Add(digit);
                }
            }

            string numberString = string.Join("", digits);
            return numberString;
        }
    }
}
