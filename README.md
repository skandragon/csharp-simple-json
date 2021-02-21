# csharp-simple-json
What might become a simple json encoder (and perhaps parser) in c#, suitable for
ancient .NET platforms like Unity, where `System.Text.Json` does't exist, and
Unity's `JsonUtil.ToJson()` doesn't do most useful things like nested objects,
lists, arrays...

This isn't intended to be a performant solution, but performance improvements are
welcome.  Note that I started learning c# about 3 days prior to coding this,
so buyer beware.

See `main.cs` for example usage.

# TODO

* Properly handle Dictionary<T, X> types, where T must be a string (or
  converted to a string somehow) and X can be any object type, which we
  will render as JSON.
* Allow Dictionary<> types to be rendered as: `{"key1": value1, "key2": value2}` or
  `[{"Key": "key1", "Value": "value1"}, {...}]` with some sort of `JsonDictionaryEncoding`
  option.
* Allow Dictionary<> keys and value fields to be named differently than the default
  `Key` and `Value` names.

