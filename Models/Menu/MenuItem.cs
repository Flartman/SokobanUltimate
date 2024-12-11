using System;

namespace Sokoban.Models.Menu
{
    public interface IMenuItem
    {
        public string Name { get; }

        public Action Enter { get; }
    }

    public class MenuItem : IMenuItem
    {
        public string Name { get; }

        public Action Enter { get; }

        public MenuItem(string name, Action onEnter)
        {
            Name = name;
            Enter = onEnter;
        }
    }
}
