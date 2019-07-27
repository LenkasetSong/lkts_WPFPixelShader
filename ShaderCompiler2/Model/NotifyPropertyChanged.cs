using System.ComponentModel;

public class NotifyPropertyChanged: INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaiseNotifyChanged(string propName)
	{
		if (this.PropertyChanged != null)
		{
			this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
		}
	}
}

