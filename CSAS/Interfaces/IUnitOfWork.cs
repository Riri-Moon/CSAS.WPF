using System;

namespace CSAS.Interfaces
{
	public interface IUnitOfWork : IDisposable
	{
		IStudentRepository Students { get; }
		int Complete();
	}
}
