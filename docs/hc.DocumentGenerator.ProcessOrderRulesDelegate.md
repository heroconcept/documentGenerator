# [hc.DocumentGenerator.ProcessOrderRulesDelegate](#T:hc.DocumentGenerator.ProcessOrderRulesDelegate)

```csharp
public class hc.DocumentGenerator+ProcessOrderRulesDelegate : MulticastDelegate, ICloneable, ISerializable
```
### Properties
>
```csharp
public Boolean HasSingleTarget {} 

```
>
```csharp
public MethodInfo Method {} 

```
>
```csharp
public Object Target {} 

```
### Methods


```csharp
public virtualboolean Invoke(IDocumentBuilder out builder)

```


```csharp
public virtualıasyncresult BeginInvoke(IDocumentBuilder out builder, AsyncCallback callback, Object object)

```


```csharp
public virtualboolean EndInvoke(IDocumentBuilder out builder, IAsyncResult result)

```
