namespace TrackSpendMvvm.Tests;

internal static class Util
{
	/// <summary>
	/// Use reflection to create a deep (non-referential) copy of an object and its members recursively.
	/// </summary>
	/// <typeparam name="T">The type of object to copy.</typeparam>
	/// <param name="input">The object to create a copy of.</param>
	/// <returns>A clone of <paramref name="input"/> with no shared references and all members cloned recursively.</returns>
	public static T DeepCopy<T>(T input)
		where T : class, new()
	{
		Type type = typeof(T);
		T clone = new();
		
		foreach (var property in type.GetProperties())
		{
			if (!property.CanWrite
			    || property.GetValue(input) is not { } value)
			{
				continue;
			}
			
			Type valueType = value.GetType();
			switch (valueType)
			{
				case { IsValueType: true }:
					property.SetValue(clone, value);
					break;

				case { IsClass: true }
					when value is string stringValue:
					property.SetValue(clone, stringValue);
					break;

				case { IsClass: true, FullName: { } typeName }
					when !typeName.StartsWith("System."):
					property.SetValue(clone, DeepCopy(value));
					break;

				default:
					continue;
			}
		}

		return clone;
	}
}