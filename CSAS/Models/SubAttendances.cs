using Newtonsoft.Json;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CSAS.Enums.Enums;

namespace CSAS.Models
{
	public class SubAttendances : BindableBase
	{
		[Key]
		[JsonProperty("id")]
		public virtual string Id { get; set; }

		private Attendance _attendance;
		public virtual Attendance Attendance
		{
			get => _attendance;
			set => SetProperty(ref _attendance, value);
		}

		private Student _student;
		public virtual Student Student
		{
			get => _student;
			set => SetProperty(ref _student, value);
		}
		public virtual AttendanceEnums State
		{
			get => _state;
			set => SetProperty(ref _state, value);
		}
		private AttendanceEnums _state;
		[NotMapped]
		public bool IsSelected
		{
			get => _isSelected;
			set => SetProperty(ref _isSelected, value);
		}
		private bool _isSelected = false;
	}
}
