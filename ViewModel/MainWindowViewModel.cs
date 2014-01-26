using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PerforceChangelistViewer.ViewModel
{
	// Main view-model for the app.
	public class MainWindowViewModel : ReactiveObject
	{
		public ICommand GoCommand { get; private set; }

		// Path to get changelists for
		private string _path;
		public string Path
		{
			get { return _path; }
			set { this.RaiseAndSetIfChanged(ref _path, value); }
		}

		// The retrieved set of changelists.
		private ChangelistSetViewModel _changelistSet = new ChangelistSetViewModel();
		public ChangelistSetViewModel ChangelistSet
		{
			get { return _changelistSet; }
			set { this.RaiseAndSetIfChanged(ref _changelistSet, value); }
		}


		public MainWindowViewModel()
		{
			// TEMP
			Path = "//depot/UE4/dev/...";

			var havePathObservable = this.WhenAny(o => o.Path, s =>
				{
					return !string.IsNullOrWhiteSpace(s.Value);
				});
			var goCommand = new ReactiveCommand(havePathObservable);
			GoCommand = goCommand;
			goCommand.Subscribe(o =>
				{
					// Temp: add some dummy changelists.
					ChangelistSet.Changelists.Clear();
					ChangelistSet.Changelists.Add(new ChangelistViewModel(1234, "staylor"));
					ChangelistSet.Changelists.Add(new ChangelistViewModel(2345, "staylor"));
					ChangelistSet.Changelists.Add(new ChangelistViewModel(3456, "staylor"));
					ChangelistSet.Changelists.Add(new ChangelistViewModel(4567, "staylor"));
				});
		}
	}
}
