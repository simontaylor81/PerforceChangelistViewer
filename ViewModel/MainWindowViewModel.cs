using PerforceChangelistViewer.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PerforceChangelistViewer.ViewModel
{
	// Main view-model for the app.
	public class MainWindowViewModel : ReactiveObject, IDisposable
	{
		public ICommand GoCommand { get; private set; }


		// Path to get changelists for
		private string _path;
		public string Path
		{
			get { return _path; }
			set { this.RaiseAndSetIfChanged(ref _path, value); }
		}

		// The retrieved set of changelists Doesn't change so no need for notification stuff.
		public ChangelistSetViewModel ChangelistSet { get; private set; }

		private P4Async p4;
		private CompositeDisposable disposables = new CompositeDisposable();

		public MainWindowViewModel()
		{
			// Connect to p4.
			p4 = new P4Async("public.perforce.com:1666");
			disposables.Add(p4);

			ChangelistSet = new ChangelistSetViewModel(p4);
			disposables.Add(ChangelistSet);

			// Can press Go when the Path contains something.
			var havePathObservable = this.ObservableForProperty(o => o.Path, s => !string.IsNullOrWhiteSpace(s) );

			var goCommand = new ReactiveCommand(havePathObservable.CombineLatest(
				ChangelistSet.FetchChangesCommand.CanExecuteObservable,
				(a, b) => a && b));
			GoCommand = goCommand;

			// Fire fetch changes command when the user presses Go.
			disposables.Add(
				goCommand.Subscribe(_ => ChangelistSet.FetchChangesCommand.Execute(Path)));

			// Also store the last used path when hitting go.
			disposables.Add(goCommand.Subscribe(o =>
				{
					Properties.Settings.Default.LastDepotPath = Path;
					Properties.Settings.Default.Save();
				}));

			// Restore last used path. //public/images/...
			Path = Properties.Settings.Default.LastDepotPath;
		}

		public void Dispose()
		{
			disposables.Dispose();
		}
	}
}
