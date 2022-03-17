using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Repositories
{

	public sealed class UoWSingleton
	{
		private static readonly Lazy<UnitOfWork> lazy =
		new(() => new UnitOfWork(new AppDbContext()));
		public static UnitOfWork Instance { get { return lazy.Value; } }

		private UoWSingleton()
		{

		}
	}
}
