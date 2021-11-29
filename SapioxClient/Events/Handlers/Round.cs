using System;

namespace SapioxClient.Events.Handlers
{
    public class Round
    {
        public static event Action Start;

        public static void OnRoundStart() => Start?.Invoke();
    }
}