using SQLitePCL;
using System.Runtime.Intrinsics.Arm;

namespace WorkTime.Services;

public class WorkData
{
    public const string DatabaseName = "WorkTime.db";
    public static string DatabasePath
    {
        get
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string path =Path.Combine(basePath +"/"+ DatabaseName);
            return $"Data Source={path}";
        }
    }
    public WorkData()
    {

    }
    public static SqliteConnection CreateConnection()
    {
        SqliteConnection sqlite_conn;
        // Create a new database connection:
        sqlite_conn = new SqliteConnection(DatabasePath);
        try
        {
            sqlite_conn.Open();
        }
        catch (Exception ex)
        {

        }
        return sqlite_conn;
    }
    public static void CreateTable (SqliteConnection conn)
    {
        
        SqliteCommand command = conn.CreateCommand();
        command.CommandText = @"
        CREATE TABLE IF NOT EXISTS WorkTime (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Date TEXT,
        Start TEXT,
        End TEXT,
        Time TEXT,
        Distance INTEGER
        );";
        command.ExecuteNonQuery();
    }
    public static void InsertData(SqliteConnection conn)
    {
        SqliteCommand sqlite_cmd = conn.CreateCommand();
        sqlite_cmd.CommandText = "INSERT INTO WorkTime" +
            "(Date, Start, End, Time, Distance) VALUES ('2020.01.01', '6:30', '8:30', '8:00', 156)," +
            "('2021.05.06', '1:30', '18:30', '20:00', 156)";
        sqlite_cmd.ExecuteNonQuery();
    }
    public static ObservableCollection<WorkTimeEntry> GetItems(SqliteConnection conn)
    {
        var list = new ObservableCollection<WorkTimeEntry>();
        SqliteCommand sqlite_cmd = new("SELECT * FROM WorkTime", conn);
        SqliteDataReader reader = null;
        try
        {
            
            conn.Open();
            reader = sqlite_cmd.ExecuteReader();
            while (reader.Read())
            {
                WorkTimeEntry data = new WorkTimeEntry();
                data.Date = (string)reader[1];
                data.Start = (string)reader[2];
                data.End = (string)reader[3];
                data.Time = (string)reader[4];
                data.Distance = (long)reader[5];
                list.Add(data);
            }
        }
        finally
        {
            if (reader != null)
            {
                reader.Close();
            }
            if (conn != null)
            {
                conn.Close();
            }
        }
        
        return list;
    }
}
