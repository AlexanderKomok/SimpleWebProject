using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebAppTry3.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
    }
}
