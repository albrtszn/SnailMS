namespace SnailMS.Service
{
    public class Service
    {
        private UserService userService;
        private ManagerService managerService;
        private AdminService adminService;
        //private RoleService userService;
        //private UserRoleRepoService userService;
        private TempCallService tempCallService;
        private CallService callService;
        private NotificationService notificationService;
        //private DepartmentService userService;

        public Service(UserService _userService, ManagerService _managerService,
                       AdminService _adminService, TempCallService _tempCallService,
                       CallService _callService, NotificationService _notificationService)
        {
            userService = _userService;
            managerService = _managerService;
            adminService = _adminService;
            tempCallService = _tempCallService;
            callService = _callService;
            notificationService = _notificationService;
        }

        public UserService Users { get { return userService; } }
        public ManagerService Managers { get { return managerService; } }
        public AdminService Admin { get { return adminService; } }
        //public RoleService userService;
        //public UserRoleRepoService userService;
        public TempCallService TempCalls { get { return tempCallService; } }
        public CallService Calls { get { return callService; } }
        public NotificationService Notifications { get { return notificationService; } }
        //private DepartmentService userService;
    }
}
