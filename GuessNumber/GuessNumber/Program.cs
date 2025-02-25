namespace GuessNumber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать в игру \"Угадай Число\"!");

            EntranceMenu();

            /*
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
            */
        }

        static void EntranceMenu()
        {
            Console.WriteLine("Добро пожаловать в игру Guess Number!");
            Console.WriteLine("Сперва вам нужно зарегистрироваться," +
                " если вы уже зарегистрированы, то необходимо авторизоваться.");

            Console.WriteLine("Введите номер необходимой операции.");
            Console.WriteLine("1) Войти \n" +
                              "2) Регистрация \n" +
                              "3) Выход");

            int userInput = Convert.ToInt32(Console.ReadLine());

            switch (userInput)
            {
                case 1:
                    //Authentication();
                    break;
                case 2:
                    Registration();
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
        }

        static void Registration()
        {
            string login;
            string password;
            string pathToUserData = "users.txt";

            if (!File.Exists(pathToUserData))
            {
                File.Create(pathToUserData).Dispose();
            }

            while (true)
            {
                Console.WriteLine("Введите ваш логин: ");

                login = Console.ReadLine();

                string[] users = File.ReadAllLines(pathToUserData);
                bool userExists = false;

                foreach (var user in users)
                {
                    string userLogin = user.Split(':')[0];

                    if (userLogin == login)
                    {
                        userExists = true;
                        break;
                    }
                }

                if (userExists)
                {
                    Console.WriteLine("Пользователь с таким именем уже существует. Выберите себе другое.");
                }
                else
                {
                    Console.WriteLine("Введите ваш пароль: ");
                    password = Console.ReadLine();

                    File.AppendAllText(pathToUserData, $"{login}:{password}" + Environment.NewLine);
                    Console.WriteLine("Поздравляем, вы успешно зарегистрированы." +
                        " Теперь вы можете войти под своим именем.");

                    EntranceMenu();
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
