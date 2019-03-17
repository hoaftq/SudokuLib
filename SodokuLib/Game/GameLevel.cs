// Sodoku library
// Write by Trac Quang Hoa, 03/2019

using System.ComponentModel;

namespace SudokuLib.Game
{
    public enum GameLevel
    {
        [Description("Easy")]
        Easy,

        [Description("Medium")]
        Medium,

        [Description("Hard")]
        Hard,

        [Description("Very hard")]
        VeryHard
    }
}
