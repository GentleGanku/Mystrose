namespace Mystrose.Utilities.Classes;

public class TypeList<T> : List<T> where T : class
{

    #region Constructors
    public TypeList(params T[] items)
    {
        AddRange(items);
    }
    #endregion

    #region Fields
    public T? this[string name]
    {
        get
        {
            return Find(item => item.ToString()!.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }

    public new T? this[int index]
    {
        get
        {
            if (index < 0 || index >= Count)
            {
                return null;
            }

            return base[index];
        }
    }
    #endregion

    #region Methods: Actions
    public new bool Contains(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");
        }

        return base.Contains(item);
    }

    public bool Contains(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }

        return this[name] is not null;
    }

    public bool Contains(int index)
    {
        if (index < 0 || index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }

        return base[index] is not null;
    }
    #endregion

    #region Methods: Addition
    public new T Add(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");
        }

        base.Add(item);
        return item;
    }

    public T Add(params object[] args)
    {
        try
        {
            var type = typeof(T);
            var instance = Activator.CreateInstance(type, args);

            if (instance is not T item)
            {
                throw new InvalidCastException($"Cannot cast instance of type {type.Name} to {typeof(T).Name}.");
            }

            base.Add(item);
            return item;
        }
        catch
        {
            throw new InvalidOperationException("Failed to create an instance of the specified type. Ensure the type has a parameterless constructor or provide the necessary parameters.");
        }
    }

    public new T[] AddRange(T[] items)
    {
        if (items.Length == 0)
        {
            throw new ArgumentException("Cannot add an empty array.", nameof(items));
        }

        base.AddRange(items);
        return items;
    }
    #endregion

    #region Methods: Removal
    public new bool Remove(T item)
    {
        if (item is null)
        {
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");
        }

        return base.Remove(item);
    }

    public bool Remove(string name)
    {
        var item = this[name];
        if (item is null)
        {
            return false;
        }

        return base.Remove(item);
    }

    public bool Remove(int index)
    {
        if (index < 0 || index >= Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
        }

        var item = base[index];
        return base.Remove(item);
    }
    #endregion

    #region Methods: Clearance
    public new List<T> Clear()
    {
        if (Count == 0)
        {
            return [];
        }

        List<T> clearedItems = new(this);
        base.Clear();

        return clearedItems;
    }
    #endregion

}
