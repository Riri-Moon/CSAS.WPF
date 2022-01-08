using Microsoft.VisualStudio.PlatformUI;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static CSAS.Enums.Enums;

namespace CSAS.Models
{
	public class Student : BindableBase
	{
		[Key]
		public virtual int Id { get; set; }
		public string _name;
		public virtual string Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}
		public virtual string? Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}
		public virtual string? Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}
		public virtual string? PathToFolder
		{
			get => _pathToFolder;
			set => SetProperty(ref _pathToFolder, value);
		}
		public virtual MainGroup? MainGroup
		{
			get => _mainGroup;
			set => SetProperty(ref _mainGroup, value);
		}
		public virtual SubGroup? SubGroup
		{
			get => _subGroup;
			set => SetProperty(ref _subGroup, value);
		}
		public virtual List<Activity>? ListOfActivities
		{
			get
			{
				if (_listOfActivities != null)
				{
					return _listOfActivities.OrderByDescending(x => x.Deadline).ToList();
				}
				else
				{
					return _listOfActivities;
				}
			}
			set => SetProperty(ref _listOfActivities, value);
		}
		public virtual List<SubAttendances>? SubAttendances
		{
			get
			{
				if (_subAttendances != null)
				{
					return _subAttendances.OrderByDescending(x => x.Attendance.Date).ToList();
				}
				else
				{
					return _subAttendances;
				}
			}
			set => SetProperty(ref _subAttendances, value);
		}
		public virtual string SchoolEmail
		{
			get => _schoolEmail;
			set => SetProperty(ref _schoolEmail, value);
		}
		[MaxLength(1)]
		public virtual int? Year
		{
			get => _year;
			set => SetProperty(ref _year, value);
		}
		public virtual FormEnums Form
		{
			get => _form;
			set => SetProperty(ref _form, value);
		}

		[DependsOnProperty("ListOfActivities")]
		[MaxLength(3)]
		public virtual double? TotalPoints
		{
			get
			{
				double totalPts = 0;
				if (ListOfActivities != null)
				{
					foreach (var x in ListOfActivities)
					{
						if (x.EarnedPoints.HasValue)
							totalPts += x.EarnedPoints.Value;
					}

					return totalPts;
				}
				else return 0;
			}
		}
		[NotMapped]
		[DependsOnProperty("SubAttendances")]
		public virtual int? MissedLectures
		{
			get
			{
				var attendance = SubAttendances.Where(x => x.Attendance.Form == AttendanceFormEnums.Lecture);
				return attendance.Where(p => p.State == AttendanceEnums.NotPresent).Count();
			}
		}
		[NotMapped]
		[DependsOnProperty("SubAttendances")]
		public virtual int? MissedSeminars
		{
			get
			{

				var attendance = SubAttendances.Where(x => x.Attendance.Form == AttendanceFormEnums.Seminar); ;
				return attendance.Where(p => p.State == AttendanceEnums.NotPresent).Count();
			}
		}

		private bool _individualStudy;
		public virtual bool IndividualStudy
		{
			get => _individualStudy;
			set => SetProperty(ref _individualStudy, value);
		}

		[NotMapped]
		public FinalAssessment FinalAssessment 
		{
			get => _finalAssessment;
			set => SetProperty(ref _finalAssessment, value);
		}
		private FinalAssessment _finalAssessment;
		public virtual string Isic
		{
			get => _isic;
			set => SetProperty(ref _isic, value);
		}

		public string? _title;
		public string? _email;
		public MainGroup? _mainGroup;
		public SubGroup? _subGroup;
		public List<Activity>? _listOfActivities;
		public List<SubAttendances>? _subAttendances;
		public string? _schoolEmail;
		public int? _year;
		public FormEnums _form;
		public int? _totalPoints;
		public string? _isic;
		public string? _pathToFolder;
	}
}
