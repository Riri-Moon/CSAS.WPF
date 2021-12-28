using CSAS.Models;
using CSAS.Repositories;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using CSAS.Validators;
using System.Windows;
using CSAS.Views;
using CSAS.Services;
using System.Net.Mail;

namespace CSAS.ViewModels
{
    public class HomeViewModel : BaseViewModelBindableBase
    {
        private bool _isAddStudent;
        public bool IsAddStudent
        {
            get { return _isAddStudent; }
            set
            {
                SetProperty(ref _isAddStudent, value);
            }
        }
        private bool _isModifyStudent;
        public bool IsModifyStudent
        {
            get { return _isModifyStudent; }
            set
            {
                SetProperty(ref _isModifyStudent, value);
            }
        }
        public DelegateCommand SetSubGroupCommand { get; }
        public DelegateCommand<int?> ModifyStudentCommand { get; }
        public DelegateCommand<int?> OpenModifyStudentCommand { get; }
        public DelegateCommand AddNewStudentCommand { get; }
        public DelegateCommand<int?> RemoveStudentCommand { get; }
        public DelegateCommand<int?> ShowActivitiesCommand { get; }
        public DelegateCommand<int?> IndividualStudyCommand { get; }
        private ObservableCollection<Student> _students;
        public DelegateCommand ShowAddStudentCommand { get; }
        public DelegateCommand LoadStudentsCommand { get; }
        public DelegateCommand<int?> OpenActivityCommand { get; }
        public DelegateCommand<int?> SendEmailToStudentCommand { get; }
        public DelegateCommand<int?> GiveOnePointCommand { get; }
        public DelegateCommand<int?> GiveTwoPointsCommand { get; }
        public DelegateCommand<int?> GiveThreePointsCommand { get; }

        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }
        private SubGroup _selectedGroup;

        public SubGroup SelectedGroup
        {
            get => _selectedGroup;
            set => SetProperty(ref _selectedGroup, value);
        }

        private bool _isActivityClosed;

