using CsvHelper;
using System.Globalization;

namespace LINQ;

class Program
{
    static void Main(string[] args)
    {
        string csvPath = @"C:\Users\jank7\source\repos\LINQ\LINQ\googleplaystore1.csv";
        var googleApps = LoadGoogleAps(csvPath);

        Display(googleApps);
        //GetData(googleApps);
        //ProjectData(googleApps);
        //DivideData(googleApps);
        //OrderData(googleApps);
        //DataSetOperation(googleApps);
        //DataVerification(googleApps);
        //GroupData(googleApps);
        //GroupDataOperations(googleApps);
    }

    static void GetData(IEnumerable<GoogleApp> googleApps)
    {
        ///Pobieranie danych
        var appsWithRatingOver46 = googleApps.Where(x => x.Rating >= 4.6); //x => x.Rating >= 4.6 - Predykat
        var appsWithRatingOver46Beauty = appsWithRatingOver46.Where(x => x.Category == Category.BEAUTY);
        Display(appsWithRatingOver46Beauty);

        //Jesli warunek nie zostanie spelniony dostaniemy wyjatek System.InvalidOperationException
        var firstApp = appsWithRatingOver46Beauty.First(x => x.Reviews >= 30);
        Console.Write($"First: ");
        Display(firstApp);

        //Jesli warunek nie zostanie spelniony dostaniemy wartość domyślną danego typu(firstOrDefaultApp) w tym przypadku null
        //W konsoli nic nie zostanie wypisane
        var firstOrDefaultApp = appsWithRatingOver46Beauty.FirstOrDefault(x => x.Reviews <= 30);
        Console.Write($"FirstOrDefault: ");
        Display(firstOrDefaultApp);

        //Single = First z taką różnicą że sprawdza czy elementów spełniających daną predykatę jest więcej niz 1 element
        //Jesli jest więcej wyrzuci błąd System.InvalidOperationException
        //var singleApp = appsWithRatingOver46Beauty.Single(x => x.Reviews >= 30); //BLĄD
        //Console.Write($"Single: ");
        //Display(singleApp);

        //Jesli warunek nie zostanie spelniony dostaniemy wartość domyślną danego typu(singleOrDefaultApp) w tym przypadku null
        //W konsoli nic nie zostanie wypisane
        //Jesli jest więcej wyrzuci błąd System.InvalidOperationException
        var singleOrDefaultApp = appsWithRatingOver46Beauty.SingleOrDefault(x => x.Reviews <= 30);
        Console.Write($"SingleOrDefault: ");
        Display(singleOrDefaultApp);

        //Last = First (zamiast pierwszego elementu znajdzie ostatni)
        var lastApp = appsWithRatingOver46Beauty.Last(x => x.Reviews >= 30);
        Console.Write($"Last: ");
        Display(lastApp);

        //Jesli warunek nie zostanie spelniony dostaniemy wartość domyślną danego typu(lastOrDefaultApp) w tym przypadku null
        //W konsoli nic nie zostanie wypisane
        var lastOrDefaultApp = appsWithRatingOver46Beauty.Last(x => x.Reviews >= 30);
        Console.Write($"Last: ");
        Display(lastOrDefaultApp);
    }

    static void ProjectData(IEnumerable<GoogleApp> googleApps)
    {
        ///Projekcja danych 
        //LINQ Select pozwala nam również określić, jakie właściwości chcemy pobrać, czy chcesz pobrać wszystkie właściwości,
        //czy niektóre właściwości, które musisz określić w operatorze select.
        var highRatedBeautyApps = googleApps.Where(x => x.Rating >= 4.6 && x.Category == Category.BEAUTY);
        var highRatedBeautyAppsNames = highRatedBeautyApps.Select(x => x.Name); //x => x.Name - Selektor
                                                                                //Console.WriteLine(string.Join(", ", highRatedBeautyAppsNames));

        //Dla każdego elementu GoogleApp w naszej kolekcji chcemy zwrócić nowy objekt klasy GoogleAppDto
        var dtos = highRatedBeautyApps.Select(x => new GoogleAppDto()
        {
            Name = x.Name, //W którym sprecyzowaliśmy ze Name = x.Name
            Reviews = x.Reviews //...
        });

        foreach (var item in dtos)
        {
            Console.WriteLine($"{item.Name}: {item.Reviews}");
        }

        #region Co zastępuje SelectMany
        //genres = kolekcja kolekcji (aby wypisać elementy potrzebujemy pętli w pętli)
        var genres = highRatedBeautyApps.Select(x => x.Genres);

        foreach (var genres1 in genres)
        {
            foreach (var genre in genres1)
            {
                Console.Write(genre + " ");
            }
        }
        #endregion

        //SelectMany - wykona za nas całą logikę. Jeżeli mamy kolekcje w kolekcji pozwala nam spłaszczyć wszystko do 1 listy i potem ją przerabiać.
        var genresMany = highRatedBeautyApps.SelectMany(x => x.Genres);
        Console.WriteLine(string.Join(" ", genresMany));

        //Typ anonimowy
        //Dla każdego elementu GoogleApp w naszej kolekcji chcemy zwrócić nowy objekt bez żadnego typu
        var annonymousDtos = highRatedBeautyApps.Select(x => new
        {
            Name = x.Name, //W którym sprecyzowaliśmy ze Name = x.Name
            Reviews = x.Reviews //...
        });

        Console.WriteLine("annonymousDtos");
        foreach (var item in annonymousDtos)
        {
            Console.WriteLine($"{item.Name}: {item.Reviews}");
        }
    }

