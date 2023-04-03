namespace WorkTime.ViewModel;

public partial class WorkTimeMainViewModel : BaseViewModel
{
    public ObservableCollection<WorkTimeEntry> Entries { get; } = new();
    public WorkTimeMainViewModel()
    {
        WorkTimeEntry workTimeEntry = new WorkTimeEntry
        {
            Day = "2023.01.01",
            Start = "8:30",
            Distance = "123",
            End = "12:30",
            Time = "8:00"
        };
        Entries.Add(workTimeEntry);
    }
}
