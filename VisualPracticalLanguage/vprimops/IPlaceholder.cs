namespace VisualPracticalLanguage
{
    public interface IPlaceholder
    {
        IPlaceholderContainer parent { get; }
        bool OnDrop(DraggableControl el);
        void OnOver(DraggableControl c);
        void OnLeave(DraggableControl c);
        void ResetColor();
    }
}