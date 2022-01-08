using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Drawing;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using static CSAS.Enums.Enums;
using CSAS.Enums;
using CSAS.Helpers;

namespace CSAS.ViewModels
{
	public class StatisticsViewModel : BaseDataViewModel
	{
		public DelegateCommand RefreshCommand { get; }
		public StatisticsViewModel(int id, ref AppDbContext appDbContext)
		{
			CurrentMainGroupId = id;
			AppDbContext = appDbContext;
			Work = new UnitOfWork(appDbContext);
			IsAll = true;
			IsActivity = true;
			Groups = new ObservableCollection<SubGroup>(Work.SubGroup.GetAll().Where(g => g.MainGroup.Id == CurrentMainGroupId));
			Refresh();
			RefreshCommand = new DelegateCommand(Refresh);


		}
		private List<ISeries> _series = new();
		public List<ISeries> Series
		{
			get => _series;
			set => SetProperty(ref _series, value);
		}
		private List<Axis> _xaxes;
		public List<Axis> XAxes
		{
			get => _xaxes;
			set => SetProperty(ref _xaxes, value);
		}

		private double _avgPtsPerAct;
		public double AvgPtsPerAct
		{
			get => _avgPtsPerAct;
			set => SetProperty(ref _avgPtsPerAct, value);
		}
		private double _avgPtsPerTask;
		public double AvgPtsPerTask
		{
			get => _avgPtsPerTask;
			set => SetProperty(ref _avgPtsPerTask, value);
		}
		private double _avgPerLecture;
		public double AvgPerLecture
		{
			get => _avgPerLecture;
			set => SetProperty(ref _avgPerLecture, value);
		}
		private double _avgPerSeminar;
		public double AvgPerSeminar
		{
			get => _avgPerSeminar;
			set => SetProperty(ref _avgPerSeminar, value);
		}

		private string _avgGrade;
		public string AvgGrade
		{
			get => _avgGrade;
			set => SetProperty(ref _avgGrade, value);
		}
		public IEnumerable<ISeries> PieSeries { get; set; } = new List<ISeries>
		{
			new PieSeries<double> { Values = new List<double> { 4 }, InnerRadius = 70 },
			new PieSeries<double> { Values = new List<double> { 5 }, InnerRadius = 70 },
			new PieSeries<double> { Values = new List<double> { 3 }, InnerRadius = 70 },
			new PieSeries<double> { Values = new List<double> { 5 }, InnerRadius = 70 },
			new PieSeries<double> { Values = new List<double> { 7 }, InnerRadius = 70 },
		};

		private void Refresh()
		{
			CreatePointsChart();
			AvgPtsPerAct = GetAveragePointsForActivity();
			AvgPtsPerTask = GetAveragePointsForTask();
			AvgPerLecture = GetAverageAttendanceLecture();
			AvgPerSeminar = GetAverageAttendanceSeminar();
			AvgGrade = EnumExtension.GetDescriptionValue<Grade>( GetAverageGrade().ToString());
		}

		private void CreatePointsChart()
		{
			
			List<Student> list = GetStudents();

			if (list == null)
				return;
			Series = new();
			Axis axis = new();
			axis.Labels = new List<string>();
			XAxes = new();
			XAxes.Add(new Axis());
			XAxes[0].Position = AxisPosition.Start;
			XAxes[0].TextSize = 20;
			XAxes[0].Labels = new List<string>();

			foreach (var x in list)
			{
				Series.Add(new ColumnSeries<double>()
				{
					Name = x.Name,
					Values = new double[] { x.TotalPoints.Value },
				});
			}
			XAxes[0].Labels.Add("Študenti - Body");
		}

		private double GetAveragePointsForTask()
		{
			var studs = GetStudents();
			if (studs == null)
			{
				return 0;
			}
			var count = studs.Select(x => x.ListOfActivities.Select(p => p.Tasks)).Sum(m => m.Sum(x => x.Count));
			var studentCount = studs.Sum(i => i.TotalPoints.Value);
			return Math.Round((studentCount / count), 2);
		}

