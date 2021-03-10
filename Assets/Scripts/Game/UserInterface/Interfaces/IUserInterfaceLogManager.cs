namespace RTSEngine.RTSUserInterface
{
    public interface IUserInterfaceLogManager
    {
        void AddLog(string log);
        void AddLog(string log, bool topDown);
        void Clear();
        DefaultLogText CreateLog(string log);
    }

}