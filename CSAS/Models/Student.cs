using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static CSAS.Enums.Enums;

namespace CSAS.Models
{
    public class Student : BindableBase
    {
        [Key]
        public virtual int Id { get; set; }
        public string _name;
        public virtual string Name {
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
            get => _listOfActivities;
            set => SetProperty(ref _listOfActivities, value);
        }
        public virtual List<Attendance>? Attendances
        {
            get => _attendances;
            set => SetProperty(ref _attendances, value);
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
        [MaxLength(3)]
        public virtual int? TotalPoints
        {
            get => _totalPoints;
            set => SetProperty(ref _totalPoints, value);
        }

        private bool _individualStudy;
        public virtual bool IndividualStudy
        {
            get => _individualStudy;
            set => SetProperty(ref _individualStudy, value);
        }

        public string? _title;
        public string? _email;
        public MainGroup? _mainGroup;
        public SubGroup? _subGroup;
        public List<Activity>? _listOfActivities;
        public List<Attendance>?_attendances;
        public string _schoolEmail;
        public int? _year;
        public FormEnums _form;
        public int? _totalPoints;

    }
}
