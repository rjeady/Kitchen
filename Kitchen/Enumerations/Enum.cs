using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kitchen.Enumerations
{
	/// <summary>
	/// An enhanced, named enumeration class. All members must be declared as public static read-only fields to the implementing class.
	/// The name (identifier) of each member may be obtained as a string.
	/// </summary>
	/// <typeparam name="T">The implementing class, derived from <cref>Enum</cref>.</typeparam>
	public abstract class Enum<T>
		where T : Enum<T>
	{
		private static readonly Dictionary<string, T> _members = new Dictionary<string, T>();

		private static bool _gotMembers;
		private static object membersLock = new object();

		/// <summary>
		/// A read-only collection containing all the members of the enum.
		/// </summary>
		public static IEnumerable<T> Members
		{
			get
			{
				GetMembers();
				return _members.Values;
			}
		}

		private static void GetMembers()
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
								_members.Add(field.Name, member);
							}
						}

						_gotMembers = true;
					}
				}
			}
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
			_members.Add(name, member);
		}

		private string _name;

		/// <summary>
		/// The member's name, which is its identifier.
		/// Beware that the identifier, and so this property, will be changed if an obfuscator is used.
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
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return Name;
		}

		/// <summary>
		/// Performs an explicit conversion from <see cref="System.String"/> to Enum{T}.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>The result of the conversion.</returns>
		/// <exception cref="System.InvalidCastException">Thrown when no member of this enum has the specified name</exception>
		public static explicit operator Enum<T>(string name)
		{
			GetMembers();
			try
			{
				return _members[name];
			}
			catch (KeyNotFoundException ex)
			{
				throw new InvalidCastException("No member of this enum has the specified name", ex);
			}
		}

		public static bool TryParse(string name, out T member)
		{
			GetMembers();
			return _members.TryGetValue(name, out member);
		}

		public static T ParseOrDefault(string name, T defaultValue)
		{
			T member;
			TryParse(name, out member);
			return member ?? defaultValue;
		}
	}
}