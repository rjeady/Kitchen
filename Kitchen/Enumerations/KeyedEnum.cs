using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kitchen.Enumerations
{
    /// <summary>
    /// A keyed enum class. Implementations must specify the key type, and may include any other required properties.
    /// </summary>
    /// <remarks>
    /// All members must be added as static read-only fields to the implementing class.
    /// </remarks>
    /// <typeparam name="TKey">The enum key type. Built-in types are strongly recommended.</typeparam>
    /// <typeparam name="T">The implementing class, derived from Enum.</typeparam>
    public abstract class KeyedEnum<TKey, T> : IEquatable<KeyedEnum<TKey, T>> where T : KeyedEnum<TKey, T>
    {
        private static readonly Dictionary<TKey, T> _members = new Dictionary<TKey, T>();

        private static bool _gotMembers;
        private static readonly object membersLock = new object();

        private static readonly bool _keyIsString;

        protected KeyedEnum(TKey key)
        {
            Key = key;
        }

        static KeyedEnum()
        {
            if (typeof(string) == typeof(TKey))
                _keyIsString = true;
        }

        /// <summary>
        /// A read-only collection containing all the members of the enum.
        /// </summary>
        public static ICollection<T> Members
        {
            get { return GetMembers().Values; }
        }

        private static Dictionary<TKey, T> GetMembers()
        {
            // first check avoids hitting the lock upon every call.
            if (!_gotMembers)
            {
                lock (membersLock)
                {
                    if (!_gotMembers)
                    {
                        foreach (var field in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.DeclaredOnly))
                        {
                            var member = field.GetValue(null) as T;
                            if (member != null)
                            {
                                member.Name = field.Name;
                                _members.Add(member.Key, member);
                            }
                        }

                        _gotMembers = true;
                    }
                }
            }

            return _members;
        }

        /// <summary>
        /// Adds a member to the enum that was not declared in code.
        /// Can be used to load members from an external source at runtime.
        /// Note that members cannot be removed at runtime.
        /// </summary>
        /// <param name="name">The name (identifier) of the member.</param>
        /// <param name="member">The member.</param>
        protected static void AddMember(string name, T member)
        {
            GetMembers();
            member.Name = name;
            _members.Add(member.Key, member);
        }

        private string _name;

        /// <summary>
        /// The name of this member. Can also be obtained by casting the member to a string, if the key is not already a string.
        /// </summary>
        public string Name
        {
            get
            {
                GetMembers();
                return _name;
            }
            private set { _name = value; }
        }

        /// <summary>
        /// The key of this member. The key may also be obtained by explicitly casting the member to the key type.
        /// </summary>
        public TKey Key { get; private set; }

        public static explicit operator TKey(KeyedEnum<TKey, T> enumMember)
        {
            return enumMember.Key;
        }

        public override string ToString()
        {
            return _keyIsString ? Key.ToString() : Name;
        }

        public static explicit operator KeyedEnum<TKey, T>(TKey key)
        {
            GetMembers();
            try
            {
                return _members[key];
            }
            catch (KeyNotFoundException ex)
            {
                throw new InvalidCastException("No member of this enum has the specified key", ex);
            }
        }

        public static bool TryParse(TKey key, out T member)
        {
            GetMembers();
            return _members.TryGetValue(key, out member);
        }

        public static T ParseOrDefault(TKey key, T defaultValue)
        {
            T member;
            TryParse(key, out member);
            return member ?? defaultValue;
        }

        public bool Equals(KeyedEnum<TKey, T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<TKey>.Default.Equals(Key, other.Key);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((KeyedEnum<TKey, T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TKey>.Default.GetHashCode(Key);
        }
    }
}