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
                         "Свободных мест: " + this.FreePlace.ToString(); 
            return Out;
        }
    }

    private static Flight[]? Flights;

    const string FILEPATH = @"C:/Users/manko/source/repos/sharps_cur/Aeroflot.txt";
    const string FILEPATHDIFF = @"C:/Users/manko/source/repos/sharps_cur/Aeroflot_different.txt";
    readonly static string[] TIMEFORMATS = { "H:mm", "HH:mm" };

    public static Flight MakeFlight()
    {
        /*Создание записи.*/
        Flight new_flight = new Flight();
        Console.WriteLine("Введите номер рейса: ");
        while (!int.TryParse(Console.ReadLine(), out new_flight.NumberFlight))
        {
            Console.WriteLine("Введите именно число!");
        }
        Console.WriteLine("Пункт назначения: ");
        new_flight.Destination = Console.ReadLine();
        Console.WriteLine("Время отправления(в формате H:mm или HH:mm): ");
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
        while (!int.TryParse(Console.ReadLine(), out new_flight.FreePlace))
        {
            Console.WriteLine("Введите именно число!");
        }

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
        /*Создание файла.*/
        OutPut.WriteLine("Количество записей в файле: " + Flights.Length);

        for (int i = 0; i < Flights.Length; i++)
        {
            OutPut.WriteLine(Flights[i].PrintToFile());
        }

    }

    public static void ReadFile(StreamReader OutRead)
    {
        /*Вывод файла в консоль.*/
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
        /*Вывод задания про город*/

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

    public static Flight[] ReadValuablesFromfile(StreamReader InRead)
    {
        /*Читаем файл*/
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
        /*Создаем файл по заданию.*/
        for (int i = 0; i < Flights.Length; i++)
        {
            OutPut.WriteLine("Номер рейса: " + Flights[i].NumberFlight + "\n" +
                "Пункт назначения: " + Flights[i].Destination + "\n" +
                "Время вылета: " + Flights[i].DepartureTime);
        }
        Console.WriteLine("Файл создан.\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();
    }

    public static Flight[] DeleteFlight(Flight[] Flights, int FlightToDel)
    {
        /*Удаление записи*/
        Flight[] FlightsDels = Flights;
        bool key = false;
        bool deleted = false;
        int IndexOfDeleted = 0;

        foreach (var Flight in Flights)
        {
            if (Flight.NumberFlight == FlightToDel)
            {            
                key = true;
                deleted = true;
            }
            if (key)
            {
                FlightsDels = new Flight[Flights.Length - 1];
                Array.Copy(Flights, 0, FlightsDels, 0, IndexOfDeleted);
                Array.Copy(Flights, IndexOfDeleted + 1, FlightsDels, IndexOfDeleted, Flights.Length - IndexOfDeleted - 1);
                key = false;
            }
            IndexOfDeleted++;
        }
        if (!deleted)
        {
            Console.WriteLine("Указанного номера не существует.\nНажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();

            return FlightsDels;
        }
        Console.WriteLine("Запись удалена.\nНажмите любую клавишу чтобы продолжить...");
        Console.ReadKey();
        return FlightsDels;
    }

    public static Flight[] EditFlight(Flight[] Flights, int FlightToEdit)
    {
        /*Изменение записи*/
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
            Console.WriteLine("Запись изменена.\nНажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
        }
        else
        {
            Console.WriteLine("Заданного рейса не существует.");
        }
        return Flights;
    }

    public static Flight[] AddToFile(Flight[] Flights)
    {
        /*Добавляет в конец файла ещё одну запись.*/
        Flight[] NewArrayOfFlights = new Flight[Flights.Length + 1];
        Flight NewFlight = MakeFlight();
        
        Array.Copy(Flights, NewArrayOfFlights, Flights.Length);

        NewArrayOfFlights[Flights.Length] = NewFlight;

        return NewArrayOfFlights ;
    }

    public static StreamReader? FileCheckReader (string FilePath)
    {
        /*Проверяет файл на работоспособность. + Создание объекта чтения файла*/
        try
        {
            StreamReader OutRead = new StreamReader(FilePath);
            return OutRead;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка файла: " + ex.Message);
            Console.WriteLine("Попробуйте пересоздать файл.");

            Console.WriteLine("\nНажмите любую клавишу чтобы продолжить...");
            Console.ReadKey();
            return null;
        }
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
                        Console.WriteLine("Введите количество рейсов: ");
                        int NumsOfFlights;
                        while (!int.TryParse(Console.ReadLine(), out NumsOfFlights))
                        {
                            Console.WriteLine("Введите именно число!");
                        }

                        Flights = new Flight[NumsOfFlights];

                        for (int i = 0; i < NumsOfFlights; i++)
                        {
                            Console.Clear();
                            Console.WriteLine("Данные будут в виде:\n" +
                                "№ рейса\n" +
                                "Пункт назначения\n" +
                                "Время вылета\n" +
                                "Время прибытия\n" +
                                "Количество свободных мест в салоне\n");
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
                        StreamReader? OutRead = FileCheckReader(FILEPATH);

                        if (OutRead == null)
                        {
                            break;
                        }

                        ReadFile(OutRead);
                        OutRead.Close();

                        break;
                    }
                case 3:
                    {
                        StreamReader? OutRead = FileCheckReader(FILEPATH);

                        if (OutRead == null)
                        {
                            break;
                        }
                        Flights = ReadValuablesFromfile(OutRead);
                        OutRead.Close();

                        FindCity(Flights);
                        break;
                    }
                case 4:
                    {
                        StreamReader? OutRead = FileCheckReader(FILEPATH);

                        if (OutRead == null)
                        {
                            break;
                        }

                        Flights = ReadValuablesFromfile(OutRead);

                        OutRead.Close();

                        StreamWriter OutPut = new StreamWriter(FILEPATHDIFF);
                        MakeDifferentFile(Flights, OutPut);
                        OutPut.Close();
                        break;
                    }
                case 5:
                    {

                        StreamReader? OutRead = FileCheckReader(FILEPATHDIFF);

                        if (OutRead == null)
                        {
                            break;
                        }

                        ReadFile(OutRead);
                        OutRead.Close();

                        break;
                    }
                case 6:
                    {
                        Console.WriteLine("Добавление новой записи в файл.");

                        StreamReader? OutRead = FileCheckReader(FILEPATH);

                        if (OutRead == null)
                        {
                            break;
                        }

                        Flights = ReadValuablesFromfile(OutRead);
                        OutRead.Close();

                        Flights = AddToFile(Flights);
                        StreamWriter OutPut = new StreamWriter(FILEPATH);                        
                        MakeFile(OutPut, Flights);
                        OutPut.Close();

                        break;
                    }
                case 7:
                    {
                        StreamReader? OutRead = FileCheckReader(FILEPATH);

                        if (OutRead == null)
                        {
                            break;
                        }
                        Flights = ReadValuablesFromfile(OutRead);

                        OutRead.Close();

                        Console.Clear();
                        Console.Write("Введите номер рейса, запись которого хотите удалить: ");
                        int FligthToDel = Convert.ToInt32(Console.ReadLine());

                        Flights = DeleteFlight(Flights, FligthToDel);
 
                        StreamWriter OutPut = new StreamWriter(FILEPATH);
                        MakeFile(OutPut, Flights);
                        OutPut.Close();

                        break;
                    }
                case 8:
                    {

                        StreamReader? OutRead = FileCheckReader(FILEPATH);

                        if (OutRead == null)
                        {
                            break;
                        }
                        Flights = ReadValuablesFromfile(OutRead);
                        OutRead.Close();

                        Console.Clear();
                        Console.Write("Корректировка файла.\nУкажите номер рейса количество которого нужно изменить: ");
                        
                        int FlightToEdit = Convert.ToInt32(Console.ReadLine());

                        Flights = EditFlight(Flights, FlightToEdit);

                        StreamWriter OutPut = new StreamWriter(FILEPATH);
                        MakeFile(OutPut, Flights);
                        OutPut.Close();

                        break;
                    }
                default:
                    {
                        Console.WriteLine("Введите число из списка выше!\nНажмите любую клавишу чтобы продолжить...");
                        Console.ReadKey();


                        Console.Clear();
                        break;
                    }
            }
            Console.Clear();
        }
    }
}