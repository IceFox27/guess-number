namespace GuessNumber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в игру \"Угадай Число\"!");

            GameLogic();

            while (true)
            {
                Console.WriteLine("Продолжим?");
                Console.WriteLine("1) Да");
                Console.WriteLine("2) Нет");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        GameLogic();
                        break;
                    case "2":
                        Console.WriteLine("Спасибо за игру! До свидания!");
                        return;
                    default:
                        Console.WriteLine("Неправильный ввод. Пожалуйста, выберите 1 или 2.");
                        break;
                }
            }
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

        static void GameLogic()
        {
            string randomNumber = GetRandomNumberString();

            Console.WriteLine("Я сгенерировал 4-х значное число, цифры в нём не повторяются. " +
                "Попробуй угадать его. " +
                "Вводи число только с неповтояющимися цифрами!");

            while (true)
            {
                Console.WriteLine("Введи своё предположение: ");

                string guess = Console.ReadLine();

                if (guess.Length != 4)
                {
                    Console.WriteLine("Неправильный формат. Введите 4 цифры.");
                    continue;
                }

                int rightPos = 0;
                int incorrectPos = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (guess[i] == randomNumber.ToString()[i])
                    {
                        rightPos++;
                    }
                    else if (randomNumber.ToString().Contains(guess[i]))
                    {
                        incorrectPos++;
                    }
                }

                Console.WriteLine($"Угадано на правильных позициях: {rightPos}, " +
                            $"угадано на неправильных позициях: {incorrectPos}");

                if (rightPos == 4)
                {
                    Console.WriteLine("Поздравляю, вы угадали число!");
                    break;
                }
            }
        }
    }
}
