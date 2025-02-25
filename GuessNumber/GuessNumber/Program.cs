namespace GuessNumber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EntranceMenu();
        }

        static void EntranceMenu()
        {
            Console.WriteLine("Добро пожаловать в игру Guess Number!");
            Console.WriteLine("Сперва вам нужно зарегистрироваться");
            Console.WriteLine("Если вы уже зарегистрированы, то войдите.");

            Console.WriteLine("Введите номер необходимой операции.");
            Console.WriteLine("1) Войти \n" +
                              "2) Регистрация \n" +
                              "3) Выход");

            int userInput = Convert.ToInt32(Console.ReadLine());

            switch (userInput)
            {
                case 1:
                    Authentication();
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

        static void Authentication()
        {
            string login;
            string password;
            string pathToUserData = "users.txt";

            if (!File.Exists(pathToUserData))
            {
                Console.WriteLine("В нашей базе данных нет ещё пользователей." +
                    " Пройдите регистрацию и станьте первым!");

                Registration();
            }

            while (true)
            {
                Console.WriteLine("Введите ваш логин: ");

                login = Console.ReadLine();

                string[] users = File.ReadAllLines(pathToUserData);
                bool userExists = false;
                string userPassword = "";

                foreach (string user in users)
                {
                    string userLogin = user.Split(':')[0];

                    if (userLogin == login)
                    {
                        userPassword = user.Split(':')[1];
                        userExists = true;
                        break;
                    }
                }

                if (!userExists)
                {
                    Console.WriteLine("Пользователь не найден. Опечатка? " +
                        "Или может быть вам необходимо зарегистрироваться?");

                    EntranceMenu();
                }

                while (true)
                {
                    Console.Write("Ввведите ваш пароль: ");

                    password = Console.ReadLine();

                    if (password == userPassword)
                    {
                        Console.WriteLine("Авторизация успешна, вход в программу");
                        GameMenu(login, password);
                    }
                    else
                    {
                        Console.WriteLine("Неправильный пароль, попробуйте еще раз.");
                    }
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

        static void GameMenu(string login, string password)
        {
            Console.WriteLine($"Добро пожаловать, {login} \n" +
                    "1) Начать игру \n" +
                    "2) Ваши игры \n" +
                    "3) Таблица лидеров \n" +
                    "4) Выход"
                    );

            int userInput = Convert.ToInt32(Console.ReadLine());

            switch (userInput)
            {
                case 1:
                    GameLogic(login, password);
                    break;
                case 2:
                    PrintPlayerResults(login, password);
                    break;
                case 3:
                    PrintLeaderBord(login, password);
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
            }
        }

        static void GameLogic(string login, string password)
        {
            string randomNumber = GetRandomNumberString();

            Console.WriteLine("Я сгенерировал 4-х значное число, цифры в нём не повторяются. " +
                "Попробуй угадать его. " +
                "Вводи число только с неповтояющимися цифрами!");

            int attempts = 0;

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
                    Console.WriteLine("Поздравляем, вы угадали число!");
                    attempts++;

                    SaveResult(login, attempts);
                    GameMenu(login, password);
                }

                attempts++;
            }
        }

        static void SaveResult(string login, int attempts)
        {
            string resultPath = "results.txt";
            string result = $"{login}:{attempts}";

            if (!File.Exists(resultPath))
            {
                File.Create(resultPath);
            }

            string[] existingResults = File.ReadAllLines(resultPath);
            List<string> updatedResults = new List<string>();

            bool existingResultsFound = false;

            foreach (string existingResult in existingResults)
            {
                if (existingResult.StartsWith(login))
                {
                    string[] existingAttempts = existingResult.Split(':');
                    string newAttempts = $"{existingAttempts[1].Trim()}|{attempts}";

                    updatedResults.Add($"{login}:{newAttempts}");
                    existingResultsFound = true;
                }
                else
                {
                    updatedResults.Add(existingResult);
                }
            }

            if (!existingResultsFound)
            {
                updatedResults.Add(result);
            }

            File.WriteAllText(resultPath, string.Join(Environment.NewLine, updatedResults) + Environment.NewLine);

            Console.WriteLine($"Вы выиграли. Количество попыток {attempts}");
        }

        static void PrintPlayerResults(string login, string password)
        {
            string resultPath = "results.txt";

            if (!File.Exists(resultPath))
            {
                Console.WriteLine("Файл результатов не существует.");
                return;
            }

            string[] results = File.ReadAllLines(resultPath);

            foreach (string result in results)
            {
                string[] parts = result.Split(':');
                string player = parts[0];

                if (player == login)
                {
                    string[] attempts = parts[1].Split('|');
                    Console.WriteLine("Результаты игрока:");

                    for (int i = 0; i < attempts.Length; i++)
                    {
                        Console.WriteLine($"Игра {i + 1}: {attempts[i]} попыток");
                    }

                    GameMenu(login, password);
                }
            }

            Console.WriteLine("Игрок не найден.");
        }

        static List<(string, int)> MakeLeaderBord()
        {
            string resultPath = "results.txt";

            if (!File.Exists(resultPath))
            {
                Console.WriteLine("Файл результатов не существует.");
                return new List<(string, int)>();
            }

            string[] results = File.ReadAllLines(resultPath);
            Dictionary<string, int> playerResults = new Dictionary<string, int>();

            foreach (string result in results)
            {
                string[] parts = result.Split(':');
                string player = parts[0];
                string[] attempts = parts[1].Split('|');

                int bestResult = 1000000;

                foreach (string attempt in attempts)
                {
                    int attemptValue = int.Parse(attempt.Trim());
                    if (attemptValue < bestResult)
                    {
                        bestResult = attemptValue;
                    }
                }

                playerResults[player] = bestResult;
            }

            List<(string, int)> sortedPlayerResults = playerResults.OrderBy(x => x.Value).Select(x => (x.Key, x.Value)).ToList();
            return sortedPlayerResults;
        }

        static void PrintLeaderBord(string login, string password)
        {
            List<(string, int)> sortedPlayerResults = MakeLeaderBord();

            if (sortedPlayerResults.Count == 0)
            {
                Console.WriteLine("Лидерборд пуст");
            }
            else
            {
                Console.WriteLine("Лидерборд:");
                int place = 1;
                foreach ((string, int) playerResult in sortedPlayerResults)
                {
                    Console.WriteLine($"{place}.{playerResult.Item1} победил за {playerResult.Item2} ходов!");
                    place++;
                }
            }

            GameMenu(login, password);
        }
    }
}
