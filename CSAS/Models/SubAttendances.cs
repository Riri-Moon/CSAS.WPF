using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSAS.Enums.Enums;

namespace CSAS.Models
{
    public class SubAttendances : BindableBase
    {
        [Key]
        public virtual int Id { get; set; }

        private Attendance _attendance;
        public virtual Attendance Attendance
        {
            get => _attendance;
            set => SetProperty(ref _attendance, value);
        }

        private Student _student;
        public virtual Student Student
        {
            get => _student;
            set => SetProperty(ref _student, value);
        }
        public virtual AttendanceEnums State 
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }
        private AttendanceEnums _state;
    }
}
