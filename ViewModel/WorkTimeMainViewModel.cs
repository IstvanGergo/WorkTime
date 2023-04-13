namespace WorkTime.ViewModel;

public partial class WorkTimeMainViewModel : BaseViewModel
{
    public ObservableCollection<WorkTimeEntry> Workentries { get; } = new();
    public WorkTimeMainViewModel() 
    {
        enddate = new(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        startdate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
    }
    private DateTime startdate;

    public DateTime Startdate
    {
        get { return startdate; }
        set
        {
            SetProperty(ref startdate, value);
            OnPropertyChanged(nameof(startdate));
        }
    }
    
    private DateTime enddate;
    public DateTime Enddate
    {
        get { return enddate; }
        set
        {
            SetProperty(ref enddate, value);
            OnPropertyChanged(nameof(enddate));
        }
    }
    
    [ICommand]
    public Task GetEntries()
    {
        WorkData wd = new();
        List<WorkTimeEntry> entries = wd.GetItems(startdate, enddate);
        foreach (WorkTimeEntry entry in entries)
        {
            Workentries.Add(entry);
        }
        return Task.FromResult(entries);
        
    }
    
}