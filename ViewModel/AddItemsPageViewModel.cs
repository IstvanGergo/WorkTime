namespace WorkTime.ViewModel;

public partial class AddItemsPageViewModel : BaseViewModel
{
    public ObservableObject WorkTimeEntry { get; set; }
    public AddItemsPageViewModel()
    {
        Vdate = DateTime.Now;
    }

    private DateTime Vdate;
    public DateTime VDate
    {
        get { return Vdate; }
        set
        {
            Vdate = value;
            OnPropertyChanged(nameof(Vdate));
        }
    }

    private TimeSpan Vstart;
    public TimeSpan VStart
    {
        get { return Vstart; }
        set
        {
            SetProperty(ref Vstart, value);
            OnPropertyChanged(nameof(Vstart));
        }
    }

    private TimeSpan Vend;
    public TimeSpan VEnd
    {
        get { return Vend; }
        set
        {
            Vend = value;
            OnPropertyChanged(nameof(Vend));
        }
    }

    private long Vdistance;
    public long VDistance
    {
        get { return Vdistance; }
        set
        {
            Vdistance = value;
            OnPropertyChanged(nameof(Vdistance));
        }
    }

    [ICommand]
    Task AddItem()
    {
        
        WorkData.InsertData(Vdate,Vstart,Vend,Vdistance);
        return Task.CompletedTask;
    }
}
