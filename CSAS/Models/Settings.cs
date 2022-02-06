namespace CSAS.Models
{
	public class Settings : BaseModelBindableBase
	{
		private MainGroup _mainGroup;
		public virtual MainGroup? MainGroup
		{
			get => _mainGroup;
			set => SetProperty(ref _mainGroup, value);
		}

		private double? _maxPoints =100;
		public virtual double? MaxPoints
		{
			get => _maxPoints;
			set => SetProperty(ref _maxPoints, value);
		}
		public virtual int A
		{
			get { return _a; }
			set
			{
				value = IsValid(value);
				SetProperty(ref _a, value);
			}
		}

		private int _a = 94;
		public virtual int B
		{
			get { return _b; }
			set
			{
				value = IsValid(value);
				SetProperty(ref _b, value);
			}
		}

		private int _b = 87;
		public virtual int C
		{
			get { return _c; }
			set
			{
				value = IsValid(value);
				SetProperty(ref _c, value);
			}
		}
		private int _c = 80;

		public virtual int D
		{
			get { return _d; }
			set
			{
				value = IsValid(value);
				SetProperty(ref _d, value);
			}
		}

		private int _d = 63;

		public virtual int E
		{
			get { return _e; }
			set
			{
				value = IsValid(value);
				SetProperty(ref _e, value);
			}
		}

		private int _e = 56;
		private string? _lastname;

		public virtual string? LastName
		{
			get => _lastname;
			set => SetProperty(ref _lastname, value);
		}
		private string? _title;

		public virtual string? Title
		{
			get => _title;
			set => SetProperty(ref _title, value);
		}

		private string? _email;

		public virtual string? Email
		{
			get => _email;
			set => SetProperty(ref _email, value);
		}

		private string? _titleAfterName;

		public virtual string? TitleAfterName
		{
			get => _titleAfterName;
			set => SetProperty(ref _titleAfterName, value);
		}

		private string? _name;
		public virtual string? Name
		{
			get => _name;
			set => SetProperty(ref _name, value);
		}

		private string? _signature = "";
		public virtual string? Signature
		{
			get => _signature;
			set => SetProperty(ref _signature, value);
		}


		private int IsValid(int value)
		{
			switch (value)
			{
				case > 100:
					value = 100;
					break;
				case < 0:
					value = 0;
					break;
			}
			return value;
		}
	}
}
