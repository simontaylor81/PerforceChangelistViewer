using Perforce.P4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerforceChangelistViewer.Model
{
	public class P4Async : IDisposable
	{
		private Repository rep;
		private CompositeDisposable disposables = new CompositeDisposable();

		public P4Async(string serverAddress)
		{
			var server = new Server(new ServerAddress(serverAddress));
			rep = new Repository(server);

			// Connect to the server.
			rep.Connection.Connect(null);

			disposables.Add(rep);
			disposables.Add(rep.Connection);
		}

		public Task<IEnumerable<Changelist>> GetChanges(string depotPath)
		{
			// P4API has no native async support, so just run on the thread pool.
			return Task.Run(() =>
				{
					var options = new ChangesCmdOptions(ChangesCmdFlags.FullDescription,
						null, 100, ChangeListStatus.Submitted, null);

					IEnumerable<Changelist> changes = rep.GetChangelists(options, FileSpec.DepotSpec(depotPath));
					return changes;
				});
		}

		public void Dispose()
		{
			disposables.Dispose();
		}
	}
}
