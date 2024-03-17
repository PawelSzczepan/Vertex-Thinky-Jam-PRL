namespace UI
{
    /// <summary>Interface for story elements.</summary>
    public interface IStoryElementInfoProvider
    {
        public StoryElementInfo GetInfo();
    }

    public struct StoryElementInfo
    {
        public string Name { get; set; }
        // TODO Icon support
    }

    public class DefaultStoryElementInfoProvider : IStoryElementInfoProvider
    {
        public StoryElementInfo GetInfo()
        {
            return new StoryElementInfo { Name = "Story Element" };
        }
    }
}
