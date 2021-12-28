using CSAS.Models;
using CSAS.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.ViewModels
{
    public class ExportViewModel : BaseViewModelBindableBase
    {

        private bool _isAnonymized = new();
        public bool IsAnonymized
        {
            get => _isAnonymized;
            set => SetProperty(ref _isAnonymized, value);
        }

        private bool _isExcel =true;
        public bool IsExcel
        {
            get => _isExcel;
            set => SetProperty(ref _isExcel, value);
        }
        private bool _isAll = true;
        public bool IsAll
        {
            get => _isAll;
            set => SetProperty(ref _isAll, value);
        }
        private bool _isGroup;
        public bool IsGroup
        {
            get => _isGroup;
            set => SetProperty(ref _isGroup, value);
        }
        public bool IsStudent
        {
            get => _isStudent;
            set => SetProperty(ref _isStudent, value);
        }
        private bool _isStudent;
        public bool IsActivity
        {
            get => _isActivity;
            set => SetProperty(ref _isActivity, value);
        }
        private bool _isActivity = true;
        public bool IsAttendance
        {
            get => _isAttendance;
            set => SetProperty(ref _isAttendance, value);
        }
        private bool _isAttendance;
        public bool IsAssessment
        {
            get => _isAssessment;
            set => SetProperty(ref _isAssessment, value);
        }
        private bool _isSendMe;
        public bool IsSendMe
        {
            get => _isSendMe;
            set => SetProperty(ref _isSendMe, value);
        }
        private bool _isSendToStudents;
        public bool IsSendToStudents
        {
            get => _isSendToStudents;
            set => SetProperty(ref _isSendToStudents, value);
        }
        private bool _isAssessment;

        private SubGroup _selectedGroup;
        public SubGroup SelectedGroup
        {
            get => _selectedGroup;
            set => SetProperty(ref _selectedGroup, value);
        }

        private ObservableCollection<SubGroup> _groups = new();
        public ObservableCollection<SubGroup> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }
        private UnitOfWork _work { get; set; }

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        private Student _selectedStudent = new();
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set => SetProperty(ref _selectedStudent, value);
        }

        public ExportViewModel(int currentGroupId, ref AppDbContext appDbContext)
        {
            _work = new UnitOfWork(appDbContext);
            Students = new ObservableCollection<Student>(_work.Students.GetAll().Where(x => x.MainGroup.Id == currentGroupId));
            Groups = new ObservableCollection<SubGroup>(_work.SubGroup.GetAll().Where(g => g.MainGroup.Id == currentGroupId));
        }
    }
}