		private double GetAverageAttendanceLecture()
		{
			var studs = GetStudents();
			if (studs == null)
			{
				return 0;
			}
			List<Attendance> attendances = new();
			foreach (var stud in studs)
			{
				attendances.AddRange(stud.SubAttendances.Select(x => x.Attendance));
			}
			var attend = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Lecture).Select(x => x.SubAttendances.Count).Sum(x => x);
			var lec = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Lecture);
			var attendPresent = lec.Select(x => x.SubAttendances.Where(x => x.State == AttendanceEnums.IsPresent)).Sum(o => o.Count());

			if (IsStudent)
			{
				attend = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Lecture).Select(x => x.SubAttendances.FirstOrDefault(y => y.Student == SelectedStudent)).Count();
				lec = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Lecture);
				attendPresent = lec.Select(x => x.SubAttendances.Where(x => x.State == AttendanceEnums.IsPresent && x.Student == SelectedStudent)).Sum(o => o.Count());

			}
			else if (IsGroup)
			{
				IEnumerable<SubAttendances>? att3 = attendances.Where(x => x.SubGroup == null).SelectMany(x => x.SubAttendances
				.Where(y => y.Student.SubGroup == SelectedGroup));

				attend = att3.Count();
				attendPresent = att3.Where(x => x.State == AttendanceEnums.IsPresent).Count();//lec.Select(x => x.SubAttendances.Where(x => x.State == AttendanceEnums.IsPresent)).Sum(o => o.Count());
			}
			double percentage = attendPresent / ((double)attend);

			return Math.Round(percentage * 100, 2);
		}
		private double GetAverageAttendanceSeminar()
		{
			var studs = GetStudents();
			if (studs == null)
			{
				return 0;
			}
				List<Attendance> attendances = new();
			foreach (var stud in studs)
			{
				attendances.AddRange(stud.SubAttendances.Select(x => x.Attendance));
			}
			var attend = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Seminar).Select(x => x.SubAttendances.Count).Sum(x => x);
			var lec = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Seminar);
			var attendPresent = lec.Select(x => x.SubAttendances.Where(x => x.State == AttendanceEnums.IsPresent)).Sum(o => o.Count());

			if (IsStudent)
			{
				attend = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Seminar).Select(x=>x.SubAttendances.FirstOrDefault(y => y.Student == SelectedStudent)).Count();
				lec = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Seminar);
				attendPresent = lec.Select(x => x.SubAttendances.Where(x => x.State == AttendanceEnums.IsPresent && x.Student == SelectedStudent)).Sum(o => o.Count());

			}
			else if (IsGroup)
			{
				attend = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Seminar && x.SubGroup == SelectedGroup).Select(x => x.SubAttendances.Count).Sum(x => x);
				lec = attendances.Distinct().Where(x => x.Form == AttendanceFormEnums.Seminar && x.SubGroup==SelectedGroup);
				attendPresent = lec.Select(x => x.SubAttendances.Where(x => x.State == AttendanceEnums.IsPresent)).Sum(o => o.Count());

			}
			double percentage = attendPresent/ ((double)attend );

			return Math.Round(percentage * 100, 2);
		}

		private double GetAveragePointsForActivity()
		{
			var studs = GetStudents();
			if (studs != null)
			{
				var count = studs.Select(x => x.ListOfActivities).Sum(x => x.Count);
				var studentCount = studs.Sum(i => i.TotalPoints.Value);
				return Math.Round((studentCount / count), 2);
			}
			
			return 0;
		}

		private Grade GetAverageGrade()
		{
			IEnumerable<FinalAssessment> assessments = null;
			if (IsGroup && SelectedGroup != null)
			{
				 assessments = Work.FinalAssessment.GetAll().Where(x => x.Student.SubGroup.Id == SelectedGroup.Id);
			}
			else if (IsAll)
			{
				 assessments = Work.FinalAssessment.GetAll().Where(x => x.Student.MainGroup.Id == CurrentMainGroupId);
			}
			if (assessments != null)
			{
				var sum = assessments.Sum(x => (int)x.Grade);

				return (Grade)(sum / assessments.Count());
			}
			else
			{
				return Grade.Empty;
			}
		}
	}
}