using TerminalSnake.Canvas;
using TerminalSnake.Extensions;
using TerminalSnake.GameObjects.Interfaces;

namespace TerminalSnake.GameObjects
{
    internal class Snake(I2DCanvas canvas, char foodChar) : ISnake
    {
        private readonly LinkedList<Point> _coordinates = [];
        private readonly I2DCanvas _canvas = canvas;
        private readonly char _foodChar = foodChar;
        private Direction _currentDirection = Direction.Right;
        private const char HeadWhenMovingUp = '\u1050';
        private const char HeadWhenMovingDown = '\u1051';
        private const char HeadWhenMovingRight = '\u03ff';
        private const char HeadWhenMovingLeft = '\u03fe';
        private const char BodyWhenMovingVertically = '\u2551';
        private const char BodyWhenMovingHorizontally = '\u2550';
        private const char TailWhenMovingUp = '\u02c5';
        private const char TailWhenMovingDown = '\u02c4';
        private const char TailWhenMovingLeft = '\u02c3';
        private const char TailWhenMovingRight = '\u02c2';
        private const char RightTurnWhenMovingUp = '\u2554';
        private const char RightTurnWhenMovingDown = '\u255a';
        private const char LeftTurnWhenMovingUp = '\u2557';
        private const char LeftTurnWhenMovingDown = '\u255d';

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
            _canvas.Draw(tail.X, tail.Y, TailWhenMovingRight);
            for (int x = tail.X + 1; x < head.X; x++)
            {
                _canvas.Draw(x, canvasVerticalMiddle, BodyWhenMovingHorizontally);
                _coordinates.AddFirst(new Point(x, canvasVerticalMiddle));
            }
            _canvas.Draw(head.X, head.Y, HeadWhenMovingRight);
            _coordinates.AddFirst(head);
            _coordinates.AddLast(tail);
        }

        public bool TryMove(Direction? direction)
        {
            // Trying to move in the opposite direction yields no result and no error
            if (direction != _currentDirection.Opposite())
                _currentDirection = direction ?? _currentDirection;

            Point newHead = NextCoordinate();
            bool collided = Collided(newHead);
            if (!collided)
            {
                RedrawTail(newHead);
                RedrawHead(newHead);
            }
            return !collided;
        }

        private Point NextCoordinate()
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
            char neckChar = NextNeckChar(currentNeck, newHead);
            char headChar = _currentDirection switch
            {
                Direction.Up => HeadWhenMovingUp,
                Direction.Down => HeadWhenMovingDown,
                Direction.Left => HeadWhenMovingLeft,
                Direction.Right => HeadWhenMovingRight,
                _ => throw new NotImplementedException(),
            };
            _canvas.Draw(newNeck.X, newNeck.Y, neckChar);
            _coordinates.AddFirst(newHead);
            _canvas.Draw(newHead.X, newHead.Y, headChar);
        }

        private char NextNeckChar(Point currentNeck, Point newHead)
        {
            (int x, int y) = (newHead.X - currentNeck.X, newHead.Y - currentNeck.Y);
            return (x, y, _currentDirection) switch
            {
                (2, 0, _) or (-2, 0, _) => BodyWhenMovingHorizontally,
                (0, 2, _) or (0, -2, _) => BodyWhenMovingVertically,
                (1, -1, Direction.Up) or (-1, 1, Direction.Left) => LeftTurnWhenMovingDown,
                (1, 1, Direction.Down) or (-1, -1, Direction.Left) => LeftTurnWhenMovingUp,
                (-1, -1, Direction.Up) or (1, 1, Direction.Right) => RightTurnWhenMovingDown,
                (-1, 1, Direction.Down) or (1, -1, Direction.Right) => RightTurnWhenMovingUp,
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
            _canvas.Draw(_coordinates.Last!.Value.X, _coordinates.Last!.Value.Y, NextTailChar(currentTail, newTail));
        }

        private static char NextTailChar(Point currentTail, Point newTail)
        {
            return (currentTail.X - newTail.X, currentTail.Y - newTail.Y) switch
            {
                (1, 0) => TailWhenMovingLeft,
                (-1, 0) => TailWhenMovingRight,
                (0, 1) => TailWhenMovingUp,
                (0, -1) => TailWhenMovingDown,
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
