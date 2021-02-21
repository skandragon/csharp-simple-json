# csharp-simple-json
A simple json encoder (and perhaps parser) in c#, suitable for
ancient .NET platforms like Unity, where `System.Text.Json` does't exist, and
Unity's `JsonUtil.ToJson()` doesn't do most useful things like nested objects,
lists, arrays...

This isn't intended to be a performant solution, but performance improvements are
welcome.  Note that I started learning c# about 3 days prior to coding this,
so buyer beware.

# Usage

See `main.cs` for example usage.

# Installation

This is not intended to compile to a .dll library, and then be included as
a package or reference.  You should drop the json.cs file into your project, and
compile it as part of your code base.  This is easier as a dependent .dll is hard
to include in a Unity plugin/mod due to dependency conflicts.

# TODO

* Properly handle Dictionary<T, X> types, where T must be a string (or
  converted to a string somehow) and X can be any object type, which we
  will render as JSON.
* Allow Dictionary<> types to be rendered as: `{"key1": value1, "key2": value2}` or
  `[{"Key": "key1", "Value": "value1"}, {...}]` with some sort of `JsonDictionaryEncoding`
  option.
* Allow Dictionary<> keys and value fields to be named differently than the default
  `Key` and `Value` names.

