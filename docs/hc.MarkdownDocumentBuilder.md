# [hc.MarkdownDocumentBuilder](#T:hc.MarkdownDocumentBuilder)

```csharp
public class hc.MarkdownDocumentBuilder IDocumentBuilder
```
### Fields
| Access Modifier | Inference | Type | Name | Summary | 
| --- | --- | --- | --- | --- | 
| public | - | `MarkdownBuilder` | markdownBuilder |  | 
| private | - | `Structure` | structure |  | 


### Methods


```csharp
public finalstructure getStructure()

```


```csharp
public finalvoid setStructure(Structure str)

```


```csharp
public finalstring getDocumentContext()

```


```csharp
public finalstring getFileExtention()

```


```csharp
public finalvoid buildStructureNameAndDescriptions()

```


```csharp
public finalvoid buildMethods()

```


```csharp
public finalvoid buildEvents()

```


```csharp
public finalvoid buildFields()

```


```csharp
public finalvoid buildProperties()

```


```csharp
public finalvoid buildInnerNestedTypes()

```


```csharp
public void buildEnums(List<EnumInfo> enumInfos)

```


```csharp
public void buildNestedTypeLinkList(Dictionary<String, Type> nestedList)

```
