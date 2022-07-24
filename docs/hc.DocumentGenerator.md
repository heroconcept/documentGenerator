# [hc.DocumentGenerator](#T:hc.DocumentGenerator)

```csharp
public class hc.DocumentGenerator
```
### Fields
| Access Modifier | Inference | Type | Name | Summary | 
| --- | --- | --- | --- | --- | 
| public | static | `Settings` | settings |  | 
| public | static | `List<Assembly>` | filteredAssemblies |  | 
| public | static | `List<String>` | filters |  | 
| public | static | `String[]` | customTags |  | 
| private | - | `XDocument` | xml |  | 


### Methods


```csharp
public string documentType(Type type, IDocumentBuilder builder)

```


```csharp
public boolean buildNameDescription(IDocumentBuilder out builder)

```


```csharp
public boolean buildMethods(IDocumentBuilder out builder)

```


```csharp
public boolean buildEvents(IDocumentBuilder out builder)

```


```csharp
public boolean buildFields(IDocumentBuilder out builder)

```


```csharp
public boolean buildProperties(IDocumentBuilder out builder)

```


```csharp
public boolean buildInnerNestedTypes(IDocumentBuilder out builder)

```


```csharp
public void generate(Assembly casm)

```


```csharp
public static void onGUI()

```


```csharp

[InitializeOnLoadMethodAttribute]
private static void init()

```


```csharp
private static boolean checkFilter(String element, List<String> filters)

```


```csharp
public static void createFilters()

```
