using PlanningApi.ViewModel;
using Repositories.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlanningApi.Helpers
{
    public class ModelToVmMapper
    {
        public static User BuidUserFromRegisterVm(User vm)
        {
            vm.DateOfBirth = Convert.ToDateTime(vm.DateOfBirth);
            return vm;
        }
    }
}
