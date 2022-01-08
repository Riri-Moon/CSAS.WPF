using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace CSAS.Models
{
	public class Task : BindableBase
	{
		public int Id { get; set; }
		public virtual Activity Activity
		{
			get { return _activity; }
			set
			{
				SetProperty(ref _activity, value);
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
		public virtual double? MaxPoints
		{
			get { return _maxPoints; }
			set
			{
				SetProperty(ref _maxPoints, value);
			}
		}
		[MaxLength(3), MinLength(1)]
		public virtual double? Points
		{
			get
			{

				if (_points == null)
				{
					return 0;
				}
				return _points;
			}
			set
			{
				if (value > MaxPoints.Value)
				{
					SetProperty(ref _points, MaxPoints.Value);

				}
				else if (value < 0)
				{
					SetProperty(ref _points, 0);

				}
				else
				{
					SetProperty(ref _points, value);
				}
			}
		}
		public virtual string? Comment
		{
			get { return _comment; }
			set
			{
				SetProperty(ref _comment, value);
			}
		}
		private string _name;
		private double? _maxPoints;
		private Activity? _activity;
		private string _comment;
		private double? _points = 0;

		public bool Validate()
		{
			if (string.IsNullOrEmpty(Name))
			{
				return false;
			}

			return true;
		}
		public Task Clone()
		{
			return MemberwiseClone() as Task;
		}
	}
}
