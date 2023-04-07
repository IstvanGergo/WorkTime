namespace WorkTime.ViewModel;

public partial class WorkTimeMainViewModel : BaseViewModel
{
    public ObservableCollection<WorkTimeEntry> Workentries { get; } = new();
    public WorkTimeMainViewModel() 
    {
        WorkData.DropTable();
        WorkData.CreateTable();
        WorkData wd = new();
        List<WorkTimeEntry> entries = wd.GetItems();
        entries.ForEach(Workentries.Add);
    }
}