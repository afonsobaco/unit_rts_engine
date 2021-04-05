using RTSEngine.Core;

namespace RTSEngine.Integration.Scene
{

    public class UIUpdatePartySignal
    {
        public IntegrationSceneObject[] Selection { get; set; }
        public int PartyId { get; internal set; }
    }

}