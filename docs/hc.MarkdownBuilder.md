# [hc.MarkdownBuilder](#T:hc.MarkdownBuilder)

```csharp
public class hc.MarkdownBuilder
```
### Fields
| Access Modifier | Inference | Type | Name | Summary | 
| --- | --- | --- | --- | --- | 
| public | - | `StringBuilder` | stringBuilder |  | 


### Methods


```csharp
public static string markdownCodeQuote(String code)

```


```csharp
public void append(String text)

```


```csharp
public void appendLine()

```


```csharp
public void appendLine(String text)

```


```csharp
public void header(Int32 level, String text)

```


```csharp
public void headerWithCode(Int32 level, String code)

```


```csharp
public void headerWithLink(Int32 level, String text, String url)

```


```csharp
public void link(String text, String url)

```


```csharp
public void image(String altText, String imageUrl)

```


```csharp
public void code(String language, String code)

```


```csharp
public void codeQuote(String code)

```


```csharp
public void table(String[] headers, IEnumerable<String[]> items)

```


```csharp
public void list(String text)

```


```csharp
public void listLink(String text, String url)

```


```csharp
public void quote(String text)

```


```csharp
public virtualstring ToString()

```
