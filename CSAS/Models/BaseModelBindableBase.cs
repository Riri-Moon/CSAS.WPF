using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace CSAS.Models
{
	public class BaseModelBindableBase : BindableBase
	{
		[Key]
		public virtual int Id { get; set; }
	}
}
