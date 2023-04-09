namespace WorkTime.ViewModel;

public partial class WorkTimeMainViewModel : BaseViewModel
{
    public ObservableCollection<WorkTimeEntry> Workentries { get; } = new();
    public WorkTimeMainViewModel() 
    {
        WorkData wd = new();
        List<WorkTimeEntry> entries = wd.GetItems();
        foreach (WorkTimeEntry entry in entries)
        {
            Workentries.Add(entry);
        }
    }
}