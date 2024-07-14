namespace TerminalSnake
{
    internal partial class TerminalSnakeGame
    {
        private const int MinPlayableWidth = 20;
        private const int MinPlayableHeight = 20;
        private const int HorizontalSpeed = 45;
        private const int VerticalSpeed = 60;
        private enum Action
        {
            DoNothing,
            GoUp,
            GoDown,
            GoLeft,
            GoRight,
            Quit,
        }
        private const string MainMenuTitle =
"""
 _____                   _             _   _____             _
|_   _|                 (_)           | | /  ___|           | |
  | | ___ _ __ _ __ ___  _ _ __   __ _| | \ `--. _ __   __ _| | _____
  | |/ _ \ '__| '_ ` _ \| | '_ \ / _` | |  `--. \ '_ \ / _` | |/ / _ \
  | |  __/ |  | | | | | | | | | | (_| | | /\__/ / | | | (_| |   <  __/
  \_/\___|_|  |_| |_| |_|_|_| |_|\__,_|_| \____/|_| |_|\__,_|_|\_\___|
""";
    }
}