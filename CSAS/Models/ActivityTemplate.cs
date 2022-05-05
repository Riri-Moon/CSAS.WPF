using System.ComponentModel.DataAnnotations.Schema;

namespace CSAS.Models
{
	public class ActivityTemplate : BaseWithNameModelBindableBaseBindableBase
	{
		public virtual string Subject
		{
			get => _subject;

			set => SetProperty(ref _subject, value);
		}
		public virtual IList<TaskTemplate> TasksTemplate
		{
			get => _tasksTemplate.OrderBy(x=>x.CreateDate).ToList();
			set => SetProperty(ref _tasksTemplate, value);
		}

		public virtual DateTime Created
		{
			get => _created;
			set => SetProperty(ref _created, value);
		}

		[NotMapped]
		public bool IsUpdate
		{
			get => _isUpdate;
			set => SetProperty(ref _isUpdate, value);
		}
		public int MaxPoints
		{
			get => _maxPoints;
			set => SetProperty(ref _maxPoints, value);
		}
		public ActivityTemplate Clone()
		{
			return MemberwiseClone() as ActivityTemplate;
		}
		private int _maxPoints;
		private bool _isUpdate = false;
		private DateTime _created = DateTime.Now;
		private IList<TaskTemplate> _tasksTemplate;
		private string _subject;

		public bool Validate()
		{
			if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Subject))
			{
				return false;
			}

			return true;
		}
	}
}
