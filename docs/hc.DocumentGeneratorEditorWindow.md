# [hc.DocumentGeneratorEditorWindow](#T:hc.DocumentGeneratorEditorWindow)

```csharp
public class hc.DocumentGeneratorEditorWindow : EditorWindow
```
### Fields
| Access Modifier | Inference | Type | Name | Summary | 
| --- | --- | --- | --- | --- | 
| private | - | `ReorderableList` | listView |  | 
| private | - | `SerializedProperty` | serializedFilters |  | 
| private | - | `Vector2` | scrollPos |  | 
| private | - | `SerializedObject` | serializedObject |  | 
| private | - | `ListView` | obj |  | 


### Properties
>
```csharp
public VisualElement rootVisualElement {} 

```
>
```csharp
public SerializableJsonDictionary viewDataDictionary {} 

```
>
```csharp
public Boolean wantsMouseMove {} 

```
>
```csharp
public Boolean wantsMouseEnterLeaveWindow {} 

```
>
```csharp
public Boolean wantsLessLayoutEvents {} 

```
>
```csharp
public Boolean autoRepaintOnSceneChange {} 

```
>
```csharp
public Boolean maximized {} 

```
>
```csharp
public Boolean hasFocus {} 

```
>
```csharp
public Boolean docked {} 

```
>
```csharp
public Boolean disableInputEvents {} 

```
>
```csharp
public Boolean hasUnsavedChanges {} 

```
>
```csharp
public String saveChangesMessage {} 

```
>
```csharp
public Vector2 minSize {} 

```
>
```csharp
public Vector2 maxSize {} 

```
>
```csharp
public String title {} 

```
>
```csharp
public GUIContent titleContent {} 

```
>
```csharp
public Int32 depthBufferBits {} 

```
>
```csharp
public Int32 antiAliasing {} 

```
>
```csharp
public Int32 antiAlias {} 

```
>
```csharp
public Rect position {} 

```
>
```csharp
public String name {} 

```
>
```csharp
public HideFlags hideFlags {} 

```
### Methods


```csharp

[MenuItem]
public static void ShowWindow()

```


```csharp
private void OnEnable()

```


```csharp
public void OnGUI()

```