        public bool IsActivityClosed
        {
            get => _isActivityClosed;
            set
            {
                if (value)
                {
                    LoadStudents();
                }
                SetProperty(ref _isActivityClosed, value);
            }
        }
        private Student _newStudent = new();
        public Student NewStudent
        {
            get => _newStudent;
            set => SetProperty(ref _newStudent, value);
        }
        private Student _selectedStudent = new();
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set => SetProperty(ref _selectedStudent, value);
        }
        private ObservableCollection<SubGroup> _subGroups;
        public ObservableCollection<SubGroup> SubGroups
        {
            get => _subGroups;
            set => SetProperty(ref _subGroups, value);
        }


        private ObservableCollection<Activity> _activities;
        public ObservableCollection<Activity> Activities
        {
            get => _activities;
            set => SetProperty(ref _activities, value);
        }

        private int CurrentStudentId { get; set; }
        public HomeViewModel(int currentGroupId, ref AppDbContext context)
        {
            _work = new UnitOfWork(context);
            LoadStudents();
            AppDbContext = context;
            IndividualStudyCommand = new DelegateCommand<int?>(ChangeIndividualStudy);
            ShowAddStudentCommand = new DelegateCommand(ShowAddStudent);
            AddNewStudentCommand = new DelegateCommand(AddNewStudent);
            RemoveStudentCommand = new DelegateCommand<int?>(RemoveStudent);
            OpenModifyStudentCommand = new DelegateCommand<int?>(LoadStudentToUpdate);
            ModifyStudentCommand = new DelegateCommand<int?>(UpdateStudent);
            SetSubGroupCommand = new DelegateCommand(GetSubGroup);
            LoadStudentsCommand = new DelegateCommand(LoadStudents);
            ShowActivitiesCommand = new DelegateCommand<int?>(ShowActivities);
            OpenActivityCommand = new DelegateCommand<int?>(OpenActivity);
            SubGroups = new ObservableCollection<SubGroup>(_work.SubGroup.GetAll().ToList());
            SendEmailToStudentCommand = new DelegateCommand<int?>(SendEmailToTheStudent);
            GiveOnePointCommand = new DelegateCommand<int?>(GivePoint);
            GiveTwoPointsCommand = new DelegateCommand<int?>(Give2Points);
            GiveThreePointsCommand = new DelegateCommand<int?>(Give3Points);

        }

        public void GetSubGroup()
        {
            NewStudent.SubGroup = SelectedGroup;
        }

        public void LoadStudentToUpdate(int? id)
        {

            NewStudent = Students.FirstOrDefault(x => x.Id == id);
            SelectedGroup = NewStudent.SubGroup;
            IsAddStudent = true;
            IsModifyStudent = true;
        }
        public void UpdateStudent(int? id)
        {
            if (NewStudent != null && NewStudent.Id == id.Value)
            {
                _work.Students.Update(NewStudent);
                _work.Complete();
            }
            IsAddStudent = false;
            IsModifyStudent = false;
            NewStudent = new Student();
        }
        public void RemoveStudent(int? id)
        {
            if (id.HasValue)
            {
                _work.Students.Remove(_work.Students.Get(id.Value));
                _work.Complete();

                LoadStudents();
            }
        }

        public void AddNewStudent()
        {
            try
            {
                if (StudentValidator.ValidateStudent(NewStudent))
                {
                    // Change to CurrentMainGroupID
                    NewStudent.MainGroup = _work.MainGroup.Get(CurrentMainGroupId);
                    NewStudent.SubGroup = _work.SubGroup.Get(1);
                    _work.Students.Add(NewStudent);
                    _work.Complete();
                    Students.Add(NewStudent);
                    NewStudent = new Student();
                    IsAddStudent = false;
                }
                else
                {
                    //Add error message if data are not correctly filled out
                    Application.Current.Dispatcher.Invoke(() =>
                    {

                    });
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void ShowAddStudent()
        {
            if (IsAddStudent)
            {
                IsAddStudent = false;

            }
            else
            {
                if (IsModifyStudent)
                {
                    SelectedGroup = new SubGroup();
                    NewStudent = new Student();
                    IsModifyStudent = false;
                }
                IsAddStudent = true;
            }
        }
        public void LoadStudents()
        {
            Students = new ObservableCollection<Student>(_work.Students.GetStudentsByGroup(_work.MainGroup.Get(CurrentMainGroupId)).ToList());
            IsActivityClosed = false;
        }
        public void ChangeIndividualStudy(int? studentId)
        {
            if (studentId.HasValue)
            {
                _work.Students.Update(Students.FirstOrDefault(x => x.Id == studentId));
                _work.Complete();
            }
        }

        private void ShowActivities(int? id)
        {
            CurrentStudentId = id.Value;
            SelectedStudent = new Student();
            SelectedStudent = _work.Students.Get(CurrentStudentId);
        }

        private void SendEmailToTheStudent(int? id)
        {
            OutlookService outlookService = new();
            string email = _work.Settings.GetAll().FirstOrDefault().Email;
            if (string.IsNullOrEmpty(email))
            {
                email = string.Empty;
            }
            MailAddressCollection collection = new MailAddressCollection();
            collection.Add(new MailAddress(_work.Students.Get(id.Value).SchoolEmail));
            outlookService.SendEmail(new MailAddress(email), "", collection, null, "", null, true);
        }

        private void OpenActivity(int? id)
        {
            SelectedActivityWindow saw = new SelectedActivityWindow();
            saw.DataContext = new SelectedActivityViewModel(CurrentMainGroupId, id.Value);
            var result = saw.ShowDialog();

            AppDbContext = new AppDbContext();
            _work = new UnitOfWork(AppDbContext);
            int selStudId = SelectedStudent.Id;
            LoadStudents();
            SelectedStudent = Students.FirstOrDefault(x => x.Id == selStudId);
        }

        private void GivePoint(int? id)
        {
            var act = new Activity()
            {
                Name = "Body za aktivitu",
                Student = _work.Students.Get(id.Value),
                Created = DateTime.Now,
                Tasks = new List<Models.Task>()
                    {
                         new Models.Task()
                        {
                             Name="Aktivita na hodine",
                             MaxPoints=1,
                             Points=1
                        }
                    }
            };

            _work.Activity.Add(act);
            _work.Complete();
            LoadStudents();
        }
        private void Give2Points(int? id)
        {
            var act = new Activity()
            {
                Name = "Body za aktivitu",
                Student = _work.Students.Get(id.Value),
                Created = DateTime.Now,
                Tasks = new List<Models.Task>()
                    {
                         new Models.Task()
                        {
                             Name="Aktivita na hodine",
                             MaxPoints=2,
                             Points=2
                        }
                    }
            };

            _work.Activity.Add(act);
            _work.Complete();
            LoadStudents();
        }
        private void Give3Points(int? id)
        {
            var act = new Activity()
            {
                Name = "Body za aktivitu",
                Student = _work.Students.Get(id.Value),
                Created = DateTime.Now,
                Tasks = new List<Models.Task>()
                    {
                         new Models.Task()
                        {
                             Name="Aktivita na hodine",
                             MaxPoints=3,
                             Points=3
                        }
                    }
            };

            _work.Activity.Add(act);
            _work.Complete();
            LoadStudents();
        }

    }
}
