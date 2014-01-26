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

			// Restore last used path. //public/images/...
			Path = Properties.Settings.Default.LastDepotPath;

			// Can press Go when the Path contains something.
			var havePathObservable = this.WhenAny(o => o.Path, s =>
				{
					return !string.IsNullOrWhiteSpace(s.Value);
				});
			var goCommand = new ReactiveCommand(havePathObservable);
			GoCommand = goCommand;

			// When we hit go, get changes from P4.
			var changes = goCommand.RegisterAsyncTask(o => p4.GetChanges(Path));

			// Also store the last used path when hitting go.
			disposables.Add(goCommand.Subscribe(o =>
				{
					Properties.Settings.Default.LastDepotPath = Path;
					Properties.Settings.Default.Save();
				}));

			// Pass the stream of change sets to the changelist set vm.
			ChangelistSet = new ChangelistSetViewModel(changes);
		}

		public void Dispose()
		{
			disposables.Dispose();
		}
	}
}
