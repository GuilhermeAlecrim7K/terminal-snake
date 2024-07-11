namespace TerminalSnake
{
    internal class Snake : ISnake
    {
        private readonly I2DCanvas _canvas;
        private Direction _currentDirection = Direction.Right;
        private const char _headWhenMovingUp = '\u1050';
        private const char _headWhenMovingDown = '\u1051';
        private const char _headWhenMovingRight = '\u03ff';
        private const char _headWhenMovingLeft = '\u03fe';
        private const char _bodyWhenMovingVertically = '\u2551';
        private const char _bodyWhenMovingHorizontally = '\u2550';
        private const char _tailWhenMovingUp = '\u02c5';
        private const char _tailWhenMovingDown = '\u02c4';
        private const char _tailWhenMovingLeft = '\u02c3';
        private const char _tailWhenMovingRight = '\u02c2';
        private const char _rightTurnWhenMovingUp = '\u2554';
        private const char _rightTurnWhenMovingDown = '\u255a';
        private const char _leftTurnWhenMovingUp = '\u2557';
        private const char _leftTurnWhenMovingDown = '\u255d';
        public Snake(I2DCanvas canvas)
        {
            this._canvas = canvas;
        }

        public void Render(int initialSize)
        {
            if (initialSize < 3)
                throw new ArgumentException("Initial size can't be lower than 3.", "initialSize");
            if (initialSize >= _canvas.Width)
                throw new ArgumentException("Initial size can't be larger than the buffer width.", "initialSize");
            int canvasHorizontalMiddle = _canvas.Width / 2;
            int canvasVerticalMiddle = _canvas.Height / 2;
            int tailXPosition = canvasHorizontalMiddle - initialSize / 2;
            int headXPosition = canvasHorizontalMiddle + initialSize / 2;
            _canvas.Draw(tailXPosition, canvasVerticalMiddle, _tailWhenMovingRight);
            for (int x = tailXPosition + 1; x < headXPosition; x++)
                _canvas.Draw(x, canvasVerticalMiddle, _bodyWhenMovingHorizontally);
            _canvas.Draw(headXPosition, canvasVerticalMiddle, _headWhenMovingRight);
        }

        public void Move(Direction? direction = null)
        {
            if (direction == _currentDirection.Opposite())
                return;
            _currentDirection = direction ?? _currentDirection;
        }

    }
}
