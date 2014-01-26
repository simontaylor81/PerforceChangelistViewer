using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerforceChangelistViewer.ViewModel
{
	public class ChangelistViewModel : ReactiveObject
	{
		// The changelist number.
		private int _number;
		public int Number
		{
			get { return _number; }
			set { this.RaiseAndSetIfChanged(ref _number, value); }
		}

		// The user who submitted the changelist
		private string _user;
		public string User
		{
			get { return _user; }
			set { this.RaiseAndSetIfChanged(ref _user, value); }
		}


		public ChangelistViewModel(int number, string user)
		{
			Number = number;
			User = user;
		}
	}
}
