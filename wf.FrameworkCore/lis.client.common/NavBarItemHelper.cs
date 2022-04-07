using DevExpress.XtraNavBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace dcl.client.common
{
    public class NavBarItemHelper
    {
        public NavBarControl Control = null;

        public void CreatItem(List<EntityDictMenu> Menus)
        {
            if (Menus == null || Menus.Count == 0|| Control == null)
                return;

            foreach (EntityDictMenu menu in Menus)
            {
                if (menu.IsRoot)//顶级节点   创建一个Group
                {
                    AddGroup(menu);
                }
                else //非顶级节点，创建一个子菜单
                {
                    //1.寻找其上级节点

                    var s = from a in Menus where a.ID == menu.ParentID select a;
                    if (s.Count() == 0)
                        continue;//找不到上级节点，无法创建
                    foreach (EntityDictMenu dic in s)
                    {
                        NavBarGroup nbg = dic.MenuGroup as NavBarGroup;
                        if (nbg == null)
                            break;
                        menu.PicName = dic.PicName;
                        AddGroupItem(nbg, menu);
                        break;
                    }

                }
            }
        }

        /// <summary>
        /// 新增子级菜单
        /// </summary>
        /// <param name="nbg"></param>
        /// <param name="menu"></param>
        private void AddGroupItem(NavBarGroup nbg, EntityDictMenu menu)
        {
            NavBarItem nbItem1 = new NavBarItem();
            nbItem1.Name = menu.ID;
            nbItem1.Caption = menu.Name;
            nbItem1.SmallImageIndex = -1;
            nbItem1.LargeImageIndex = -1;
            nbItem1.LinkClicked += menu.ClickHandler;
            //添加到导航栏所有子项目集合
            Control.Items.Add(nbItem1);

            if (menu.PicName !=null)
            {
                nbItem1.LargeImage = menu.PicName;
                nbItem1.SmallImage = menu.PicName;
            }


            //添加子项目至某一分组
            nbg.ItemLinks.AddRange(new NavBarItemLink[] {
                new NavBarItemLink(nbItem1) });
        }

        /// <summary>
        /// 新增父级菜单
        /// </summary>
        /// <param name="menu"></param>
        private void AddGroup(EntityDictMenu menu)
        {
            NavBarGroup nbGroup1 = new NavBarGroup();
            nbGroup1.Name = menu.ID;
            nbGroup1.Caption = menu.Name;
            nbGroup1.SmallImageIndex = -1;
            nbGroup1.LargeImageIndex = -1;
            menu.MenuGroup = nbGroup1;//存储上级菜单，供下级菜单绑定
            if (menu.PicName != null)
            {
                nbGroup1.LargeImage = menu.PicName;
                nbGroup1.SmallImage = menu.PicName;
            }
            Control.Groups.Add(nbGroup1);
        }
    }

    public class EntityDictMenu
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父节点ID
        /// </summary>
        public string ParentID { get; set; }

        /// <summary>
        /// 本节点是否为顶级节点
        /// </summary>
        public bool IsRoot
        {
            get
            {
                if (ID.Contains("ROOT"))
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 菜单项
        /// </summary>
        public object MenuGroup { get; set; }

        public Image PicName { get; set; }

        /// <summary>
        /// 菜单项点击事件
        /// </summary>
        public NavBarLinkEventHandler ClickHandler { get; set; }
    }
}
