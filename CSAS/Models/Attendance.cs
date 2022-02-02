using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CSAS.Enums.Enums;

namespace CSAS.Models
{
	[Table("Attendances")]
	public class Attendance : BindableBase
	{
		[JsonProperty("id")]
		public virtual string Id { get; set; }

		private List<SubAttendances> _subAttendances;
		public virtual List<SubAttendances> SubAttendances
		{
			get => _subAttendances;
			set => SetProperty(ref _subAttendances, value);
		}
		public virtual string StudyForm
		{
			get => _studyForm;
			set => SetProperty(ref _studyForm, value);
		}
		public virtual AttendanceFormEnums Form
		{
			get => _form;
			set => SetProperty(ref _form, value);
		}
		public virtual DateTime Date
		{
			get => _date;
			set => SetProperty(ref _date, value);
		}
		public virtual string Day
		{
			get => _day;
			set => SetProperty(ref _day, value);
		}
		public virtual MainGroup MainGroup
		{
			get => _mainGroup;
			set => SetProperty(ref _mainGroup, value);
		}
		[BackingField(nameof(_subgroup))]
		public virtual SubGroup? SubGroup
		{
			get => _subgroup;
			set => SetProperty(ref _subgroup, value);
		}

		private SubGroup? _subgroup;
		private string _studyForm;
		private AttendanceFormEnums _form;
		private DateTime _date;
		private string _day;
		private MainGroup _mainGroup;


	}
}