    static void DivideData(IEnumerable<GoogleApp> googleApps)
    {
        ///Dzielenie danych (Zwrócenie/Ominięcie konkretnej liczby wierszy)
        var highRatedBeautyApps = googleApps.Where(x => x.Rating >= 4.6 && x.Category == Category.BEAUTY);
        Console.WriteLine("All: ");
        Display(highRatedBeautyApps);

        //5 pierwszych elementow 
        var first5 = highRatedBeautyApps.Take(5);
        Console.WriteLine("Take: ");
        Display(first5);

        //5 ostatnich elementow 
        var last5 = highRatedBeautyApps.TakeLast(5);
        Console.WriteLine("TakeLast: ");
        Display(last5);

        //Zwraca elementy z sekwencji, o ile określony warunek jest spełniony.
        var takeWhile = highRatedBeautyApps.TakeWhile(x => x.Reviews > 1000);
        Console.WriteLine("TakeWhile: ");
        Display(takeWhile);

        //Pomija 5 pierwszych elementow.
        var skip5 = highRatedBeautyApps.Skip(5);
        Console.WriteLine("Skip: ");
        Display(skip5);

        //Pomija 5 ostatnich elementow.
        var skipLast5 = highRatedBeautyApps.SkipLast(5);
        Console.WriteLine("SkipLast: ");
        Display(skipLast5);

        //Pomija elementy w sekwencji, o ile określony warunek jest spełniony, a następnie zwraca pozostałe elementy.
        var skipWhile = highRatedBeautyApps.SkipWhile(x => x.Reviews > 1000);
        Console.WriteLine("SkipWhile: ");
        Display(skipWhile);
    }

    static void OrderData(IEnumerable<GoogleApp> googleApps)
    {
        ///Sortowanie danych(Zwrócenie nowej kolekcji na podstawie bazowej-
        ///-której elementy bedą w konkretnej kolejności która sami sprecyzujemy)          
        var highRatedBeautyApps = googleApps.Where(x => x.Rating >= 4.6 && x.Category == Category.BEAUTY);
        Console.WriteLine("All: ");
        Display(highRatedBeautyApps);

        var orderedByRating = highRatedBeautyApps.OrderBy(x => x.Rating); //Rosnąco: 4.6, 4.7...
        Console.WriteLine("Order By Rating: ");
        Display(orderedByRating);

        //Zmienna jest typem IOrderedEnumerable, aby można było wywołać funkcję ThenBy lub ThenByDescending-
        //-jesli Rating 2 aplikacji jest taki sam to posortujemy go po nazwie
        var orderedByRatingDescending = highRatedBeautyApps.OrderByDescending(x => x.Rating)//Malejąco: 4.7, 4.6...
            .ThenBy(x => x.Name);//Alfabetycznie po nazwie aplikacji.
        Console.WriteLine("Order By Rating Descending: ");
        Display(orderedByRatingDescending);
    }

