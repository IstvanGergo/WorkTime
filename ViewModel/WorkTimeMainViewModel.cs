namespace WorkTime.ViewModel;

public partial class WorkTimeMainViewModel : BaseViewModel
{
    
    public WorkTimeMainViewModel()
    {
        SqliteConnection conn = WorkData.CreateConnection();
        conn.Open();
        WorkData.CreateTable(conn);
        WorkData.InsertData(conn);
        ObservableCollection<WorkTimeEntry> entries = WorkData.GetItems(conn);
    }
}
