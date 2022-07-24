# [hc.DocumentStructureGenerator](#T:hc.DocumentStructureGenerator)

```csharp
public class hc.DocumentStructureGenerator
```
### Fields
| Access Modifier | Inference | Type | Name | Summary | 
| --- | --- | --- | --- | --- | 
| private | static | `BindingFlags` | flags |  | 
| private | - | `XDocument` | xml |  | 


### Methods


```csharp
public structure buildNew(Type type)

```


```csharp
private void buildBodyAndDescriptions(Type type, out StructureInfo out structureInfo)

```


```csharp
private void buildMethods(Type type, out List<MethodInfo> methodStructs)

```


```csharp
private string buildMethodSummary(MethodInfo m, Type type, Boolean hasParameters, out Dictionary<ParameterInfo, String> parameters, out Dictionary<String, String> customTagVariables)

```


```csharp
private void buildProperties(Type type, XDocument xml, out Dictionary<PropertyInfo, String> propInfos)

```


```csharp
private void buildEnums(Type type, XDocument xml, out List<EnumInfo> enums)

```


```csharp
private dictionary<string, type> buildInnerNestedTypes(Type t)

```


```csharp
public fieldınfo[] buildFields(Type type)

```
