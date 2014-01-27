﻿using Perforce.P4;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using PerforceChangelistViewer.Util;
using System.Collections.Concurrent;
using System.Reactive;

namespace PerforceChangelistViewer.ViewModel
{
	public class ChangelistSetViewModel : ReactiveObject
	{
		// List of changelists in the set.
		private ObservableAsPropertyHelper<IEnumerable<ChangelistViewModel>> _changelists;
		public IEnumerable<ChangelistViewModel> Changelists
		{
			get { return _changelists.Value; }
		}

		// Set of users in the changelist set, and whether to include them in the filtered view.
		private ObservableAsPropertyHelper<IEnumerable<FilteredUserViewModel>> _usersFilter;
		public IEnumerable<FilteredUserViewModel> UsersFilter
		{
			get { return _usersFilter.Value; }
		}

		// The currently selected changelist.
		private ChangelistViewModel _selectedChangelist;
		public ChangelistViewModel SelectedChangelist
		{
			get { return _selectedChangelist; }
			set { this.RaiseAndSetIfChanged(ref _selectedChangelist, value); }
		}

		// Dictionary of exiting filtered users.
		// Independent of contents of the changelist set so changes to filter aren't lost
		// when changing the changelist set.
		// Concurrent as observable notifications might come on any thread.
		private ConcurrentDictionary<string, FilteredUserViewModel> allFilteredUsers = new ConcurrentDictionary<string, FilteredUserViewModel>();


		public ChangelistSetViewModel(IObservable<IEnumerable<Changelist>> changes)
		{
			// Get all unique usernames in the changelist set.
			var usersObservable = changes.Select(clList =>
				clList
					.Select(cl => cl.OwnerName)
					.Distinct()
					.Select(user => allFilteredUsers.GetOrAdd(user, () => new FilteredUserViewModel(user)))
			);
			_usersFilter = new ObservableAsPropertyHelper<IEnumerable<FilteredUserViewModel>>(
				usersObservable, _ => raisePropertyChanged("UsersFilter"));

			// Unit Observable that fires any time a user's IsSelected is changed.
			var anyIsSelectedChanges = usersObservable.SelectMany(userList =>
				Observable.Merge(userList.Select(user => user.ObservableForProperty(u => u.IsSelected).Select(o => Unit.Default))));

			// Update filered changelist set when the changes change, or an IsSelected flag is modified.
			var filteredChanges = changes.CombineLatest(
				// Make sure we have at least one item so we don't wait for the first time IsSelected is changed.
				anyIsSelectedChanges.StartWith(Unit.Default),
				(clList, unit) =>
					clList.Where(cl =>
						{
							var user = allFilteredUsers.GetValueOrFallback(cl.OwnerName, null);
							if (user != null)
								return user.IsSelected;
							return true;
						}));

			// Convert Model changelists to View Model equivalent.
			var viewModelChanges = filteredChanges.Select(clList => clList.Select(cl => new ChangelistViewModel(cl)));

			_changelists = new ObservableAsPropertyHelper<IEnumerable<ChangelistViewModel>>(
				viewModelChanges, _ => raisePropertyChanged("Changelists"));
		}
	}
}
