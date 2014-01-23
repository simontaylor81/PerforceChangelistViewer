using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerforceChangelistViewer.ViewModel
{
    class ChangelistSetViewModel : ReactiveObject
    {
        // List of changelists in the set.
        private ReactiveList<ChangelistViewModel> _changelists;
        public ReactiveList<ChangelistViewModel> Changelists
        {
            get { return _changelists; }
            set { this.RaiseAndSetIfChanged(ref _changelists, value); }
        }
    }
}
