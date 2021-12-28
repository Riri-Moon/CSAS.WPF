using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class Settings : BaseModelBindableBase
    {
        public virtual int A
        {
            get {  return _a; }
            set
            {
                if (value > 100 )
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    {
                        value = 0;
                    }
                }
                SetProperty(ref _a, value);

            }
        }

        private int _a;
        public virtual int B
        {
            get { return _b; }
            set
            {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    {
                        value = 0;
                    }
                }
                SetProperty(ref _b, value);
            }
        }

        private int _b;
        public virtual int C
        {
            get { return _c; }
            set
            {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    {
                        value = 0;
                    }
                }
                SetProperty(ref _c, value);
            }
        }

        private int _c;
        public virtual int D
        {
            get { return _d; }
            set
            {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    {
                        value = 0;
                    }
                }
                SetProperty(ref _d, value);
            }
        }

        private int _d;

        public virtual int E
        {
            get { return _e; }
            set
            {
                if (value > 100)
                {
                    value = 100;
                }
                else if (value < 0)
                {
                    {
                        value = 0;
                    }
                }
                SetProperty(ref _e, value);
            }
        }

        private int _e;
        private string _lastname;

        public virtual string? LastName
        {
            get => _lastname;
            set => SetProperty(ref _lastname, value);
        }
        private string _title;

        public virtual string? Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _email;

        public virtual string? Email
        {
            get => _email;
            set =>  SetProperty(ref _email, value);
        }

        private string _titleAfterName;

        public virtual string? TitleAfterName
        {
            get => _titleAfterName;
            set => SetProperty(ref _titleAfterName, value);
        }

        private string _name;
        public virtual string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}
