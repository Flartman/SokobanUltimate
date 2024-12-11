using System;
using System.Collections.Generic;
using System.Linq;

namespace Sokoban.Models.Menu
{
    public interface IMenu : IMenuItem
    {
        public int Count { get; }

        public void Add(IMenuItem item);

        public IReadOnlyList<IMenuItem> Items { get; }

        public MenuType Type { get; }

        public void Remove(string itemName);
    }

    public enum MenuType
    {
        Main,
        Settings,
        Library,
        Profile,
        ProfileManager,
    }

    public class Menu : IMenuItem, IMenu
    {
        private readonly List<IMenuItem> _menuItems;

        public int Count => _menuItems.Count;

        public string Name { get; }

        public MenuType Type { get; }

        public Action Enter => throw new NotImplementedException();

        public Menu(string name)
        {
            Name = name;
            _menuItems = new List<IMenuItem>();
        }

        public IReadOnlyList<IMenuItem> Items => _menuItems;

        public void Add(IMenuItem item)
        {

            if (_menuItems.FindIndex(i => i.Name == Name) == -1)
            {
                _menuItems.Add(item);
            }
            else
            {
                throw new ArgumentException("Menu can't contain two items with same name");
            }
        }

        public void Remove(string itemName)
        {
            var index = _menuItems.FindIndex(i => i.Name == itemName);
            _menuItems.RemoveAt(index);
        }

        public void Clear()
        {

            //пока не нужен, мб удалить
            _menuItems?.Clear();
        }
    }


}
