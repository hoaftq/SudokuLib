using System.ComponentModel;

namespace SodokuLib.Game
{
    public enum GameLevel
    {
        Easy,
        Medium,
        Hard,
        [Description("Very hard")]
        VeryHard
    }
}
