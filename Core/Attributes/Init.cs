namespace Minesweeper.Core.Attributes
{
    public class InitAttribute : BaseInitAttribute
    {
        public InitAttribute(int order = 0) : base(order) { }
        public static void Invoke() => Invoke<InitAttribute>();
    }
}
