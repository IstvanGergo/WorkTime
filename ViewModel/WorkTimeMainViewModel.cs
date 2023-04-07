namespace WorkTime.ViewModel;

public partial class WorkTimeMainViewModel : BaseViewModel
{
    public ObservableCollection<WorkTimeEntry> Workentries { get; } = new();
    public WorkTimeMainViewModel()
    {
        SqliteConnection conn = WorkData.CreateConnection();
        conn.Open();
        WorkData.DropTable(conn);
        WorkData.CreateTable(conn);
        WorkData.InsertData(conn, "2022.10.10", "08:30", "16:30", 154);
        WorkData wd = new();
        List<WorkTimeEntry> entries = wd.GetItems(conn);
        entries.ForEach(Workentries.Add);
    }
}