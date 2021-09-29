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

namespace CSAS.ViewModels
{
    public class HomeViewModel : BaseViewModel
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
        public DelegateCommand<int?> ModifyStudentCommand { get; }
        public DelegateCommand<int?> OpenModifyStudentCommand { get; }
        public DelegateCommand AddNewStudentCommand { get; }
        public DelegateCommand<int?> RemoveStudentCommand { get; }
        public DelegateCommand<int?> IndividualStudyCommand { get; }
        private ObservableCollection<Student> _students;
        public DelegateCommand ShowAddStudentCommand { get; }
        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set
            {
                SetProperty(ref _students, value);
            }
        }
        private Student _newStudent = new Student();
        public Student NewStudent
        {
            get { return _newStudent; }
            set
            {
                SetProperty(ref _newStudent, value);
            }
            //get { return _newStudent; }
            //set
            //{
            //    _newStudent = value;
            //    OnPropertyChanged(nameof(_newStudent));
            //}
        }
        public UnitOfWork _unitOfWork { get; set; }
        public HomeViewModel()
        {
            _unitOfWork = new UnitOfWork(new AppDbContext());
            LoadStudents();

            IndividualStudyCommand = new DelegateCommand<int?>(ChangeIndividualStudy);
            ShowAddStudentCommand = new DelegateCommand(ShowAddStudent);
            AddNewStudentCommand = new DelegateCommand(AddNewStudent);
            RemoveStudentCommand = new DelegateCommand<int?>(RemoveStudent);
            OpenModifyStudentCommand = new DelegateCommand<int?>(LoadStudentToUpdate);
            ModifyStudentCommand = new DelegateCommand<int?>(UpdateStudent);

        }

        public void LoadStudentToUpdate(int? id)
        {
            ShowAddStudent();
            NewStudent = Students.FirstOrDefault(x => x.Id == id);
        }
        public void UpdateStudent(int? id)
        {
            if (NewStudent != null && NewStudent.Id == id.Value)
            {
                _unitOfWork.Students.Update(NewStudent);
                _unitOfWork.Complete();
            }
        }
        public void RemoveStudent(int? id)
        {
            if (id.HasValue)
                _unitOfWork.Students.Remove(_unitOfWork.Students.Get(id.Value));
            _unitOfWork.Complete();

            LoadStudents();
        }

        public void AddNewStudent()
        {
            try
            {
                if (StudentValidator.ValidateStudent(NewStudent))
                {
                    // Change to CurrentMainGroupID
                    NewStudent.MainGroup = _unitOfWork.MainGroup.Get(1);
                    _unitOfWork.Students.Add(NewStudent);
                    _unitOfWork.Complete();
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
                IsAddStudent = true;
            }
        }
        public void LoadStudents()
        {
            Students = new ObservableCollection<Student>(_unitOfWork.Students.GetAll().ToList());
        }
        public void ChangeIndividualStudy(int? studentId)
        {
            if (studentId.HasValue)
            {
                _unitOfWork.Students.Update(Students.FirstOrDefault(x => x.Id == studentId));
                _unitOfWork.Complete();
            }
        }
    }
}
