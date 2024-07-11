namespace TerminalSnake
{
    internal class Snake : ISnake
    {
        private readonly LinkedList<Point> _coordinates = [];
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
            Point tail = new(canvasHorizontalMiddle - initialSize / 2, canvasVerticalMiddle);
            Point head = new(canvasHorizontalMiddle + initialSize / 2, canvasVerticalMiddle);
            _canvas.Draw(tail.X, tail.Y, _tailWhenMovingRight);
            for (int x = tail.X + 1; x < head.X; x++)
            {
                _canvas.Draw(x, canvasVerticalMiddle, _bodyWhenMovingHorizontally);
                _coordinates.AddFirst(new Point(x, canvasVerticalMiddle));
            }
            _canvas.Draw(head.X, head.Y, _headWhenMovingRight);
            _coordinates.AddFirst(head);
            _coordinates.AddLast(tail);
        }

        public void Move(Direction? direction)
        {
            if (direction == _currentDirection.Opposite())
                return;
            _currentDirection = direction ?? _currentDirection;

            _canvas.Draw(_coordinates.Last!.Value.X, _coordinates.Last!.Value.Y, null);
            _coordinates.RemoveLast();
            _canvas.Draw(_coordinates.Last!.Value.X, _coordinates.Last!.Value.Y, _tailWhenMovingRight);
            _canvas.Draw(_coordinates.First!.Value.X, _coordinates.First!.Value.Y, _bodyWhenMovingHorizontally);
            _coordinates.AddFirst(new Point(_coordinates.First!.Value.X + 1, _coordinates.First!.Value.Y));
            _canvas.Draw(_coordinates.First!.Value.X, _coordinates.First!.Value.Y, _headWhenMovingRight);
            if (_coordinates.First!.Value.X >= _canvas.Width - 1)
                throw new NotImplementedException("Hit the wall");
        }

    }
}
