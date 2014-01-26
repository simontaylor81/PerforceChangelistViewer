using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerforceChangelistViewer.ViewModel
{
	public class ChangelistSetViewModel : ReactiveObject
	{
		// List of changelists in the set.
		private ReactiveList<ChangelistViewModel> _changelists = new ReactiveList<ChangelistViewModel>();
		public ReactiveList<ChangelistViewModel> Changelists
		{
			get { return _changelists; }
			set { this.RaiseAndSetIfChanged(ref _changelists, value); }
		}

		// The currently selected changelist.
		private ChangelistViewModel _selectedChangelist;
		public ChangelistViewModel SelectedChangelist
		{
			get { return _selectedChangelist; }
			set { this.RaiseAndSetIfChanged(ref _selectedChangelist, value); }
		}
	}
}
