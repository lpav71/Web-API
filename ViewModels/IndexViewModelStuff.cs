using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.ViewModels;

namespace WebAPI.Models
{
    public class IndexViewModelStuff
    {
        public IQueryable<Stuff> Stuff { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}
