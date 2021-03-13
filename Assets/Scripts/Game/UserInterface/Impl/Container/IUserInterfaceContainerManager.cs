using System.Collections.Generic;

namespace RTSEngine.RTSUserInterface
{
    public interface IUserInterfaceContainerManager
    {
        void AddContent(UserInterfaceContent content);
        void RemoveContent(UserInterfaceContentComponent component);
        List<UserInterfaceContentComponent> GetAllContentComponents();
    }
}
