using DangerZone.Models;

namespace DangerZone.EventModels
{
    public class StartGameEvent
    {
        public StartGameEvent(Zone zone)
            => Zone = zone;

        public Zone Zone { get; }
    }
}
