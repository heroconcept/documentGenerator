/* ----------------------------------------------------------------------- *

    * ClassBuilder.cs.cs

    ----------------------------------------------------------------------

    Copyright (C) 2022, HERO CONCEPT YAZILIM VE BİLİŞİM ANONİM ŞİRKETİ
    All Rights Reserved.

    THIS SOFTWARE IS PROVIDED 'AS-IS', WITHOUT ANY EXPRESS
    OR IMPLIED WARRANTY. IN NO EVENT WILL THE AUTHOR(S) BE HELD LIABLE FOR
    ANY DAMAGES ARISING FROM THE USE OR DISTRIBUTION OF THIS SOFTWARE

    Hero Concept <info@heroconcept.com>

* ------------------------------------------------------------------------ */
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace hc {

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public class MarkdownDocumentBuilder : IDocumentBuilder{

  /// <summary>
  /// markdown builder class
  /// </summary>
  /// <returns></returns>
  public MarkdownBuilder markdownBuilder = new MarkdownBuilder();
  
  /// <summary>
  /// document structure
  /// </summary>
  private DocumentStructureGenerator.Structure structure;

  // -------------------------------------------------------------------------
  /// <summary>
  /// returns structure
  /// </summary>
  /// <returns></returns>
  public DocumentStructureGenerator.Structure getStructure(){
      return structure;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// sets structure
  /// </summary>
  /// <param name="str"></param>
  public void setStructure(DocumentStructureGenerator.Structure str){
      this.structure = str;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// returns documentation contexts
  /// </summary>
  /// <returns></returns>
  public String getDocumentContext(){
      return markdownBuilder.stringBuilder.ToString();
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// returns file extention
  /// </summary>
  /// <returns>return .md </returns>
  public string getFileExtention(){
      return ".md";
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// builds structure name and desc
  /// </summary>
  public void buildStructureNameAndDescriptions(){
    markdownBuilder.headerWithLink(1, structure.info.type.fullName(), "#"+ "T:" + structure.info.type.fullName()); 
    markdownBuilder.appendLine(structure.info.description);  
    markdownBuilder.code("csharp", structure.info.body);   
  }


  // -------------------------------------------------------------------------
  /// <summary>
  /// builds methods
  /// </summary>
  public void buildMethods(){
    if(structure.methodStructs.Count > 0){
        markdownBuilder.header(3, "Methods");
        foreach(var m in structure.methodStructs) {
            var codeBlock = new StringBuilder();
            foreach(var ctm in m.customTagVariables){
                markdownBuilder.appendLine($"**{MarkdownBuilder.markdownCodeQuote(ctm.Key)}** - {ctm.Value}");
                markdownBuilder.appendLine();
            }
            if(m.description != "") markdownBuilder.append($"**{MarkdownBuilder.markdownCodeQuote("Description")}** : {m.description}"); 
            markdownBuilder.appendLine();    
            foreach(var p in m.parameters) {  
                markdownBuilder.append("- ");
                markdownBuilder.appendLine($"{MarkdownBuilder.markdownCodeQuote(p.Key.ParameterType.getFriendlyTypeName(p.Key.IsOut) +" "+ p.Key.Name)} : {p.Value}");
            }
            markdownBuilder.appendLine();
            if(m.attributes != null){ 
            
                foreach(var attribute in m.attributes) {
                    codeBlock.AppendLine();
                    codeBlock.Append("[");  
                    codeBlock.Append(attribute);   
                    codeBlock.Append("]"); 
                    codeBlock.AppendLine();     
                }
            }
            codeBlock.AppendLine(m.name);       
            markdownBuilder.code("csharp", codeBlock.ToString());   
        }
      }
  
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build events
  /// </summary>
  public void buildEvents(){
      if(structure.eventInfos.Length >0){
          markdownBuilder.header(3, "Events");  
          foreach(var e in structure.eventInfos){ 
              markdownBuilder.code("csharp", "event " + e.EventHandlerType.Name + " " + e.Name);  
              markdownBuilder.quote("Follow the rabbit hole: "); 
              markdownBuilder.append(" ");    
              markdownBuilder.link(e.EventHandlerType.Name, $"{e.EventHandlerType.fullName()}.md#{e.EventHandlerType.fullName()}");
              markdownBuilder.appendLine(); 
          }
      }
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build fields
  /// </summary>
  public void buildFields() {
      if(structure.fieldInfos.Length > 0){
            markdownBuilder.header(3, "Fields"); 
            var isTableCreated = false;
            string[] tableHead = new[] { "Access Modifier","Inference","Type", "Name", "Summary" };
            IEnumerable<string[]> dataSet = null;   
            foreach(var f in structure.fieldInfos) {
                if(!isTableCreated){
                dataSet = DocumentGeneratorUtils.buildFieldDataset(structure.fieldInfos, f=>f.accessModifier, f=>f.inference, f=>f.type, f=>f.name, f=>f.description);
                isTableCreated = true; 
                }
            } 
            markdownBuilder.table(tableHead, dataSet);  
            markdownBuilder.appendLine(); 
      } 
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build properties
  /// </summary>
  public void buildProperties(){
      if(structure.properties.Count > 0) {
          markdownBuilder.header(3, "Properties"); 
          foreach(var p in structure.properties){
            markdownBuilder.quote(p.Value);
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("public " + p.Key.PropertyType.getFriendlyTypeName(false) + " " + p.Key.Name + " {" + "} ");
            markdownBuilder.code("csharp", stringBuilder.ToString());  
          }  
      } 
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build inner nested types
  /// </summary>
  public void buildInnerNestedTypes(){
      buildEnums(structure.enumInfos);
      buildNestedTypeLinkList(structure.innerNestedTypes);
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// builds enums
  /// </summary>
  /// <param name="enumInfos"></param>
  public void buildEnums(List<DocumentStructureGenerator.EnumInfo> enumInfos){
      if(enumInfos.Count >0) {
        var enumString = new StringBuilder();
        markdownBuilder.header(3, "Enums");

        foreach (var e in enumInfos)  
        {
          StringBuilder sb = new StringBuilder();
          markdownBuilder.quote(e.enumDescription);
          sb.AppendLine(e.enumType.IsPublic ? "public" : "private " + e.enumType.Name + " {");  
          foreach(var m in e.enumMembers) {
              sb.Append("\t");
              sb.AppendLine( m + ",");  
          }
          sb.AppendLine("}");
          markdownBuilder.code("csharp", sb.ToString()); 
          //markdownBuilder.table(head, DocumentGeneratorUtils.buildNestedDataset(e.Value, f=>Enum.GetUnderlyingType(e.Key).IsPublic ? "public" : "private",  f=>"-",f => f.Name));   
          markdownBuilder.appendLine();  
        }
      }
  }

  // ---------------------------------------------------------------------------
  /// <summary>
  /// builds other nested type like struct or class...
  /// </summary>
  /// <param name="nestedList"></param>
  public void buildNestedTypeLinkList(Dictionary<string, Type> nestedList){
      if(nestedList != null) {
        markdownBuilder.header(3, "Nested Types"); 
        foreach(var n in nestedList) {     
            markdownBuilder.listLink(n.Key, $"{n.Value.fullName()}.md#{n.Value.fullName()}");    
        }
      }
  }
}


} // End of namespace hc