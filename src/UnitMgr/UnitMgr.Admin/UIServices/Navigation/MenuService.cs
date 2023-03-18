using Mgr.Core.Constants;
using MudBlazor;
using UnitMgr.Admin.Models.SideMenu;

namespace UnitMgr.Admin.UIServices;

public class MenuService : IMenuService
{
    private readonly List<MenuSectionModel> _features = new List<MenuSectionModel>()
    {
        new MenuSectionModel
        {
            Title = "Application",
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    Title = "Home",
                    Icon = Icons.Material.Filled.Home,
                    Href = "/"
                },
                new()
                {
                    Title = "E-Commerce",
                    Icon = Icons.Material.Filled.ShoppingCart,
                    PageStatus = PageStatus.Completed,
                    IsParent = true,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new(){
                             Title = "Products",
                             Href = "/pages/products",
                             PageStatus = PageStatus.Completed,
                        },
                        new(){
                             Title = "Documents",
                             Href = "/pages/documents",
                             PageStatus = PageStatus.Completed,
                        }
                    }
                },
                new()
                {
                    Title = "Analytics",
                    Roles=new string[]{ RoleNameConstants.Administrator, RoleNameConstants.Users },
                    Icon = Icons.Material.Filled.Analytics,
                    Href = "/analytics",
                    PageStatus = PageStatus.ComingSoon
                },
                new()
                {
                    Title = "Banking",
                    Roles=new string[]{ RoleNameConstants.Administrator,RoleNameConstants.Users },
                    Icon = Icons.Material.Filled.Money,
                    Href = "/banking",
                    PageStatus = PageStatus.ComingSoon
                },
                new()
                {
                    Title = "Booking",
                    Roles=new string[]{ RoleNameConstants.Administrator,RoleNameConstants.Users },
                    Icon = Icons.Material.Filled.CalendarToday,
                    Href = "/booking",
                    PageStatus = PageStatus.ComingSoon
                }
            }
        },

        new MenuSectionModel
        {
            Title = "MANAGEMENT",
            Roles=new string[]{ RoleNameConstants.Administrator },
            SectionItems = new List<MenuSectionItemModel>
            {
                new()
                {
                    IsParent = true,
                    Title = "Authorization",
                    Icon = Icons.Material.Filled.ManageAccounts,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {
                        new()
                        {
                            Title = "Tenant",
                            Href = "/admin/tenants",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Users",
                            Href = "/admin/usermanager",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Roles",
                            Href = "/Admin/Role",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Profile",
                            Href = "/user/profile",
                            PageStatus = PageStatus.Completed
                        }
                    }
                },
                new()
                {
                    IsParent = true,
                    Title = "System",
                    Icon = Icons.Material.Filled.Devices,
                    MenuItems = new List<MenuSectionSubItemModel>
                    {   new()
                        {
                            Title = "Picklist",
                            Href = "/system/picklist",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Audit Trails",
                            Href = "/system/audittrails",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Log",
                            Href = "/system/logs",
                            PageStatus = PageStatus.Completed
                        },
                        new()
                        {
                            Title = "Jobs",
                            Href = "/jobs",
                            PageStatus = PageStatus.ComingSoon,
                            Target="_blank"

                        }
                    }
                }

            }
        }
    };

    public IEnumerable<MenuSectionModel> Features => _features;
}