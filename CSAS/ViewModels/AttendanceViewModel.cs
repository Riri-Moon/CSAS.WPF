using CSAS.Models;
using CSAS.Repositories;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSAS.Enums.Enums;

namespace CSAS.ViewModels
{
    public class AttendanceViewModel : BaseViewModelBindableBase
    {
        public DelegateCommand SelectLectureCommand { get; }
        public DelegateCommand<int?> SelectSeminarCommand { get; }
        public DelegateCommand<int?> SelectAttendanceCommand { get; }
        public DelegateCommand AddNewAttendanceCommand { get; }
        public DelegateCommand<int?> RemoveAttendanceCommand { get; }
        public DelegateCommand<int?> StudentNotPresentCommand { get; }
        public DelegateCommand<int?> StudentPresentCommand { get; }
        public DelegateCommand<int?> StudentSickCommand { get; }

        private ObservableCollection<Attendance> _lectures;
        public ObservableCollection<Attendance> Lectures
        {
            get { return _lectures; }
            set
            {
                SetProperty(ref _lectures, value);
            }
        }

        private ObservableCollection<Attendance> _seminars;
        public ObservableCollection<Attendance> Seminars
        {
            get { return _seminars; }
            set
            {
                SetProperty(ref _seminars, value);
            }
        }

        private Attendance _selectedAttendance;
        public Attendance SelectedAttendance
        {
            get { return _selectedAttendance; }
            set
            {
                SetProperty(ref _selectedAttendance, value);
            }
        }

        private ObservableCollection<SubGroup> _subGroups;
        public ObservableCollection<SubGroup> SubGroups
        {
            get { return _subGroups; }
            set
            {
                SetProperty(ref _subGroups, value);
            }
        }

        private SubGroup _selectedSubGroup;

        public SubGroup SelectedSubGroup
        {
            get { return _selectedSubGroup; }
            set
            {
                SetProperty(ref _selectedSubGroup, value);
            }
        }
        private ObservableCollection<Attendance> _attendances;
        public ObservableCollection<Attendance> Attendances
        {
            get { return _attendances; }
            set
            {
                SetProperty(ref _attendances, value);
            }
        }

        private bool _isLectureSelected = true;
        public bool IsLectureSelected
        {
            get { return _isLectureSelected; }
            set
            {
                SetProperty(ref _isLectureSelected, value);
            }
        }

        private bool _isAttendanceSelected;
        public bool IsAttendanceSelected
        {
            get { return _isAttendanceSelected; }
            set
            {
                SetProperty(ref _isAttendanceSelected, value);
            }
        }

        private string _numberOfStudents;
        public string NumberOfStudents
        {
            get { return _numberOfStudents; }
            set
            {
                SetProperty(ref _numberOfStudents, value);
            }
        }
        private UnitOfWork _work { get; set; }

        public AttendanceViewModel(int currentMainGroupId, ref AppDbContext context)
        {
            _work = new UnitOfWork(context);
            Attendances = new ObservableCollection<Attendance>();

            var att = _work.Attendance.GetAttendanceByMainGroup(_work.MainGroup.Get(currentMainGroupId));
            Lectures = new ObservableCollection<Attendance>(att.Where(x => x.Form == AttendanceFormEnums.Lecture));
            Seminars = new ObservableCollection<Attendance>(att.Where(x => x.Form == AttendanceFormEnums.Seminar));
            Attendances = Lectures;
            SubGroups = new ObservableCollection<SubGroup>(_work.MainGroup.Get(currentMainGroupId).SubGroups);
            SelectedSubGroup = SubGroups.FirstOrDefault();
            SelectLectureCommand = new DelegateCommand(SelectLecture);
            SelectSeminarCommand = new DelegateCommand<int?>(SelectSeminar);
            AddNewAttendanceCommand = new DelegateCommand(AddNewAttendance);
            RemoveAttendanceCommand = new DelegateCommand<int?>(RemoveAttendance);
            SelectAttendanceCommand = new DelegateCommand<int?>(SelectAttendance);
            StudentPresentCommand = new DelegateCommand<int?>(StudentPresent);
            StudentNotPresentCommand = new DelegateCommand<int?>(StudentNotPresent);
            StudentSickCommand = new DelegateCommand<int?>(StudentSick);

        }

        private void SelectAttendance(int? id)
        {
            if (id.HasValue)
            {
                IsAttendanceSelected = true;

                SelectedAttendance = _work.Attendance.Get(id.Value);
            }
            else
            {
                IsAttendanceSelected = false;
            }
        }