    static void DataSetOperation(IEnumerable<GoogleApp> googleApps)
    {
        ///Operacje na zbiorach
        //Distinct - brak powtarzania się.0 
        var paidAppsCategories = googleApps.Where(x => x.Type == Type.Paid).Select(x => x.Category).Distinct();

        Console.WriteLine($"Paid apps categories: {string.Join(", ", paidAppsCategories)}");
        Console.WriteLine();

        var setA = googleApps.Where(a => a.Rating > 4.7 && a.Type == Type.Paid && a.Reviews > 1000);

        var setB = googleApps.Where(b => b.Name.Contains("Pro") && b.Rating > 4.6 && b.Reviews > 10000);

        //Display(setA);
        //Console.WriteLine();
        //Display(setB);

        //Połączenie 2 zbiorów (setA i setB muszą być tego samego typu, Aplikacje które występowały w obu zbiorach nie zostaną powielone).
        var appsUnion = setA.Union(setB);

        Console.WriteLine("Union: ");
        Display(appsUnion);

        //Zwrócenie wartości które były w 2 zbiorach.
        var appsIntersect = setA.Intersect(setB);

        Console.WriteLine("Intersect: ");
        Display(appsIntersect);

        //Elementy które są w zbiorze A ale nie występują w zbiorze B.
        var appsExpcept = setA.Except(setB);

        Console.WriteLine("Expcept: ");
        Display(appsExpcept);
    }

    static void DataVerification(IEnumerable<GoogleApp> googleApps)
    {
        ///Weryfikacja danych
        //Sprawdzenie czy dane aplikacje mają co najmniej 10 opinii (false - jesli chociaz 1 aplikacja nie spelnia warunku).
        var weatherApps = googleApps.Where(x => x.Category == Category.WEATHER).All(x => x.Reviews > 10);

        //true - jesli jest chociaz 1 element który spełnia dany warunek.
        var weatherApps2 = googleApps.Where(x => x.Category == Category.WEATHER).Any(x => x.Reviews > 2_000_000);
    }

    static void GroupData(IEnumerable<GoogleApp> googleApps)
    {
        ///Grupowanie danych
        //Pogrupowane dane staje się osobną kolekcją w kolekcji.
        var categoryGroup = googleApps.GroupBy(e => e.Category);//Grupa nie jest słownikiem
                                                                //var categoryGroup = googleApps.GroupBy(e => new{ e.Category, e.Type });//Grupowanie po Category i Type

        var artAndDesignGroup = categoryGroup.First(g => g.Key == Category.ART_AND_DESIGN);
        //var artAndDesignGroup = categoryGroup.First(g => g.Key.Category == Category.ART_AND_DESIGN);

        var apps = artAndDesignGroup.Select(e => e).Take(5); //Ienumerable aplikacji z tej grupy.
                                                             //var apps = artAndDesignGroup.ToList();
        Console.WriteLine($"Group: {artAndDesignGroup.Key}");
        Display(apps);

        //Wszystkie grupy
        foreach (var group in categoryGroup)
        {
            var _apps = group.ToList();
            Console.WriteLine($"Group: {group.Key}");
            Display(_apps);
        }
    }

    static void GroupDataOperations(IEnumerable<GoogleApp> googleApps)
    {
        ///Operacje na grupach
        var categoryGroup = googleApps
            .GroupBy(e => e.Category);
        //.Where(g => g.Min(a => a.Reviews) >= 10); //Grupy których każda aplikacja ma przynajmniej 10 opinii

        foreach (var group in categoryGroup)
        {
            var averageReviews = group.Average(g => g.Reviews); //Srednia liczba opinii
            var minReviews = group.Min(g => g.Reviews); //Najmniejsza liczba recenzji w danej kategorii
            var maxReviews = group.Max(g => g.Reviews); //Najwieksza...

            //Zliczenie ile opinii łącznie ma dana kategoria
            var sumReviews = group.Sum(g => g.Reviews);

            var allAppsFromGroupHaveRatingOf3 = group.All(a => a.Rating > 3.0); //Sprawdzenie każdego elementu danej grupy (czy Rating jest wiekszy niż 3.0)

            Console.WriteLine($"Category: {group.Key}");
            Console.WriteLine($"Average Reviews: {averageReviews}");
            Console.WriteLine($"Min Reviews: {minReviews}");
            Console.WriteLine($"Max Reviews: {maxReviews}");
            Console.WriteLine($"Sum Reviews: {sumReviews}");
            Console.WriteLine($"All Apps have 3.0 Rating: {allAppsFromGroupHaveRatingOf3}");
            Console.WriteLine();
        }
    }

    static void Display(IEnumerable<GoogleApp> googleApps)
    {
        foreach (var googleApp in googleApps)
        {
            Console.WriteLine(googleApp);
        }

    }
    static void Display(GoogleApp googleApp)
    {
        Console.WriteLine(googleApp);
    }

    static List<GoogleApp> LoadGoogleAps(string csvPath)
    {
        using (var reader = new StreamReader(csvPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csv.Context.RegisterClassMap<GoogleAppMap>();
            var records = csv.GetRecords<GoogleApp>().ToList();
            return records;
        }

    }

}