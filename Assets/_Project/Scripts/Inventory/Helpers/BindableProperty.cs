using System;
using Unity.Properties;

/// <summary>
/// Represents a bindable property.
/// </summary>
/// <typeparam name="T">The type of the property value.</typeparam>
public class BindableProperty<T> {
    readonly Func<T> getter;
    // Add setter to make a 2 way binding Action<T> setter;

    BindableProperty(Func<T> getter) {
        this.getter = getter;
    }
    
    [CreateProperty] // Allows binding to the UI in UI Toolkit
    public T Value => getter();
    
    public static BindableProperty<T> Bind(Func<T> getter) => new BindableProperty<T>(getter);
}
