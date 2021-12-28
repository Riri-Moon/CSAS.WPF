using CSAS.Models;
using CSAS.Repositories;
using CSAS.Services;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Process = System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.ViewModels
{
    public class ActivityViewModel : BaseViewModelBindableBase
    {
        public DelegateCommand RefreshCommand { get; }
        public DelegateCommand<int?> SelectTemplateCommand { get; }
        public DelegateCommand CreateActivityCommand { get; }
        public DelegateCommand SelectAttachmentsCommand { get; }
        public DelegateCommand<string> RemoveAttachmentCommand { get; }
        public DelegateCommand<string> OpenAttachmentCommand { get; }


        private bool _isSelectAll;
        public bool IsSelectAll
        {
            get => _isSelectAll;
            set => SetProperty(ref _isSelectAll, value);
        }
        private bool _isSelectGroup;
        public bool IsSelectGroup
        {
            get => _isSelectGroup;
            set => SetProperty(ref _isSelectGroup, value);
        }

        private bool _isSelectIndividual;
        public bool IsSelectIndividual
        {
            get => _isSelectIndividual;
            set => SetProperty(ref _isSelectIndividual, value);
        }

        private UnitOfWork _work { get; set; }

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
        private ObservableCollection<Attachments> _attachments = new();
        public ObservableCollection<Attachments> Attachments
        {
            get => _attachments;
            set => SetProperty(ref _attachments, value);
        }

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get => _students;
            set => SetProperty(ref _students, value);
        }

        private ObservableCollection<ActivityTemplate> _templates;
        public ObservableCollection<ActivityTemplate> Templates
        {
            get => _templates;
            set => SetProperty(ref _templates, value);
        }

        private Activity _activity;
        public Activity Activity
        {
            get => _activity;
            set => SetProperty(ref _activity, value);

        }

        private ActivityTemplate _activityTemplate;
        public ActivityTemplate ActivityTemplate
        {
            get => _activityTemplate;
            set => SetProperty(ref _activityTemplate, value);

        }

        private DateTime _date;
        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);

        }

        private Student _student = new();
        public Student Student 
        {
            get => _student;
            set => SetProperty(ref _student, value);

        }

        public ActivityViewModel(int currentGroupId, ref AppDbContext context)
        {
            _work = new UnitOfWork(context);

            Students = new ObservableCollection<Student>(_work.Students.GetAll().Where(x => x.MainGroup.Id == currentGroupId));
            Groups = new ObservableCollection<SubGroup>(_work.SubGroup.GetAll().Where(g => g.MainGroup.Id == currentGroupId));
            RefreshTemplates();
            Activity = new Activity();
            Activity.Created = DateTime.Now;
            Activity.Attachments = new List<Attachments>();

            RefreshCommand = new (RefreshTemplates);
            SelectTemplateCommand = new DelegateCommand<int?>(SelectActivityTemplate);
            CreateActivityCommand = new (CreateActivity);
            SelectAttachmentsCommand = new(SelectAttachments);
            RemoveAttachmentCommand = new DelegateCommand<string>(RemoveAttachment);
            OpenAttachmentCommand = new DelegateCommand<string>(OpenAttachment);

        }

        private void RefreshTemplates()
        {
            Templates = new ObservableCollection<ActivityTemplate>(_work.ActivityTemplate.GetAll().ToList());
        }

        private void SelectActivityTemplate(int? id)
        {
            ActivityTemplate = Templates.FirstOrDefault(x => x.Id == id.Value);
            Activity.Name = ActivityTemplate.Name;
            Activity.Tasks = GetTasksFromTemplate(ActivityTemplate,Activity);
        }
        // Add send email
        private void CreateActivity()
        {
            List<Activity> activity = new();
            MailAddressCollection mailAddresses = new();
            MailAddressCollection ccMailAdresses = new();
            if(Activity.Attachments==null)
            {
                Activity.Attachments = new();
            }
            Activity.Attachments.AddRange(Attachments);

            string subject = $"Nová úloha - {Activity.Name}";
            string body = $"Dobrý deň, <br/><br/> práve Vám bola pridelená nová úloha s názvom {Activity.Name}. <br/> Dátum odovzdania je { Activity.Created.ToShortDateString() } {Activity.Created.ToShortTimeString()}." +
                $"<br/><br/> V prípade akýchkoľvek otázok ma neváhajte kontaktovať. <br/><br/> S pozdravom RB";

            foreach (var student in GetStudents())
            {
                Activity act = new();
                    act = Activity.Clone();
                act.Student = student;
                act.Tasks = new List<Models.Task>();

                foreach(var task in Activity.Tasks)
                {
                    act.Tasks.Add(task.Clone());
                }
                activity.Add(act);
                mailAddresses.Add(student.SchoolEmail);
                ccMailAdresses.Add(student.Email);

            }
            _work.Activity.AddRange(activity);
            _work.Complete();


            List<Attachment> attachments = new List<Attachment>();

            if (Activity.IsSendEmail)
            {
                OutlookService outlookService = new();

                var isFinished = outlookService.SendEmail(new MailAddress("r.baricic1@gmail.com"), subject, mailAddresses, ccMailAdresses, body, Activity.Attachments.Select(x=>x.PathToFile).ToList(), true);

                if (isFinished)
                {
                    var x = 1;
                }
            }

            Activity = new();
            Activity.Created = DateTime.Now;
            ActivityTemplate = new();
        }

        private List<Student> GetStudents()
        {
            List<Student> students = new();
            if (IsSelectAll)
            {
                return Students.ToList();
            }
            else if (IsSelectGroup)
            {
                return Students.Where(x => x.SubGroup == SelectedGroup).ToList();
            }
            else
            {
                students.Add(Students.FirstOrDefault(x => x == Student));
                return students;
            }
        }

        private List<Models.Task> GetTasksFromTemplate(ActivityTemplate activityTemplate, Activity act)
        {
            List<Models.Task> tasks = new();

            foreach(var tskTemp in activityTemplate.TasksTemplate)
            {
                var task = new Models.Task();
                task.Activity = act;
                task.Name = tskTemp.Name;
                task.MaxPoints = tskTemp.MaxPoints;
                tasks.Add(task);
            }
            return tasks;
        }

        private void SelectAttachments()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Multiselect = true;
            if( openFileDialog.ShowDialog() == true) 
            {
                foreach(var file in openFileDialog.FileNames)
                {
                    Attachments att = new();
                    att.Activity = Activity;
                    att.PathToFile = file;
                    Attachments.Add(att);
                }                
            }
        }
        private void RemoveAttachment(string path)
        {
            Attachments.Remove(Attachments.Where(x=>x.PathToFile==path).FirstOrDefault());
        }
        private void OpenAttachment(string path)
        {
           var p = new Process.Process();
            p.StartInfo = new Process.ProcessStartInfo(path)
            {
                UseShellExecute = true
            };
            p.Start();
        }
    }
}


