using SudokuLib.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SudokuUWP.Models
{
    public class DispGameLevel
    {
        public GameLevel Value { get; }

        public string Text { get; }

        public static List<DispGameLevel> Values { get; }

        static DispGameLevel()
        {
            Values = Enum.GetValues(typeof(GameLevel))
                .Cast<GameLevel>()
                .Select(l => new DispGameLevel(l))
                .ToList();
        }

        private DispGameLevel(GameLevel value)
        {
            Value = value;
            Text = GetDescription(value);
        }

        public static DispGameLevel Get(GameLevel level)
        {
            return Values.First(r => r.Value == level);
        }

        public override string ToString()
        {
            return Text;
        }

        public static implicit operator DispGameLevel(GameLevel level)
        {
            return Get(level);
        }

        public static implicit operator GameLevel(DispGameLevel dispLevel)
        {
            return dispLevel.Value;
        }

        private string GetDescription(GameLevel value)
        {
            Type type = typeof(GameLevel);
            var fieldInfo = type.GetFields().FirstOrDefault(f => f.IsStatic && (GameLevel)f.GetValue(GameLevel.Easy) == value);
            return fieldInfo.GetCustomAttribute<DescriptionAttribute>().Description;
        }
    }
}
