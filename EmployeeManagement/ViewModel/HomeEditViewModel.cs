using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModel
{
    public class HomeEditViewModel : HomeCreateViewModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
