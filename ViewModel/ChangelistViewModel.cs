using Perforce.P4;
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
		// Properties. Since this class is immutable, it does not require the notify stuff.
		public int Number { get; private set; }			// The changelist number.
		public string Description { get; private set; }	// Description of the changelist.
		public string User { get; private set; }		// The user who submitted the changelist.
		public string Client { get; private set; }		// Clientspec the changelist was submitted by.

		// Construct from Model representation.
		public ChangelistViewModel(Changelist changelist)
		{
			Number = changelist.Id;
			Description = changelist.Description;
			User = changelist.OwnerName;
			Client = changelist.ClientId;
		}
	}
}
