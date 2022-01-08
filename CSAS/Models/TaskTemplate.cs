using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSAS.Models
{
	public class TaskTemplate : BindableBase
	{
		public int Id { get; set; }
		public virtual ActivityTemplate ActivityTemplate
		{
			get { return _activityTemplate; }
			set
			{
				SetProperty(ref _activityTemplate, value);
			}
		}
		public virtual string Name
		{
			get { return _name; }
			set
			{
				SetProperty(ref _name, value);
			}
		}
		[MaxLength(3), MinLength(1)]
		public virtual int? MaxPoints
		{
			get { return _maxPoints; }
			set
			{
				SetProperty(ref _maxPoints, value);
			}
		}

		[NotMapped]
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				SetProperty(ref _isSelected, value);
			}
		}
		public TaskTemplate Clone()
		{

			return MemberwiseClone() as TaskTemplate;
		}

		private string _name;
		private int? _maxPoints;
		private ActivityTemplate? _activityTemplate;
		private bool _isSelected;
	}
}
