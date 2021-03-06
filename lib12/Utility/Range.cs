﻿using System;
using lib12.Checking;

namespace lib12.Utility
{
    /// <summary>
    /// Location in range
    /// </summary>
    public enum LocationInRange
    {
        /// <summary>
        /// The less than start
        /// </summary>
        LessThanStart,
        /// <summary>
        /// The on start
        /// </summary>
        OnStart,
        /// <summary>
        /// The in range
        /// </summary>
        InRange,
        /// <summary>
        /// The on end
        /// </summary>
        OnEnd,
        /// <summary>
        /// The greater than end
        /// </summary>
        GreaterThanEnd
    }

    /// <summary>
    /// Utility for operations on Ranges
    /// </summary>
    /// <typeparam name="T">The type of Range</typeparam>
    public struct Range<T> where T : IComparable
    {
        /// <summary>
        /// The range start
        /// </summary>
        public T Start;
        /// <summary>
        /// The range end
        /// </summary>
        public T End;

        /// <summary>
        /// Initializes a new instance of the <see cref="Range{T}"/> struct. You don't need to pass borders in correct order.
        /// </summary>
        /// <param name="a">One border</param>
        /// <param name="b">Other border</param>
        public Range(T a, T b)
        {
            if (a.CompareTo(b) < 0)
            {
                Start = a;
                End = b;
            }
            else
            {
                Start = b;
                End = a;
            }
        }

        /// <summary>
        /// Checks where given object is on current range
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns></returns>
        public LocationInRange LocateInRange(T item)
        {
            if (item.CompareTo(Start) < 0)
                return LocationInRange.LessThanStart;
            else if (item.CompareTo(Start) == 0)
                return LocationInRange.OnStart;
            else if (item.CompareTo(End) < 0)
                return LocationInRange.InRange;
            else if (item.CompareTo(End) == 0)
                return LocationInRange.OnEnd;
            else
                return LocationInRange.GreaterThanEnd;
        }

        /// <summary>
        /// Checks whether given object is inside of range
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <param name="inclusiveStartAndEnd">if set to <c>true</c> [inclusive start and end].</param>
        /// <returns></returns>
        public bool InRange(T item, bool inclusiveStartAndEnd = true)
        {
            if (inclusiveStartAndEnd)
                return LocateInRange(item).IsAnyOf(LocationInRange.OnStart, LocationInRange.InRange, LocationInRange.OnEnd);
            else
                return LocateInRange(item).IsAnyOf(LocationInRange.InRange);
        }

        /// <summary>
        /// Checks whether given object is outside of range
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <param name="inclusiveStartAndEnd">if set to <c>true</c> [inclusive start and end].</param>
        /// <returns></returns>
        public bool OutsideOfRange(T item, bool inclusiveStartAndEnd = true)
        {
            return !InRange(item, inclusiveStartAndEnd);
        }

        /// <summary>
        /// Checks whether given object is on the left side of range
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns></returns>
        public bool LessThanStart(T item)
        {
            return LocateInRange(item) == LocationInRange.LessThanStart;
        }

        /// <summary>
        /// Checks whether given object is on the right side of range
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns></returns>
        public bool GreaterThanEnd(T item)
        {
            return LocateInRange(item) == LocationInRange.GreaterThanEnd;
        }
    }
}