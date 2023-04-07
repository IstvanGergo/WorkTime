namespace WorkTime.Services;

public class WorkData
{
    public const string DatabaseName = "WorkTime.db3";
    public static string DatabasePath
    {
        get
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path =Path.Combine(basePath +"/"+ DatabaseName);
            return $"Data Source={path}";
        }
    }
    List<WorkTimeEntry> entries= new();
    public WorkData()
    {

    }
    public static void CreateTable()
    {
        using var conn = new SqliteConnection(DatabasePath);
        SqliteCommand command = conn.CreateCommand();
        command.CommandText = @"
        CREATE TABLE IF NOT EXISTS WorkTime (
        Id INTEGER PRIMARY KEY,
        Date TEXT,
        Start TEXT,
        End TEXT,
        Time TEXT,
        Distance INTEGER
        );";
        command.ExecuteNonQuery();

    }
    public static void InsertData(string date, string start, string end, long distance)
    {
        using var conn = new SqliteConnection(DatabasePath);
        SqliteCommand sqlite_cmd = conn.CreateCommand();
        DateTime dstart = DateTime.ParseExact(start,"HH:mm",CultureInfo.InvariantCulture,
                                              DateTimeStyles.None);
        DateTime dend= DateTime.ParseExact(end, "HH:mm", CultureInfo.InvariantCulture,
                                              DateTimeStyles.None);
        TimeSpan length = dend - dstart;

        string time = string.Format("{0:00}:{1:00}",length.Hours,length.Minutes);
        sqlite_cmd.CommandText = $"INSERT INTO WorkTime (Date, Start, End, Time, Distance) VALUES ('{date}','{start}', '{end}', '{time}', {distance})";
        sqlite_cmd.ExecuteNonQuery();
        conn?.Close();
    }
    public List<WorkTimeEntry> GetItems()
    {
        using var conn = new SqliteConnection(DatabasePath);
        SqliteCommand sqlite_cmd = new("SELECT * FROM WorkTime", conn);
        SqliteDataReader reader = null;
        try
        {
            reader =  sqlite_cmd.ExecuteReader();
            while (reader.Read())
            {
                WorkTimeEntry data = new()
                {
                    Date = (string)reader[1],
                    Start = (string)reader[2],
                    End = (string)reader[3],
                    Time = (string)reader[4],
                    Distance = (long)reader[5]
                };
                entries.Add(data);
            }
        }
        finally
        {
            reader?.Close();
            conn?.Close();
        }
        return entries;
    }
    public static void DeleteData( int id)
    {
        using var conn = new SqliteConnection(DatabasePath);
        SqliteCommand sqlite_cmd = new($"DELETE FROM WorkTime WHERE id = {id}");
        conn?.Close();
    }
    public static void DropTable()
    {
        using var conn = new SqliteConnection(DatabasePath);
        SqliteCommand sqlite_cmd = new("DROP WorkTime");
        conn?.Close();
    }
}
