using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.Models
{
    public class BaseModel
    {
        [Key]
        public virtual int Id {  get; set; }
    }
}
