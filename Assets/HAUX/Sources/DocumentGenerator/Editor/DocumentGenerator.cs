/* ----------------------------------------------------------------------- *

    * DocumentGenerator.cs

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
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.Runtime.CompilerServices;
using System.Threading;

namespace hc {
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// Documentation Generator class responsable of the creating documentation of classes and methods.
/// </summary>
public class DocumentGenerator
{
  // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  delegate bool ProcessOrderRulesDelegate(ref IDocumentBuilder builder);

  // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
  /// <summary>
  /// Settings
  /// </summary>
  [Serializable]
  public class Settings
  {
    /// <summary>
    /// path
    /// </summary>
    public string outputPath = "docs";
    public string applicationDocumentaionRoot = "HAUX";
    public string fileExtension = "*.asmdef";

    public bool generateEveryCompile = false;

    public bool generateHaux = true;
    public bool generateOtherScripts = true;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// Settings class
  /// </summary>
  /// <returns></returns>
  public static Settings settings = new Settings();

  public static List<UnityEditor.Compilation.Assembly> filteredAssemblies;
  public static List<string> filters = new List<string>();

  // -------------------------------------------------------------------------
  public static string[] customTags = new string[]{"hc.TODO", "hc.WARNING", "hc.ERROR"};

  // -------------------------------------------------------------------------
  /// <summary>
  /// xdocument
  /// </summary>
  XDocument xml;

  // -------------------------------------------------------------------------
  /// <summary>
  /// Builds all classes.
  /// </summary>
  /// <param name="xml">xml document</param>
  /// <param name="type">class type</param>
  /// <param name="mb">markdown builder</param>
  /// <returns></returns>
  public string documentType(Type type, IDocumentBuilder builder)
  {
    string rv = "";
    if(type != null) {

      DocumentStructureGenerator generator = new DocumentStructureGenerator(xml);
      DocumentStructureGenerator.Structure structure = generator.buildNew(type);
      /*if(structure.innerNestedTypes.Count >0){
        foreach(var nested in structure.innerNestedTypes){
          //generateInnerNestedTypes(nested.Value, builder);
        }
      }*/

      if(builder.getStructure() == null) {
        builder.setStructure(structure);
      }

      foreach(ProcessOrderRulesDelegate rule in new ProcessOrderRulesDelegate[] {
        buildNameDescription,
        buildEvents,
        buildFields,
        buildProperties,
        buildMethods }) {
          if(!rule(ref builder)) {
              continue;
          }
        }

    }
    return rv += builder.getDocumentContext();
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// buildNameDescription
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public bool buildNameDescription(ref IDocumentBuilder builder)
  {
    builder.buildStructureNameAndDescriptions();
    return true;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build methods
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public bool buildMethods(ref IDocumentBuilder builder)
  {
    builder.buildMethods();
    return true;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build events and delegates
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public bool buildEvents(ref IDocumentBuilder builder)
  {
    builder.buildEvents();
    return true;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build fields
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public bool buildFields(ref IDocumentBuilder builder)
  {
    builder.buildFields();
    return true;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build properties
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public bool buildProperties(ref IDocumentBuilder builder)
  {
    builder.buildProperties();
    return true;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// build inner nested types
  /// </summary>
  /// <param name="builder"></param>
  /// <returns></returns>
  public bool buildInnerNestedTypes(ref IDocumentBuilder builder)
  {
    builder.buildInnerNestedTypes();
    return true;
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// Creating folder and classes from assemblies.
  /// </summary>
  /// <param name="casm">assembly compilation file</param>
  /// <hc.TODO>make something here</hc.TODO>
  /// <hc.WARNING>this is a warning</hc.WARNING>
  /// <hc.ERROR>this is a cause an error</hc.ERROR>
  public void generate(UnityEditor.Compilation.Assembly casm)
  {
    var assembly = AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == casm.name);
    xml = File.Exists(casm.xmlFileName()) ? XDocument.Load(casm.xmlFileName()) : XDocument.Parse(@"<?xml version=""1.0""?><docs/>");
    Directory.CreateDirectory(settings.outputPath);
    foreach(Type type in assembly.GetTypes().Where(t => (t.IsClass || t.IsInterface || t.IsValueType) && !t.IsDefined(typeof(CompilerGeneratedAttribute), false))) {

      IDocumentBuilder builder = new MarkdownDocumentBuilder();
      var output = documentType(type, builder);
      try
      {
        File.WriteAllText(settings.outputPath + "/" + type.fullName() + ".md", output);
      }
      catch(Exception ex)
      {
        Debug.Log(ex);
      }

    }
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// onGUI function.
  /// </summary>
  public static void onGUI()
  {

    if (GUILayout.Button("Generate documentation"))
    {
      createFilters();
      var all = CompilationPipeline.GetAssemblies(AssembliesType.Editor).Concat(CompilationPipeline.GetAssemblies(AssembliesType.Player));
      filteredAssemblies = new List<UnityEditor.Compilation.Assembly>();
      foreach(var a in all){
        if(checkFilter(a.name, filters)){
          filteredAssemblies.Add(a);
          new DocumentGenerator().generate(a);
        }
      }
    }
    if(settings.generateEveryCompile)
    {
      foreach(var asm in filteredAssemblies) {
        CompilationPipeline.RequestScriptCompilation();
      }
    }
  }

  // -------------------------------------------------------------------------
  /// <summary>
  /// Init function.
  /// </summary>
  [InitializeOnLoadMethod]
  static void init()
  {
    CompilationPipeline.compilationStarted += (o) => {
    if(settings.generateEveryCompile)
    {
      Directory.CreateDirectory("Temp/xmldoc");
      foreach(var asm in filteredAssemblies) {
        asm.compilerOptions.AdditionalCompilerArguments = new string[] {
        "-doc:" + asm.xmlFileName()
        };
      }

      }
    };
    CompilationPipeline.assemblyCompilationFinished += (asmName, m) => {
      if(settings.generateEveryCompile)
      {
        UnityEditor.Compilation.Assembly asm = filteredAssemblies.FirstOrDefault(x=> x.name == asmName);
        if(asm != null)
        {
          new DocumentGenerator().generate(asm);
        }
      }
    };

  }

  // -------------------------------------------------------------------------
  static bool checkFilter(string element, List<string> filters)
  {
    foreach(var f in filters)
    {
      if(element.Contains(f)) return true;
    }
    return false;
  }

  // -------------------------------------------------------------------------
  public static void createFilters()
  {
    if(settings.generateHaux){
      string[] files = Directory.GetFiles(Application.dataPath+"/"+settings.applicationDocumentaionRoot, settings.fileExtension, SearchOption.AllDirectories);
      char[] separators = new char[] { ' ', '.' , '/'};
      foreach(var f in files)
      {
        var t = f.Replace('\\', '/').Split(separators).ToList();
        filters.Add(t[t.Count-2]);
      }
    }

    if(settings.generateOtherScripts)
    {
      filters.Add("Assembly-CSharp-Editor");
    }

  }


}

} // End of namespace hc