        private void SelectLecture()
        {
            Attendances = new ObservableCollection<Attendance>(Lectures.OrderByDescending(x => x.Date.Date).ToList());
            IsLectureSelected = true;
        }
        private void SelectSeminar(int? id)
        {
            if (id.HasValue)
            {
                SelectedSubGroup = _work.SubGroup.Get(id.Value);
                Attendances = new ObservableCollection<Attendance>(_work.Attendance.GetAttendanceBySubGroup(SelectedSubGroup).OrderByDescending(x => x.Date));
            }
            else if (SelectedSubGroup != null)
            {
                Attendances = new ObservableCollection<Attendance>(_work.Attendance.GetAttendanceBySubGroup(SelectedSubGroup).OrderByDescending(x => x.Date));
            }
            else
            {
                Attendances = new ObservableCollection<Attendance>();
            }

            IsLectureSelected = false;
        }
        private void ResetAttendance()
        {
            var temp = SelectedAttendance;

            SelectedAttendance = new Attendance();
            SelectedAttendance = temp;
        }
        private void StudentPresent(int? id)
        {
            SelectedAttendance.SubAttendances.FirstOrDefault(x => x.Id == id.Value).State = AttendanceEnums.IsPresent;
            _work.Attendance.Update(SelectedAttendance);
            _work.Complete();
            ResetAttendance();

        }
        private void StudentNotPresent(int? id)
        {
            SelectedAttendance.SubAttendances.FirstOrDefault(x => x.Id == id.Value).State = AttendanceEnums.NotPresent;
            _work.Attendance.Update(SelectedAttendance);
            _work.Complete();
            ResetAttendance();
        }
        private void StudentSick(int? id)
        {
            SelectedAttendance.SubAttendances.FirstOrDefault(x => x.Id == id.Value).State = AttendanceEnums.Excused;
            _work.Attendance.Update(SelectedAttendance);
            _work.Complete();
            ResetAttendance();

        }
        private void AddNewAttendance()
        {
            MainGroup mainGroup = _work.MainGroup.Get(CurrentMainGroupId);
            if (SelectedSubGroup != null)
            {
                SubGroup subGroup = _work.SubGroup.Get(SelectedSubGroup.Id);
            }

            var form = IsLectureSelected ? AttendanceFormEnums.Lecture : AttendanceFormEnums.Seminar;

            IEnumerable<Student> students;
            Attendance attendance;
            if (!IsLectureSelected)
            {
                students = _work.Students.GetStudentsBySubGroup(SelectedSubGroup);
                attendance = new Attendance()
                {
                    Date = DateTime.Now,
                    Form = form,
                    MainGroup = mainGroup,
                    StudyForm = mainGroup.Form,
                    SubGroup = SelectedSubGroup,
                };
            }
            else
            {
                students = _work.Students.GetStudentsByGroup(_work.MainGroup.Get(CurrentMainGroupId));
                attendance = new Attendance()
                {
                    Date = DateTime.Now,
                    Form = form,
                    MainGroup = mainGroup,
                    StudyForm = mainGroup.Form
                };
            }

            foreach (var student in students)
            {
                if (attendance.SubAttendances == null)
                {
                    attendance.SubAttendances = new List<SubAttendances>();
                }
                attendance.SubAttendances.Add(
                    new SubAttendances()
                    {
                        Student = new Student(),
                        State = AttendanceEnums.New
                    });

                attendance.SubAttendances.Last().Student = student;
            }

            var cultureInfo = new CultureInfo("sk-SK");
            var dateTimeInfo = cultureInfo.DateTimeFormat;
            attendance.Day = dateTimeInfo.GetDayName(attendance.Date.DayOfWeek).ToUpper();

            if (IsLectureSelected)
            {
                Lectures = Attendances;
            }
            else
            {
                Seminars = Attendances;
            }

            _work.Attendance.Add(attendance);
            _work.Complete();
            Attendances.Add(attendance);
        }

        private void RemoveAttendance(int? id)
        {
            _work.Attendance.Get(id.Value).SubAttendances.RemoveAt(0);
            _work.Attendance.Remove(_work.Attendance.Get(id.Value));
            _work.Complete();
            Attendances.Remove(Attendances.FirstOrDefault(x => x.Id == id.Value));
            if (IsLectureSelected)
            {
                Lectures = Attendances;
            }
            else
            {
                Seminars = Attendances;
            }
        }
    }
}
