using System;

namespace aeroflot;
class Aeroflot
{

    public struct Flight
    {
    public int NumberFlight;
    public string? Destination;
    public TimeOnly? DepartureTime;
    public TimeOnly? ArriveTime;
    public int FreePlace;

    public string PrintToFile()
        {
            string Out = "Номер рейса: "+ this.NumberFlight + "\n" + 
                         "Пункт назначения: " + this.Destination + "\n" + 
                         "Время отправления: " + this.DepartureTime.ToString() + "\n" + 
                         "Время прибытия: " + this.ArriveTime.ToString() + "\n" + 
                         "Свободных мест: " + this.FreePlace.ToString() + "\n"; 
            return Out;
        }
    }

    private static Flight[]? Flights;

    const string FILEPATH = @"C:/Users/manko/source/repos/sharps_cur/Aeroflot.txt";
    const string FILEPATHDIFF = @"C:/Users/manko/source/repos/sharps_cur/Aeroflot_different.txt";
    readonly static string[] TIMEFORMATS = { "H:mm", "HH:mm" };

    public static Flight MakeFlight()
    {
        Flight new_flight = new Flight();
        Console.WriteLine("Введите номер рейса: ");
        new_flight.NumberFlight = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Пункт назначения: ");
        new_flight.Destination = Console.ReadLine();
        Console.WriteLine("Время отправления: ");
        TimeOnly time;
        while (!TimeOnly.TryParseExact(Console.ReadLine(), TIMEFORMATS, out time))
        {
            Console.WriteLine("Некорректный формат времени.");
        }
        new_flight.DepartureTime = time;

        Console.WriteLine("Введите время прибытия(в формате H:mm или HH:mm):");

        while (!TimeOnly.TryParseExact(Console.ReadLine(), TIMEFORMATS, out time))
        {
            Console.WriteLine("Некорректный формат времени.");
        }
        new_flight.ArriveTime = time;
        Console.WriteLine("Свободных мест: ");
        new_flight.FreePlace = Convert.ToInt32(Console.ReadLine());

        return new_flight;
    }

    public static void HelloUser()
    {
        /*Вывод встречающего предложения.*/

        Console.WriteLine("Создание и обработка сведений расписания аэропорта");
        Console.WriteLine("Здравствуйте. Управление осуществляется вводом соответсвующей цифры с клавиатуры и нажатием Enter.");
        Console.WriteLine("0 - Выход из программы.");
        Console.WriteLine("1 - Создать файл");
        Console.WriteLine("2 - Выдать на экран содержимое файла");
        Console.WriteLine("3 - Выдать на экран для заданного города список всех номеров рейсов со свободным количеством мест в виде");
        Console.WriteLine("4 - Сформировать файл");
        Console.WriteLine("5 - Распечатать сформированный файл");
        Console.WriteLine("6 - Добавить запись в сформированный файл");
        Console.WriteLine("7 - Удалить все записи с указанным номером рейса");
        Console.WriteLine("8 - Корректировка. Изменить количество свободных мест в салоне.");

    }

    public static void MakeFile(StreamWriter OutPut, Flight[] Flights)
    {
        OutPut.WriteLine("Количество записей в файле: "+ Flights.Length);

        for (int i = 0; i < Flights.Length; i++)
        {
            OutPut.WriteLine(Flights[i].PrintToFile());
        }

    }

    public static void ReadFile(StreamReader OutRead)
    {
        Console.Clear();

        string TextOfFile = OutRead.ReadToEnd();

        if (TextOfFile.Length == 0)
        {
            Console.WriteLine("Файл пуст!");
            return;
        }

        Console.WriteLine(TextOfFile);

        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();
    }

    public static void FindCity(Flight[] Flights)
    {

        bool key = false;
        
        Console.WriteLine("Введите нужный город");

        string? UserCity = Console.ReadLine();
        while (UserCity == null)
        {
            Console.WriteLine("Введите город!");
            UserCity = Console.ReadLine();
        }

        foreach (Flight flight in Flights)
        {
            if (flight.Destination == UserCity)
            {
                key = true;
                Console.WriteLine("Номер рейса: " + flight.NumberFlight);
                Console.WriteLine("Количество свободных мест: " + flight.FreePlace);
            }
        }

        if (!key)
        {
            Console.WriteLine("Указанного города нету.");
        }

        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();

    }

