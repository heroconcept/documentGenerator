/* ----------------------------------------------------------------------- *

    * DocumentStructureGenerator.cs.cs

    ----------------------------------------------------------------------

    Copyright (C) 2022, HERO CONCEPT YAZILIM VE BİLİŞİM ANONİM ŞİRKETİ
    All Rights Reserved.

    THIS SOFTWARE IS PROVIDED 'AS-IS', WITHOUT ANY EXPRESS
    OR IMPLIED WARRANTY. IN NO EVENT WILL THE AUTHOR(S) BE HELD LIABLE FOR
    ANY DAMAGES ARISING FROM THE USE OR DISTRIBUTION OF THIS SOFTWARE

    Hero Concept <info@heroconcept.com>

* ------------------------------------------------------------------------ */
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using UnityEditor.Compilation;
using System.Runtime.CompilerServices;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace hc {

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class DocumentStructureGenerator
{
  // -------------------------------------------------------------------------
  public DocumentStructureGenerator(XDocument xml){
    this.xml = xml;
  }

  // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  [Serializable]
  public class Structure {
    public StructureInfo info;
    public List<MethodInfo> methodStructs;
    public EventInfo[] eventInfos;
    public Dictionary<PropertyInfo, string> properties;
    public Dictionary<string, Type> innerNestedTypes;
    public List<EnumInfo> enumInfos;
    public FieldInfo[] fieldInfos;
  }

  // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Method data style
  /// </summary>
  public struct MethodInfo {
    public Dictionary<ParameterInfo, string> parameters;
    public string description;
    public string name;
    public List<string> attributes;
    public Dictionary<string, string> customTagVariables;
    
  }

  // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Field Info data style
  /// </summary>
  public struct FieldInfo {
    public string accessModifier;
    public string inference;
    public Type type;
    public string name;
    public string description; 
  }

  /// <summary>
  /// Class struct to return
  /// </summary>
  public struct StructureInfo {
    public string description;
    public string body;
    public Type type;
  }

  /// <summary>
  /// 
  /// </summary>
  public struct EnumInfo {
    public Type enumType;
    public System.Array enumMembers;
    public string enumDescription;
  }

  /// <summary>
  /// binding flags
  /// </summary>
  const BindingFlags  flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
  
  XDocument xml;

  // -------------------------------------------------------------------------  
  public Structure buildNew(Type type)
  {
    Structure structure = new Structure();
    buildBodyAndDescriptions(type, out StructureInfo structureInfo);
    structure.info = structureInfo;
    structure.innerNestedTypes = buildInnerNestedTypes(type);  
    buildEnums(type, xml, out List<EnumInfo> enums);
    structure.enumInfos = enums; 
    buildProperties(type, xml, out Dictionary<PropertyInfo, string> propInfos);
    structure.properties = propInfos;
    structure.eventInfos = type.GetEvents(flags);
    structure.fieldInfos = buildFields(type);
    buildMethods(type, out List<MethodInfo> methodStructs);
    structure.methodStructs = methodStructs;
    return structure;
  }


  // -------------------------------------------------------------------------
  /// <summary>
  /// Builds classes and their descriptions
  /// </summary>
  /// <param name="xml">xdocument</param>
  /// <param name="type">class type</param>
  /// <param name="mb">markdown builder</param>
  /// <param name="classSummary">out class summary</param>
  /// <param name="classBody">out class body</param>
  private void buildBodyAndDescriptions(Type type, out StructureInfo structureInfo)
  {
    structureInfo = new StructureInfo();
    structureInfo.type = type;
    structureInfo.description = xml.getDocumentation("T:" + type.fullName())?.XPathSelectElement("summary")?.Value.Trim() ?? "";

    var body = new StringBuilder();

    var stat = (type.IsAbstract && type.IsSealed) ? "static " : "";
    var abst = (type.IsAbstract && !type.IsInterface && !type.IsSealed) ? "abstract " : ""; 
    var classOrStructOrEnumOrInterface = type.IsInterface ? "interface" : type.IsEnum ? "enum" : type.IsValueType ? "struct" : "class";
    var interfaces = type.GetInterfaces().ToList();
    var baseClass = type.BaseType;

    var interfacesStringBuilder = new StringBuilder();

    foreach(var i in interfaces) { 
      if(i != interfaces.First()) interfacesStringBuilder.Append(", ");
      interfacesStringBuilder.Append(i.Name);
    }

    body.Append($"public {stat}{abst}{classOrStructOrEnumOrInterface} {type}");

    if(type.BaseType != null && type.BaseType != typeof(System.Object)) {  
      body.Append($" : {baseClass.Name}");
      if(interfaces.Count >0) body.Append(",");   
    }

    if(interfaces.Count > 0) {
      body.Append($" {interfacesStringBuilder}"); 
    }

    structureInfo.body = body.ToString(); 
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// Builds methods 
  /// </summary>
  /// <param name="xml">xdocument</param>
  /// <param name="type">class type</param>
  /// <param name="mb">markdown builder</param>
  /// <param name="methodStructs">method structs</param>
  private void buildMethods(Type type, out List<MethodInfo> methodStructs)
  {
    var methods = type.GetMethods(flags)
                      .Where(x=>!x.IsDefined(typeof(CompilerGeneratedAttribute), false) && x.MemberType != MemberTypes.Property && !x.IsDefined(typeof(IgnoreDocumentationAttribute), false) );
    methodStructs = new List<MethodInfo>();

   if(methods.Count() < 0) return;

    foreach(var m in methods) { 
      MethodInfo newMethod = new MethodInfo();

      var stat = (m.IsStatic) ? "static " : "";
      var accessmodifier = (m.IsPrivate) ? "private " : m.IsPublic ?  "public " : "protected";
      var isAbst = m.IsAbstract ? "abstract" : m.IsFinal ? "final" : m.IsVirtual ? "virtual" : ""; 
      var derived = (m.DeclaringType != type) ? true: false; 
      var returnType = "" + m.ReturnType.getFriendlyTypeName(false).ToLower();  
      var parameters = m.GetParameters();
      var hasParameters = parameters.Length > 0 ? true : false;
      var attributes = m.GetCustomAttributes().ToList();
      var isAsync = m.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null ? "async " : "";    

      if(attributes.Count > 0){ 
        newMethod.attributes = new List<string>();
        foreach(var attribute in attributes){ newMethod.attributes.Add(attribute.GetType().getFriendlyTypeName(false)); }  
      }

      newMethod.description = buildMethodSummary(m, type, hasParameters, out Dictionary<ParameterInfo, string> parameterInfos, out Dictionary<string, string> customTagVariables); 
      
      newMethod.parameters = parameterInfos;
      newMethod.customTagVariables = customTagVariables;  
      string parameterstring = "";
      foreach(var p in parameters) { 
          parameterstring += p.ParameterType.getFriendlyTypeName(p.IsOut) + " " + p.Name;  
        if(parameters.Last() != p) parameterstring += ", ";
      }

      if(derived) continue;
      else { newMethod.name = $"{accessmodifier}{isAsync}{stat}{isAbst}{returnType} {m.Name}" + "(" + $"{parameterstring}" + ")"; }
      methodStructs.Add(newMethod);
    }
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// Builds method summaries
  /// </summary>
  /// <param name="xml">xdocument</param>
  /// <param name="m">method info</param>
  /// <param name="type">class type</param>
  /// <param name="mb">markdown builder</param>
  /// <param name="hasParameters">is method has parameters?</param>
  /// <param name="parameters">parameterinfo list</param>
  /// <returns></returns>
  private string buildMethodSummary(System.Reflection.MethodInfo m, Type type, bool hasParameters, out Dictionary<ParameterInfo, string> parameters, out Dictionary<string, string> customTagVariables)
  {
    string nameDef = "", description = "";
    parameters = new Dictionary<ParameterInfo, string>();
    customTagVariables = new Dictionary<string, string>(); 
    var methodParameters = m.GetParameters();  
    if(hasParameters) {
      nameDef = m.Name;
      nameDef += "(";
      for(int i = 0; i< methodParameters.Length; i++) {
        if((methodParameters[i].ParameterType.GetGenericArguments().Length > 0)){
          var listParameterTypeName = methodParameters[i].ParameterType.ToString().Replace('&','@').Split('[', ']'); 
          var innerParameters = listParameterTypeName[0];
          List<string> paramsStrings = listParameterTypeName.ToList();  
          paramsStrings.RemoveAt(0);
          var paramsT = innerParameters.Split('`')[0]; 
          string genericParameters = "";  
          for(int j = 0; j< paramsStrings.Count; j++) {
            if(paramsStrings[j] == paramsStrings.First()) { 
              genericParameters += paramsStrings[j].Replace('+', '.');  
            }
            else if(paramsStrings[j] != paramsStrings.Last()) {  
              genericParameters += ",";
            }
          } 
           nameDef += paramsT + "{" + genericParameters + "}" + "@";      
        }
        else{
           if(methodParameters[i].IsOut) {
              nameDef += methodParameters[i].ParameterType.ToString().Replace('&','@'); 
            }
            else{
              nameDef += methodParameters[i].ParameterType;  
            } 
        } 
        if(i != methodParameters.Length-1) nameDef += ",";   
      }
      nameDef += ")";  
    }
    else{ 
        nameDef += m.Name; 
    }

    var desc = xml.getDocumentation("M:" + type.fullName() + "." + nameDef)?.XPathSelectElement("summary")?.Value.Trim() ?? "";
    if (desc != "") description = desc;

    foreach(var tag in DocumentGenerator.customTags){
      var customTag = xml.getDocumentation("M:" + type.fullName() + "." + nameDef)?.XPathSelectElement(tag)?.Value.Trim() ?? "";
      if(customTag != "")
        customTagVariables.Add(tag, customTag);
    }    
  
    foreach(var p in methodParameters) {
        var summary = xml.getDocumentation("M:" + type.fullName() + "." + nameDef)?.XPathSelectElements("param")?.FirstOrDefault(item => item.Attribute("name").Value == p.Name)?.Value.Trim() ?? "";
        if(summary != "") { 
            parameters.Add(p, summary); 
        } 
    }
    return description;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// Creates class properties
  /// </summary>
  /// <param name="type">class type</param>
  /// <param name="mb">markdown builder</param>
  /// <param name="xml">xdocument file</param>
  /// <param name="propertyType"></param>
  /// <param name="propertyName"></param>
  /// <param name="propertySummary"></param>
  /// <param name="propertyValue"></param> 
  private void buildProperties(Type type, XDocument xml, out Dictionary<PropertyInfo, string> propInfos)
  {
    PropertyInfo[] propInfo = type.GetProperties(flags);
    propInfos = new Dictionary<PropertyInfo, string>();
      if(propInfo.Length > 0) {
        foreach(var p in propInfo) {
        var desc = xml.getDocumentation("P:" + type.FullName + "." + p.Name)?.XPathSelectElement("summary")?.Value.Trim() ?? "";
        propInfos.Add(p, desc);
      }
    }
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// Builds current class nested data types
  /// </summary>
  /// <param name="type">class type</param>
  /// <param name="mb">Markdown builder</param>
  /// <param name="xml">xdocument file</param>
  private void buildEnums(Type type, XDocument xml, out List<EnumInfo> enums)
  {
    Type[] types = type.GetNestedTypes(flags); 
    enums = new List<EnumInfo>();
    if(types.Length > 0 ) { 
      foreach(var t in types) { 
        if(t.IsEnum) {
          EnumInfo enumInfo = new EnumInfo();  
          enumInfo.enumMembers = System.Enum.GetValues(t);
          enumInfo.enumType = t;
          enumInfo.enumDescription = xml.getDocumentation("T:" + t.fullName())?.XPathSelectElement("summary")?.Value.Trim() ?? "";
          enums.Add(enumInfo);
        }
      }
    }  
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// Builds nested types like sub classes and delegates
  /// </summary>
  /// <param name="t">Parent class type</param>
  /// <returns>Dictionary<string, Type>()</returns>
  private Dictionary<string, Type> buildInnerNestedTypes(Type t)
  {   
    Type[] types = t.GetNestedTypes(flags);   
    Dictionary<string, Type> nestedList = new Dictionary<string, Type>();  
    if(types.Length > 0) {
      foreach(var ti in types) { 
        bool isCompilerValidAttributes = (Attribute.GetCustomAttribute(ti, typeof(CompilerGeneratedAttribute)) == null ? true : false);
        bool isNotAsyncValidAttributes = (Attribute.GetCustomAttribute(ti, typeof(AsyncStateMachineAttribute)) == null ? true: false);
        if(isCompilerValidAttributes && isNotAsyncValidAttributes && ((ti.IsClass) || (ti.IsValueType && !ti.IsPrimitive && !ti.IsEnum))) {       
           nestedList.Add(ti.fullName(), ti);    
           //File.WriteAllText(settings.outputPath + "/" + ti.fullName()  + ".md", DocumentGenerator.documentType(ti));       
        }    
      }
    }
    return nestedList;
  }


  // -------------------------------------------------------------------------
  /// <summary>
  /// Creates class variables table
  /// </summary>
  /// <param name="type">class type</param>
  /// <param name="mb">markdown builder</param>
  public FieldInfo[] buildFields(Type type)
  {  
    var fieldInfos = type.GetFields(flags | BindingFlags.DeclaredOnly) 
                         .Where(x=>!x.IsDefined(typeof(CompilerGeneratedAttribute), false) && !x.IsDefined(typeof(IgnoreDocumentationAttribute), false) ).ToArray();
    List<FieldInfo> fields = new List<FieldInfo>();
    foreach(var fi in fieldInfos) {
      FieldInfo field = new FieldInfo();

      field.name = fi.Name;
      field.accessModifier = fi.IsPublic ? "public" : fi.IsPrivate ? "private" : fi.IsFamily ? "internal" :  "protected";
      field.inference = fi.IsStatic ? "static" : "-";   
      field.type = fi.FieldType;
      field.description = xml.getDocumentation("F:" + type.fullName()+"."+fi.Name)?.XPathSelectElement("summary")?.Value.Trim() ?? "";

      fields.Add(field);
    }
    return fields.ToArray(); 
  }


  
}

} // End of namespace hc