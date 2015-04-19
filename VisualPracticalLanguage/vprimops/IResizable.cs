namespace VisualPracticalLanguage
{
    public interface IResizable
    {
        IResizable ResizableParent { get; }
        void UpdateSize();
    }
}