namespace WorkTime.Model;

public class WorkTimeEntry
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Date { get; set; }
    public string Start { get; set; }
    public string End { get; set; }
    public string Time { get; set; }
    public long Distance { get; set; }
}
