namespace Sokoban.Models
{
    public interface IElement
    {

    }

    public abstract class CollisionElement : IElement
    {

    }

    public abstract class GroundElement : IElement
    {

    }

    public class Player : CollisionElement, IElement
    {

    }

    public class Wall : CollisionElement, IElement
    {

    }

    public class Box : CollisionElement, IElement
    {

    }

    public class Empty : CollisionElement, IElement
    {
        
    }
    public class Ground : GroundElement, IElement
    {

    }

    public class EndPoint : GroundElement, IElement
    {

    }
}
