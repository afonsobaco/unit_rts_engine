using RTSEngine.RTSUserInterface;

namespace RTSEngine.Integration.Scene
{
    public class UIPartyContentInfo : UIContentInfo
    {
        private int _key;
        private IntegrationSceneObject[] selection; 
        public int Key { get => _key; set => _key = value; }
        public IntegrationSceneObject[] Selection { get => selection; set => selection = value; }
    }
}