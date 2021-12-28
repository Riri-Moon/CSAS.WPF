using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class BaseModelBindableBase : BindableBase
    {
        [Key]
        public virtual int Id {  get; set; }
    }
}
