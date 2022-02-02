using Newtonsoft.Json;

namespace CSAS.Models
{
	public class UserInfo
	{
		[JsonProperty("id")]
		public virtual string Id { get; set; }
		public virtual string query { get; set; } = "";
		public virtual string status { get; set; } = "";
		public virtual string country { get; set; } = "";
		public virtual string countryCode { get; set; } = "";
		public virtual string region { get; set; } = "";
		public virtual string regionName { get; set; } = "";
		public virtual string city { get; set; } = "";
		public virtual string zip { get; set; } = "";
		public virtual double lat { get; set; } = 0;
		public virtual double lon { get; set; } = 0;
		public virtual string timezone { get; set; } = "";
		public virtual string isp { get; set; } = "";
		public virtual string org { get; set; } = "";
		public virtual string @as { get; set; } = "";
		public virtual DateTime Time { get; set; } = DateTime.Now;
		public virtual string MachineName { get; set; } = "";
	}
}
