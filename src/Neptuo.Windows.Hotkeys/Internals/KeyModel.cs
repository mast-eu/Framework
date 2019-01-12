﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Neptuo.Windows.HotKeys.Internals
{
    /// <summary>
    /// An internal key registration model.
    /// </summary>
    internal class KeyModel : IEquatable<KeyModel>
    {
        public ModifierKeys Modifier { get; }
        public Key Key { get; }

        public KeyModel(ModifierKeys modifier, Key key)
        {
            Key = key;
            Modifier = modifier;
        }

        public bool Equals(KeyModel other)
        {
            if (ReferenceEquals(null, other))
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return Modifier.Equals(other.Modifier) && Key.Equals(other.Key);
        }

        public override bool Equals(object obj)
        {
            KeyModel other = obj as KeyModel;
            if (other == null)
                return false;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Modifier.GetHashCode() * 397) ^ Key.GetHashCode();
            }
        }

        public override string ToString()
            => String.Format("({0} + {1})", Modifier, Key);
    }
}
