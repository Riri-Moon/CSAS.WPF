using Microsoft.Win32;
using System.IO;
using static CSAS.Enums.Enums;

namespace CSAS.ViewModels
{
	public class SettingsViewModel : BaseViewModelBindableBase
	{
		public DelegateCommand RecalculateCommand { get; }
		public DelegateCommand SaveCommand { get; }
		public DelegateCommand SetSignatureCommand { get; }

		private Settings _settings;

		public Settings Settings
		{

			get { return _settings; }
			set => SetProperty(ref _settings, value);
		}

		public SettingsViewModel(string currGroupId)
		{
			Work = UoWSingleton.Instance;
			Settings = new Settings
			{
				MainGroup = Work.MainGroup.Get(currGroupId)
			};
			if (Work.Settings.GetAll().FirstOrDefault(x => x.MainGroup.Id == currGroupId) == null)
			{
				var temp = Work.Settings.GetAll().FirstOrDefault();
				if (temp != null)
				{
					Settings.Email = temp.Email;
					Settings.Name = temp.Name;
					Settings.LastName = temp.LastName;
					Settings.Title = temp.Title;
					Settings.TitleAfterName = temp.TitleAfterName;
					Settings.Signature = temp.Signature;
				}
				Work.Settings.Add(Settings);
				Work.Complete();
			}
			else
			{
				Settings = Work.Settings.GetAll().FirstOrDefault(x => x.MainGroup.Id == currGroupId);

			}
			SaveCommand = new DelegateCommand(SaveSettings);
			RecalculateCommand = new DelegateCommand(Recalculate);
			SetSignatureCommand = new DelegateCommand(SetSignaturePath);
		}

		private Grade GetGrade(double pts)
		{
			var grades = Work.Settings.GetAll().FirstOrDefault(x => x.MainGroup.Id == CurrentMainGroupId);
			var percentage = grades.MaxPoints.Value / 100;

			if (pts >= grades.A * percentage)
			{
				return Grade.A;
			}
			if (pts >= grades.B * percentage)
			{
				return Grade.B;
			}
			if (pts >= grades.C * percentage)
			{
				return Grade.C;
			}
			if (pts >= grades.D * percentage)
			{
				return Grade.D;
			}
			if (pts >= grades.E * percentage)
			{
				return Grade.E;
			}
			else
			{
				return Grade.Fx;
			}
		}
		private void Recalculate()
		{
			Work = UoWSingleton.Instance;
			foreach (var student in Work.Students.GetStudentsByGroup(Work.MainGroup.Get(CurrentMainGroupId)))
			{
				if(student.FinalAssessment != null && student.FinalAssessment.Grade != null)
				{
					student.FinalAssessment.Grade = GetGrade(student.TotalPoints.Value);
					Work.Students.Update(student);
				}
			}
			Work.Complete();
		}

		private void SetSignaturePath()
		{
			OpenFileDialog fileDialog = new();

			if (fileDialog.ShowDialog().Value)
			{
				if (Path.GetExtension(fileDialog.FileName) == ".htm")
				{
					Settings.Signature = fileDialog.FileName;
				}
			}
		}

		private void SaveSettings()
		{
			Work.Settings.Update(Settings);
			Work.Complete();
		}
	}
}
