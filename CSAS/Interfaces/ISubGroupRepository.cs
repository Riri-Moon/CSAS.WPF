using CSAS.Models;

namespace CSAS.Interfaces
{
	public interface ISubGroupRepository : IRepository<SubGroup>
	{
		IEnumerable<SubGroup> GetSubGroupByMainGroup(MainGroup group);

	}
}
