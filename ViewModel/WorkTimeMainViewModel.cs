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
        WorkData wd = new();
        List<WorkTimeEntry> entries = wd.GetItems(conn);
        entries.ForEach(Workentries.Add);
    }
}