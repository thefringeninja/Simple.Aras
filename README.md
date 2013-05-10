Simple.Aras
===========

Super simple adapter for Aras Innovator inspired by Mark Rendle's Simple.Data

Without Simple.Aras:
```var innovator = GetInnovator(serverConnection);
var result = innovator.applyMethod("my_InnovatorMethod", String.Format("<p1>{0}</p1><p2>{1}</p2><p3>{2}</p3>", "value", 123, Guid.NewGuid().ToString("N"));

if (result.isError()) {
 // recovery?
}
```

...which frankly is just a little bit weird.

With Simple.Aras:

```var innovator = ArasInnovator.Open(serverConnection);
Item result = innovator.Methods.my_InnovatorMethod(p1: "value", p2: 123, p3:Guid.NewGuid());
```

True, didn't really save any lines of code. But at least it actually looks like c#.

If there's an error returned we'll throw an exception.

As an added bonus, if your aras method returns embedded xml, we'll deserialize that for you:

```MyDTO result = innovator.Methods.get_my_DTO(...);
```

As we use XmlSerializer you will probably have to put the XML* attributes on your DTO.
