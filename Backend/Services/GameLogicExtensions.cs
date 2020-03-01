using Domain;
using Domain.Pieces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Services
{
    public static class GameLogicExtensions
    {
        public static IChessPiece PieceAtPosition(this IList<IChessPiece> pieces, Position position)
        {
            return pieces.FirstOrDefault(piece => piece.Position.Equals(position) && !piece.IsTaken);
        }

        public static King King(this IList<IChessPiece> pieces)
        {
            foreach (var playerPiece in pieces)
            {
                if (playerPiece is King king)
                {
                    return king;
                }
            }

            return null;
        }

        public static bool IsInBounds(this Position pos)
        {
            return (0 <= pos.X && pos.X < GameConstants.NrRows) &&
                   (0 <= pos.Y && pos.Y < GameConstants.NrColumns);
        }

        public static Transition GetTransitionToDestination(this IChessPiece piece, Position destination)
        {
            foreach (var pieceTransition in piece.AvailableTransitions)
            {
                var transition = pieceTransition;

                if (transition.IsOneTimeOnly)
                {
                    if ((piece.Position + transition).Equals(destination))
                    {
                        return transition;
                    }
                }
                else
                {
                    var nextPosition = piece.Position + transition;

                    while (nextPosition.IsInBounds())
                    {
                        if (nextPosition.Equals(destination))
                        {
                            return transition;
                        }

                        nextPosition += transition;
                    }
                }
            }

            return null;
        }

        public static List<Position> GetPathToDestination(this IChessPiece piece, Position destination)
        {
            var result = new List<Position>();

            var transition = piece.GetTransitionToDestination(destination);
            if (transition == null)
            {
                return null;
            }

            var nextPosition = piece.Position + transition;

            while (!nextPosition.Equals(destination))
            {
                result.Add(nextPosition);
                nextPosition += transition;
            }

            return result;
        }

        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            // Don't serialize a null object, simply return the default for that object
            if (source == null)
            {
                return default;
            }

            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
