using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSAS.Enums.Enums;

namespace CSAS.Models
{
    public class FinalAssessment : BaseModelBindableBase
    {
        public virtual string Comment
        {
            get => _comment;

            set => SetProperty(ref _comment, value);
        }
        private string _comment;

        public virtual Grade Grade
        {
            get => _grade;

            set => SetProperty(ref _grade, value);
        }
        private Grade _grade;
        public virtual Student Student
        {
            get => _student;

            set => SetProperty(ref _student, value);
        }
        private Student _student;

        public virtual DateTime Created
        {
            get => _created;

            set => SetProperty(ref _created, value);
        }
        private DateTime _created;
        public virtual bool IsSendEmail
        {
            get => _isSendEmail;

            set => SetProperty(ref _isSendEmail, value);
        }
        private bool _isSendEmail;
        public virtual bool IsSendExport
        {
            get => _isSendExport;

            set => SetProperty(ref _isSendExport, value);
        }
        private bool _isSendExport;

        public virtual bool IsSendAttendanceExport
        {
            get => _isSendAttendanceExport;

            set => SetProperty(ref _isSendAttendanceExport, value);
        }
        private bool _isSendAttendanceExport;

        private bool _isNew = true;
        public virtual bool IsNew
        {
            get => _isNew;

            set => SetProperty(ref _isNew, value);
        }
    }
}
