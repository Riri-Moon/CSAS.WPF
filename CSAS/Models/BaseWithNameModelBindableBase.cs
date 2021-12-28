using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class BaseWithNameModelBindableBaseBindableBase : BaseModelBindableBase
    {
        public virtual string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _name;
    }
}
