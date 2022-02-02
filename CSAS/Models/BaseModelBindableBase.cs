using Newtonsoft.Json;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace CSAS.Models
{
	public class BaseModelBindableBase : BindableBase
	{
		//[Key]
		[JsonProperty("id")]
		public virtual string Id { get; set; }
	}
}
