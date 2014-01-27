using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerforceChangelistViewer.ViewModel
{
	public class FilteredUserViewModel : ReactiveObject
	{
		// The username. Constant.
		public string Username { get; private set; }

		// Whether the user is included in the filter or not. Mutable. Included by default.
		private bool _isSelected = true;
		public bool IsSelected
		{
			get { return _isSelected; }
			set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
		}


		public FilteredUserViewModel(string username)
		{
			Username = username;
		}
	}
}
