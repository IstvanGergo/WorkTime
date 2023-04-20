namespace WorkTime.Services;

public class WorkData
{
    /*THESE TWO STRINGS ARE NEEDED FOR THE DB TO BE LOADED INTO APP DATA DIRECTORY*/
    public static string DatabaseName = "WorkTimeDB.db";
    public static string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseName);

    /*THIS STRING IS FOR THE SQLITE CONNECTION*/
    public static string DBpath = $"Data Source={DatabasePath}";
    static void LoadDatabase()
    {
        Task<Stream> task = FileSystem.OpenAppPackageFileAsync($"{DatabaseName}");
        var stream = task.Result;
        using MemoryStream memoryStream = new();
        stream.CopyTo(memoryStream);
        File.WriteAllBytes(DatabasePath, memoryStream.ToArray());
    }

    List<WorkTimeEntry> entries = new();
    public WorkData()
    {
        
    }
    public static void CreateTable()
    {
        using var conn = new SqliteConnection(DBpath);
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
    public static void InsertData(DateTime date, TimeSpan start, TimeSpan end, long distance) // Datatype Mismatch, i don't know why yet.
    {
        using var conn = new SqliteConnection(DBpath);
        conn.Open();
        SqliteCommand sqlite_cmd = conn.CreateCommand();
        string date_to_insert = date.ToString("s").Remove(10); //This way the format will be YYYY:MM:DD
        sqlite_cmd.CommandText = $"SELECT * FROM WorkTime WHERE Date = {date_to_insert}";
        TimeSpan length = end - start;
        string time = string.Format("{0:00}:{1:00}", length.Hours, length.Minutes); //this way the format will be HH:MM
        string Start = string.Format("{0:00}:{1:00}", start.Hours, start.Minutes); //this way the format will be HH:MM
        string End = string.Format("{0:00}:{1:00}", end.Hours, end.Minutes); //this way the format will be HH:MM
        if (length < TimeSpan.Zero)
        {
            Shell.Current.DisplayAlert("HIBA!", "A két idő különbsége nem lehet kisebb mint 0!", "OK");
        }
        if (sqlite_cmd.ExecuteScalar() == null)
        {

            sqlite_cmd.CommandText = $"INSERT INTO WorkTime VALUES ('{date_to_insert}','{Start}', '{End}', '{time}', {distance})";
            sqlite_cmd.ExecuteNonQuery();
        }
        else
        {
            sqlite_cmd.CommandText = $"UPDATE WorkTime Set Start = {Start}, End = {End}, Time = {time}, Distance = {distance} WHERE Date = {date_to_insert}";
            sqlite_cmd.ExecuteNonQuery();
        }
    }
    public List<WorkTimeEntry> GetItems(DateTime start, DateTime end) // List all records between the two dates
    {
        using var conn = new SqliteConnection(DBpath);
        conn.Open();
        string s = start.ToString("s").Remove(10);
        string e = end.ToString("s").Remove(10);
        SqliteCommand sqlite_cmd = new($"SELECT * FROM WorkTime",conn);
        SqliteDataReader reader = null;
        try
        {
            reader =  sqlite_cmd.ExecuteReader();
            while (reader.Read())
            {
                string date = reader.GetString(1);
                date = date.Remove(10);
                WorkTimeEntry data = new()
                {
                    Date = date,
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
    public static void DeleteData(int id) //THIS METHOD SHOULD GET A DATE INSTEAD OF ROWID
    {
        using var conn = new SqliteConnection(DBpath);
        conn.Open();
        SqliteCommand sqlite_cmd = new($"DELETE FROM WorkTime WHERE DATE = {id}");
    }
    public static void DropTable()
    {
        using var conn = new SqliteConnection(DBpath);
        conn.Open();
        SqliteCommand sqlite_cmd = new("DROP WorkTime");
    }
}
