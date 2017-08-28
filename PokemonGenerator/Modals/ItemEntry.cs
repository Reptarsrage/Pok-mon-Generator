﻿namespace PokemonGenerator.Modals
{
    /// <summary>
    /// A modal used to store and serialize/deserialize information.
    /// </summary>
    internal class ItemEntry
    {
        public byte Count; // 1
        public byte Index; // 1

        /// <summary>
        /// Prettry prints the ItemEntry contents
        /// </summary>
        public override string ToString()
        {
            return $"{Index} x ({Count})";
        }
    }
}