/* ----------------------------------------------------------------------- *

    * DocumentGeneratorUtils.cs.cs

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
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace hc {

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public static class DocumentGeneratorUtils
{
  // -------------------------------------------------------------------------
  /// <summary>
  /// Gets Assembly file and returns xml file name.
  /// </summary>
  /// <param name="asm"> Asssembly file.</param>
  /// <returns></returns>
  public static string xmlFileName(this UnityEditor.Compilation.Assembly asm) => "Temp/xmldoc/" + Path.GetFileNameWithoutExtension(asm.outputPath) + ".xml";
  
  // --------------------------------------------------------------------------
  public static string fullName(this Type t) => t.FullName.Replace('+', '.');
  
  // --------------------------------------------------------------------------
  public static XElement getDocumentation(this XDocument doc, string member) => doc.XPathSelectElement("/doc/members/member[@name='" + member + "']");
  
  // --------------------------------------------------------------------------
  /// <summary>
  /// build field table data set
  /// </summary>
  /// <param name="array">data array</param>
  /// <param name="getAccessMofidier"></param>
  /// <param name="getInference"></param>
  /// <param name="getType"></param>
  /// <param name="finalName"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public static IEnumerable<string[]> buildFieldDataset<T>(T[] array, Func<T, string> getAccessMofidier, Func<T, string> getInference, Func<T, Type> getType, Func<T, string> finalName, Func<T, string> summary)
  {
    IEnumerable<T> seq = array; 
    var data = seq.Select(item => {
    return new[] { getAccessMofidier(item), getInference(item), MarkdownBuilder.markdownCodeQuote(getType(item).getFriendlyTypeName(false)), finalName(item), summary(item)};   
    });
    return data;
  }
  
  // ------------------------------------------------------------------------
  /// <summary>
  /// build nested dataset
  /// </summary>
  /// <param name="array"></param>
  /// <param name="getAccessMofidier"></param>
  /// <param name="getValue"></param>
  /// <param name="finalName"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public static IEnumerable<string[]> buildNestedDataset<T>(T[] array, Func<T, string> getAccessModifier, Func<T, string> getValue, Func<T, string> finalName)
  {
    IEnumerable<T> seq = array;
    var data = seq.Select(item => { 
    return new[] {getAccessModifier(item), finalName(item), getValue(item)};   
    });  
    return data; 
  }
  
  // ---------------------------------------------------------------------------
  public static string getFriendlyTypeName(this Type t, bool isOut)
  {
    var typeName = "";
    if(isOut) typeName += "out "; 
    typeName += t.Name.stripStartingWith("`"); 
    typeName = typeName.Replace("&", " out");
    var genericArgs = t.GetGenericArguments(); 
    if (genericArgs.Length > 0) {
        typeName += "<"; 
        foreach (var genericArg in genericArgs) {
            typeName += genericArg.getFriendlyTypeName(false) + ", ";
        }
        typeName = typeName.TrimEnd(',', ' ') + ">";
    }
    return typeName;
  }
  
  // ---------------------------------------------------------------------------
  public static string stripStartingWith(this string s, string stripAfter)
  {
    if (s == null) {
        return null;
    }
    var indexOf = s.IndexOf(stripAfter, StringComparison.Ordinal);
    if (indexOf > -1) {
        return s.Substring(0, indexOf);
    }
    return s;
  }
}
  
} // End of namespace hc