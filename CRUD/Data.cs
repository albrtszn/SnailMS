using CRUD.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD
{
    public class Data
    {
        private IUserRepo userRepo;
        private IManagerRepo managerRepo;
        private IAdminRepo adminRepo;
        private IRoleRepo roleRepo;
        private IUserRoleRepo userRoleRepo;
        private ITempCallRepo tempCallRepo;
        private ICallRepo callRepo;
        private INotificationRepo notificationRepo;
        private IDepartmentRepo departmentRepo;

        public Data(IUserRepo _userRepo, IManagerRepo _managerRepo, IAdminRepo _adminRepo,
                    IRoleRepo _roleRepo, IUserRoleRepo _userRoleRepo, ITempCallRepo _tempCall,
                    ICallRepo _callRepo, INotificationRepo _notificationRepo, IDepartmentRepo _departmentRepo)
        {
            userRepo = _userRepo;
            managerRepo = _managerRepo;
            adminRepo = _adminRepo;
            roleRepo = _roleRepo;
            userRoleRepo = _userRoleRepo;
            tempCallRepo = _tempCall;
            callRepo = _callRepo;
            notificationRepo = _notificationRepo;
            departmentRepo = _departmentRepo;
        }

        public IUserRepo Users { get { return userRepo; } }
        public IManagerRepo Managers { get { return managerRepo; } }
        public IAdminRepo Admins { get { return adminRepo; } }
        public IRoleRepo Roles { get { return roleRepo; } }
        public IUserRoleRepo UserRoles { get { return userRoleRepo; } }
        public ITempCallRepo TempCalls { get { return tempCallRepo; } }
        public ICallRepo Calls { get { return callRepo; } }
        public INotificationRepo Notifications { get { return notificationRepo; } }
        public IDepartmentRepo Departments { get { return departmentRepo; } }
    }
}
