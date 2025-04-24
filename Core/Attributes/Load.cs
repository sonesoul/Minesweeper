namespace Minesweeper.Core.Attributes
{
    public class LoadAttribute : BaseInitAttribute
    {
        public LoadAttribute(int order = 0) : base(order) { }
        public static void Invoke() => Invoke<LoadAttribute>();
    }
}
