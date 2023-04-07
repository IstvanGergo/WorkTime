namespace WorkTime.ViewModel;

public partial class WorkTimeMainViewModel : BaseViewModel
{
    public ObservableCollection<WorkTimeEntry> Workentries { get; } = new();
    public WorkTimeMainViewModel() 
    {
        WorkData.DropTable();
        WorkData.CreateTable();
        WorkData.InsertData("2020.10.10", "10:10", "20:20", 156);
        WorkData wd = new();
        
        List<WorkTimeEntry> entries = wd.GetItems();
        foreach (WorkTimeEntry entry in entries)
        {
            Workentries.Add(entry);
        }
    }
}