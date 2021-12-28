using BenchmarkDotNet.Attributes;
using CSAS.Interfaces;
using Microsoft.VisualStudio.PlatformUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSAS.Models
{
    public class Activity : BaseWithNameModelBindableBaseBindableBase
    {
        public virtual Student Student
        {
            get => _student;
            set => SetProperty(ref _student, value);
        }
        public virtual double? TotalPoints
        {
            get
            {
                double totalPts = 0;
                foreach (var x in Tasks)
                {
                    if(x.MaxPoints.HasValue)
                    totalPts += x.MaxPoints.Value;
                }

                return totalPts;
            }

        }
        public virtual IList<Task> Tasks
        {
            get => _tasks;
            set => SetProperty(ref _tasks, value);
        }
        public virtual DateTime Created
        {
            get => _created;
            set => SetProperty(ref _created, value);
        }

        public virtual bool IsSendEmail
        {
            get => _isSendEmail;
            set => SetProperty(ref _isSendEmail, value);
        }

        public virtual bool IsSendNotifications
        {
            get => _isSendNotifications;
            set => SetProperty(ref _isSendNotifications, value);
        }
        public virtual bool IsNotifyMe
        {
            get => _isNotifyMe;
            set => SetProperty(ref _isNotifyMe, value);
        }
        public virtual List<Attachments>? Attachments
        {
            get => _attachments;
            set => SetProperty(ref _attachments, value);
        }
        [DependsOnProperty("Tasks")]
        public virtual double? EarnedPoints
        {
            get
            {
                double totalPts = 0;
                foreach (var x in Tasks)
                {
                    if (x.Points.HasValue)
                        totalPts += x.Points.Value;
                }

                return totalPts;
            }
        }

        public Activity Clone()
        {
            return MemberwiseClone() as Activity;
        }
        private List<Attachments> _attachments;
        private bool _isNotifyMe;
        private bool _isSendEmail;
        private bool _isSendNotifications;
        private DateTime _created;
        private IList<Task> _tasks;
        private Student _student;
    }
}

