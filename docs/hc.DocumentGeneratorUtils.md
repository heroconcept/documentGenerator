# [hc.DocumentGeneratorUtils](#T:hc.DocumentGeneratorUtils)

```csharp
public static class hc.DocumentGeneratorUtils
```
### Methods


```csharp

[ExtensionAttribute]
public static string xmlFileName(Assembly asm)

```


```csharp

[ExtensionAttribute]
public static string fullName(Type t)

```


```csharp

[ExtensionAttribute]
public static xelement getDocumentation(XDocument doc, String member)

```


```csharp
public static ıenumerable<string[]> buildFieldDataset(T[] array, Func<T, String> getAccessMofidier, Func<T, String> getInference, Func<T, Type> getType, Func<T, String> finalName, Func<T, String> summary)

```


```csharp
public static ıenumerable<string[]> buildNestedDataset(T[] array, Func<T, String> getAccessModifier, Func<T, String> getValue, Func<T, String> finalName)

```


```csharp

[ExtensionAttribute]
public static string getFriendlyTypeName(Type t, Boolean isOut)

```


```csharp

[ExtensionAttribute]
public static string stripStartingWith(String s, String stripAfter)

```
