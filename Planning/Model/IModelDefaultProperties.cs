using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Model
{
    interface IModelDefaultProperties
    {
        DateTime? Created { get; set; }
        DateTime? Modified { get; set; }
    }
}