    public static Flight[]? ReadValuablesFromfile(StreamReader InRead)
    {
        string? line;
        line = InRead.ReadLine();

        if (line == null)
        {
            Console.WriteLine("\nФайл пуст, Создайте новый.");
            Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
            return null;
        }

        string[] words = line.Split(' ');

        int NumsOfLines = int.Parse(words[4]);
        if (NumsOfLines == 0 )
        {
            Console.WriteLine("В файле нету записей.");
            return null;
        }

        Flight[] Flights = new Flight[NumsOfLines];

        for (int i = 0; i < NumsOfLines; i++)
        {
            Flights[i] = new Flight();
            words = InRead.ReadLine().Split(" ");
            Flights[i].NumberFlight = int.Parse(words[2]);
            words = InRead.ReadLine().Split(" ");
            Flights[i].Destination = words[2];
            words = InRead.ReadLine().Split(" ");
            Flights[i].DepartureTime = TimeOnly.Parse(words[2]);
            words = InRead.ReadLine().Split(" ");
            Flights[i].ArriveTime = TimeOnly.Parse(words[2]);
            words = InRead.ReadLine().Split(" ");
            Flights[i].FreePlace = int.Parse(words[2]);
        }   

        return Flights;
        
        }

    public static void MakeDifferentFile(Flight[] Flights, StreamWriter OutPut)
    {
        for (int i = 0; i < Flights.Length; i++)
        {
            OutPut.WriteLine("Номер рейса: " + Flights[i].NumberFlight + "\n" +
                "Пункт назначения: " + Flights[i].Destination + "\n" +
                "Время вылета: " + Flights[i].DepartureTime);
        }
        Console.WriteLine("Файл создан.\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();
    }

    public static Flight[]? DeleteFlight(Flight[] Flights, int FlightToDel)
    {
        Flight[]? FlightsDels;
        bool key = false;
        int IndexOfDeleted = 0;

        foreach (var Flight in Flights)
        {
            if (Flight.NumberFlight == FlightToDel)
            {

                key = true;
                break;
            }
            IndexOfDeleted++;
        }
        if (key)
        {
            FlightsDels = new Flight[Flights.Length - 1];
            Array.Copy(Flights, 0, FlightsDels, 0, IndexOfDeleted);
            Array.Copy(Flights, IndexOfDeleted + 1, FlightsDels, IndexOfDeleted, Flights.Length - IndexOfDeleted - 1);
            return FlightsDels;
        }
        return null;
    }

    public static Flight[] EditFlight(Flight[] Flights, int FlightToEdit)
    {
        bool key = false;
        int IndexOfEdited = 0;

        foreach (var Flight in Flights)
        {
            if (Flight.NumberFlight == FlightToEdit)
            {
                key = true;
                break;
            }
            IndexOfEdited++;
        }
        if (key)
        {
            Console.WriteLine("Введите количетсво свободных мест: ");
            Flights[IndexOfEdited].FreePlace = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Количество мест успешно изменено.");
        }
        return Flights;
    }

    public static void Main(String[] args)
    {

        while (true)
        {
            HelloUser();
            int UserChoise;

            while (!int.TryParse(Console.ReadLine(), out UserChoise))
            {
                Console.WriteLine("Введите именно число!");
            }

            switch (UserChoise)
            {
                case 0:
                    {
                        Console.WriteLine("До свидания.");
                        System.Environment.Exit(1);
                        break;
                    }
                case 1:
                    {
                        Console.Clear();
                        Console.WriteLine("Данные будут в виде:\n" +
                            "№ рейса\n" +
                            "Пункт назначения\n" +
                            "Время вылета\n" +
                            "Время прибытия\n" +
                            "Количество свободных мест в салоне\n");

                        Console.WriteLine("Введите количество рейсов: ");
                        int NumsOfFlights = Convert.ToInt32(Console.ReadLine());
                        Flights = new Flight[NumsOfFlights];

                        for (int i = 0; i < NumsOfFlights; i++)
                        {
                            Flights[i] = MakeFlight();
                        }

                        StreamWriter OutPut = new StreamWriter(FILEPATH);
                        MakeFile(OutPut, Flights);
                        OutPut.Close();

                        Console.Clear();
                        Console.WriteLine("Файл создан успешно.");
                        break;
                    }
                case 2: 
                    {
                        StreamReader OutRead = new StreamReader(FILEPATH);
                        ReadFile(OutRead);
                        OutRead.Close();
                        break;
                    }
                case 3:
                    {
                        if (Flights == null)
                        {
                            StreamReader OutRead = new StreamReader(FILEPATH);
                            Flights = ReadValuablesFromfile(OutRead);
                            OutRead.Close();
                        }
                        FindCity(Flights);
                        break;
                    }
                case 4:
                    {
                        if (Flights == null)
                        {
                            StreamReader OutRead = new StreamReader(FILEPATH);
                            Flights = ReadValuablesFromfile(OutRead);
                            OutRead.Close();
                        }

                        StreamWriter OutPut = new StreamWriter(FILEPATHDIFF);
                        MakeDifferentFile(Flights, OutPut);
                        OutPut.Close();
                        break;
                    }
                case 5:
                    {
                        string[] Lines = File.ReadAllLines(FILEPATHDIFF);

                        foreach (var Line in Lines)
                        {
                            Console.WriteLine(Line);
                        }

                        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
                        Console.ReadKey();

                        break;
                    }
                case 6:
                    {
                        Console.Clear();
                        Console.WriteLine("Добавление новой записи в файл.");

                        Flight new_flight = MakeFlight();
                        StreamWriter OutPut = new StreamWriter(FILEPATH, true);
                        OutPut.WriteLine(new_flight.PrintToFile());
                        OutPut.Close();

                        Console.WriteLine("Запись создана.\nНажмите любую клавишу чтобы продолжить...");
                        Console.ReadKey();

                        break;
                    }
                case 7:
                    {
                        if (Flights == null)
                        {
                            StreamReader OutRead = new StreamReader(FILEPATH);
                            Flights = ReadValuablesFromfile(OutRead);
                            OutRead.Close();
                        }

                        Console.Clear();
                        Console.Write("Введите номер рейса, запись которого хотите удалить: ");
                        int FligthToDel = Convert.ToInt32(Console.ReadLine());
/* TODO:: ИСправить запись в начало файла количества запаисейй и в эдите тоже.*/
                        Flights = DeleteFlight(Flights, FligthToDel);

                        StreamWriter OutPut = new StreamWriter(FILEPATH);
                        for (int i = 0; i < Flights.Length; i++)
                        {
                            OutPut.WriteLine(Flights[i].PrintToFile());
                        }

                        Console.WriteLine("Запись удалена.\nНажмите любую клавишу чтобы продолжить...");
                        Console.ReadKey();

                        break;
                    }
                case 8:
                    {
                        Console.Clear();
                        Console.Write("Корректировка файла.\nУкажите номер рейса количество которого нужно изменить");

                        if (Flights == null)
                        {
                            StreamReader OutRead = new StreamReader(FILEPATH);
                            Flights = ReadValuablesFromfile(OutRead);
                            OutRead.Close();
                        }
                        
                        int FlightToEdit = Convert.ToInt32(Console.ReadLine());

                        Flights = EditFlight(Flights, FlightToEdit);

                        StreamWriter OutPut = new StreamWriter(FILEPATH);
                        for (int i = 0; i < Flights.Length; i++)
                        {
                            OutPut.WriteLine(Flights[i].PrintToFile());
                        }

                        Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
                        Console.ReadKey();

                        break;
                    }
                default:
                    {
                        Console.WriteLine("Введите число из списка выше!");
                        Console.Clear();
                        break;
                    }

            }
            Console.Clear();
        }
    }
}