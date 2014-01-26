using Perforce.P4;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerforceChangelistViewer.ViewModel
{
	public class ChangelistSetViewModel : ReactiveObject
	{
		// List of changelists in the set.
		private ObservableAsPropertyHelper<IEnumerable<ChangelistViewModel>> _changelists;
		public IEnumerable<ChangelistViewModel> Changelists { get { return _changelists.Value; } }

		// The currently selected changelist.
		private ChangelistViewModel _selectedChangelist;
		public ChangelistViewModel SelectedChangelist
		{
			get { return _selectedChangelist; }
			set { this.RaiseAndSetIfChanged(ref _selectedChangelist, value); }
		}


		public ChangelistSetViewModel(IObservable<IEnumerable<Changelist>> changes)
		{
			// Convert Model changelists to View Model equivalent.
			var viewModelChanges = changes.Select(clList => clList.Select(cl => new ChangelistViewModel(cl)));

			_changelists = new ObservableAsPropertyHelper<IEnumerable<ChangelistViewModel>>(
				viewModelChanges, _ => raisePropertyChanged("Changelists"));
		}
	}
}
