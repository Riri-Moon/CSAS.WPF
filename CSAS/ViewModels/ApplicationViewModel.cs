using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSAS.ViewModels
{
    public class ApplicationViewModel
    {
        public HomeViewModel HomeViewModel {  get; set; } = new HomeViewModel();


        public void SetCurrentGroup(int id)
        {
            HomeViewModel.CurrentMainGroupId = id;
        }
    }
}
