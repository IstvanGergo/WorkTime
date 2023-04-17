using CommunityToolkit.Mvvm.Input;

namespace WorkTime.Services;

public class WorkData
{
    public static string DatabaseName = "WorkTimeDB.db";
    public static string DatabasePath
    {
        get
        {

            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path = Path.Combine(basePath + @"/" + DatabaseName);
            return $"Data Source={path}";
        }
    }
    static async Task<string> LoadMauiAsset()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("WorkTimeDB.db");
        using var reader = new StreamReader(stream);
        var contents = reader.ReadToEnd();
        return contents;
    }

    List<WorkTimeEntry> entries = new();
    public WorkData()
    {
        LoadMauiAsset();

    }
    public static void CreateTable()
    {
        using var conn = new SqliteConnection(DatabasePath);
        SqliteCommand command = conn.CreateCommand();
        conn.Open();
        command.CommandText = @"
        CREATE TABLE IF NOT EXISTS WorkTime (
        Date TEXT UNIQUE,
        Start TEXT,
        End TEXT,
        Time TEXT,
        Distance INTEGER
        );";
        command.ExecuteNonQuery();
    }
    public static void InsertData(DateTime date, TimeSpan start, TimeSpan end, long distance) // Check if the Data is already in the database. If it is, ask the user if he wants to update said data
    {
        using var conn = new SqliteConnection(DatabasePath);
        conn.Open();
        SqliteCommand sqlite_cmd = conn.CreateCommand();
        sqlite_cmd.CommandText = $"SELECT * FROM WorkTime WHERE Date = {date.Year}-{date.Month}-{date.Day}";
        TimeSpan length = end - start;
        string time = string.Format("{0:00}:{1:00}", length.Hours, length.Minutes);
        string Start = string.Format("{0:00}:{1:00}", start.Hours, start.Minutes);
        string End = string.Format("{0:00}:{1:00}", end.Hours, end.Minutes);
        if (length < TimeSpan.Zero)
        {
            Shell.Current.DisplayAlert("HIBA!", "A két idő különbsége nem lehet kisebb mint 0!", "OK");
        }
        if (sqlite_cmd.ExecuteScalar() == null)
        {
            sqlite_cmd.CommandText = $"INSERT INTO WorkTime (Date, Start, End, Time, Distance) VALUES ('{date.Year}-{date.Month}-{date.Day}','{Start}', '{End}', '{time}', {distance})";
            sqlite_cmd.ExecuteNonQuery();
        }
        else
        {
            sqlite_cmd.CommandText = $"UPDATE WorkTime Set Start = {start}, End = {end}, Time = {time}, Distance = {distance} WHERE Date = {date.Year}-{date.Month}-{date.Day}";
            sqlite_cmd.ExecuteNonQuery();
        }




    }
    public List<WorkTimeEntry> GetItems(DateTime start, DateTime end) // The select query will be updated. This method should be able to do all the queries.
    {
        using var conn = new SqliteConnection(DatabasePath);
        conn.Open();
        string s = start.ToString("s").Remove(10);
        string e = end.ToString("s").Remove(10);
        SqliteCommand sqlite_cmd = new($"SELECT * FROM WorkTime WHERE Date BETWEEN {s} AND {e}",conn);
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
        }
        return entries;
    } 
    public static void DeleteData(int id)
    {
        using var conn = new SqliteConnection(DatabasePath);
        conn.Open();
        SqliteCommand sqlite_cmd = new($"DELETE FROM WorkTime WHERE ROWID = {id}");
    }
    public static void DropTable()
    {
        using var conn = new SqliteConnection(DatabasePath);
        conn.Open();
        SqliteCommand sqlite_cmd = new("DROP WorkTime");
    }
}
