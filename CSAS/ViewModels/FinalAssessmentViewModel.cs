using CSAS.Models;
using CSAS.Repositories;
using CSAS.Services;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.ViewModels
{
    public class FinalAssessmentViewModel : BaseViewModelBindableBase
    {
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand SaveCommand { get; }

        private ObservableCollection<Student> _students;

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        public Array Grades
        {
            get
            {
                List<string> arr = new();

                foreach (var grade in Enum.GetValues(typeof(Enums.Enums.Grade)))
                {
                    arr.Add(Enums.EnumExtension.GetDescriptionFromEnumValue<Enums.Enums.Grade>(grade.ToString()));
                }
                return arr.ToArray();
            }
        }
        private Student _student;
        public Student SelectedStudent
        {

            get => _student;
            set
            {
                var assessment = _work.FinalAssessment.GetAll().FirstOrDefault(x => x.Student.Id == value.Id);
                if (assessment != null)
                    FinalAssessment = assessment;
                else
                {
                    FinalAssessment = new FinalAssessment();
                    FinalAssessment.Grade = GetGrade(value.TotalPoints.Value);
                }
                SetProperty(ref _student, value);
            }
        }
        private FinalAssessment _finalAssessment;
        public FinalAssessment FinalAssessment
        {
            get => _finalAssessment;
            set => SetProperty(ref _finalAssessment, value);
        }

        private UnitOfWork _work;
        public FinalAssessmentViewModel(int currentGroupId, ref AppDbContext context)
        {
            _work = new UnitOfWork(context);
            CurrentMainGroupId = currentGroupId;
            RefreshCommand = new DelegateCommand(Reload);
            Students = new ObservableCollection<Student>(_work.Students.GetStudentsByGroup(_work.MainGroup.Get(CurrentMainGroupId)));
            SaveCommand = new DelegateCommand(SaveFinalAssessment);
        }

        private void Reload()
        {
            _work = new UnitOfWork(new AppDbContext());
            Students = new ObservableCollection<Student>(_work.Students.GetStudentsByGroup(_work.MainGroup.Get(CurrentMainGroupId)));
        }

        private Enums.Enums.Grade GetGrade(double pts)
        {
            var grades = _work.Settings.GetAll().FirstOrDefault();

            if (pts >= grades.A)
            {
                return Enums.Enums.Grade.A;
            }
            if (pts >= grades.B)
            {
                return Enums.Enums.Grade.B;
            }
            if (pts >= grades.C)
            {
                return Enums.Enums.Grade.C;
            }
            if (pts >= grades.D)
            {
                return Enums.Enums.Grade.D;
            }
            if (pts >= grades.E)
            {
                return Enums.Enums.Grade.E;
            }
            else
            {
                return Enums.Enums.Grade.Fx;
            }
        }

        private void SaveFinalAssessment()
        {
            if (FinalAssessment.IsNew)
            {
                FinalAssessment.Student = SelectedStudent;
                FinalAssessment.IsNew = false;
                _work.FinalAssessment.Add(FinalAssessment);
            }
            else
            {
                _work.FinalAssessment.Update(FinalAssessment);
            }
            _work.Complete();

            if (FinalAssessment.IsSendEmail)
            {
                OutlookService outlookService = new();
                //outlookService.SendEmail()
            }
        }
    }
}
