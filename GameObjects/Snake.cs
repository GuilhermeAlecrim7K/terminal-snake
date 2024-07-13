using TerminalSnake.Canvas;
using TerminalSnake.Extensions;
using TerminalSnake.GameObjects.Interfaces;

namespace TerminalSnake.GameObjects
{
    internal class Snake : ISnake
    {
        private readonly LinkedList<Point> _coordinates = [];
        private readonly I2DCanvas _canvas;
        private readonly char _foodChar;
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
        public Snake(I2DCanvas canvas, char foodChar)
        {
            _canvas = canvas;
            _foodChar = foodChar;
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

        public bool TryMove(Direction? direction)
        {
            // Trying to move in the opposite direction yields no result and no error
            if (direction != _currentDirection.Opposite())
                _currentDirection = direction ?? _currentDirection;

            Point newHead = _nextCoordinate();
            bool collided = Collided(newHead);
            if (!collided)
            {
                RedrawTail(newHead);
                RedrawHead(newHead);
            }
            return !collided;
        }

        private Point _nextCoordinate()
        {
            int newX, newY;
            (newX, newY) = (_coordinates.First!.Value.X, _coordinates.First!.Value.Y);
            switch (_currentDirection)
            {
                case Direction.Up:
                    newY--;
                    break;
                case Direction.Down:
                    newY++;
                    break;
                case Direction.Right:
                    newX++;
                    break;
                case Direction.Left:
                    newX--;
                    break;
                default:
                    throw new NotImplementedException();
            }
            return new(newX, newY);
        }

        private void RedrawHead(Point newHead)
        {
            Point newNeck = _coordinates.First!.Value;
            Point currentNeck = _coordinates.First!.Next!.Value;
            char neckChar = _nextNeckChar(currentNeck, newHead);
            char headChar = _currentDirection switch
            {
                Direction.Up => _headWhenMovingUp,
                Direction.Down => _headWhenMovingDown,
                Direction.Left => _headWhenMovingLeft,
                Direction.Right => _headWhenMovingRight,
                _ => throw new NotImplementedException(),
            };
            _canvas.Draw(newNeck.X, newNeck.Y, neckChar);
            _coordinates.AddFirst(newHead);
            _canvas.Draw(newHead.X, newHead.Y, headChar);
        }

        private char _nextNeckChar(Point currentNeck, Point newHead)
        {
            (int x, int y) = (newHead.X - currentNeck.X, newHead.Y - currentNeck.Y);
            return (x, y, _currentDirection) switch
            {
                (2, 0, _) or (-2, 0, _) => _bodyWhenMovingHorizontally,
                (0, 2, _) or (0, -2, _) => _bodyWhenMovingVertically,
                (1, -1, Direction.Up) or (-1, 1, Direction.Left) => _leftTurnWhenMovingDown,
                (1, 1, Direction.Down) or (-1, -1, Direction.Left) => _leftTurnWhenMovingUp,
                (-1, -1, Direction.Up) or (1, 1, Direction.Right) => _rightTurnWhenMovingDown,
                (-1, 1, Direction.Down) or (1, -1, Direction.Right) => _rightTurnWhenMovingUp,
                _ => '#',
            };
        }

        private void RedrawTail(Point newHead)
        {
            if (_canvas.TryGetPixelAt(newHead.X, newHead.Y, out object? pixel) && pixel is char pixelChar && pixelChar == _foodChar)
                return;

            Point currentTail = _coordinates.Last!.Value;
            Point newTail = _coordinates.Last!.Previous!.Value;
            _canvas.Draw(_coordinates.Last!.Value.X, _coordinates.Last!.Value.Y, null);
            _coordinates.RemoveLast();
            _canvas.Draw(_coordinates.Last!.Value.X, _coordinates.Last!.Value.Y, _nextTailChar(currentTail, newTail));
        }

        private char _nextTailChar(Point currentTail, Point newTail)
        {
            return (currentTail.X - newTail.X, currentTail.Y - newTail.Y) switch
            {
                (1, 0) => _tailWhenMovingLeft,
                (-1, 0) => _tailWhenMovingRight,
                (0, 1) => _tailWhenMovingUp,
                (0, -1) => _tailWhenMovingDown,
                _ => throw new NotImplementedException("There was an error on rendering the tail."),
            };
        }

        private bool Collided(Point newHead)
        {
            return
                newHead.X >= _canvas.Width
                || newHead.X <= 0
                || newHead.Y >= _canvas.Height
                || newHead.Y <= 0
                || (_canvas.TryGetPixelAt(newHead.X, newHead.Y, out object? pixel)
                    && pixel is char pixelChar && pixelChar != _foodChar &&
                    newHead != _coordinates.Last!.Value);
        }

    }
}
