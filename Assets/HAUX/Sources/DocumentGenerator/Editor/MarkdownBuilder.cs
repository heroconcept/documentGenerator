/* ----------------------------------------------------------------------- *

    * MarkdownBuilder.cs.cs

    ----------------------------------------------------------------------

    Copyright (C) 2022, HERO CONCEPT YAZILIM VE BİLİŞİM ANONİM ŞİRKETİ
    All Rights Reserved.

    THIS SOFTWARE IS PROVIDED 'AS-IS', WITHOUT ANY EXPRESS
    OR IMPLIED WARRANTY. IN NO EVENT WILL THE AUTHOR(S) BE HELD LIABLE FOR
    ANY DAMAGES ARISING FROM THE USE OR DISTRIBUTION OF THIS SOFTWARE

    Hero Concept <info@heroconcept.com>

* ------------------------------------------------------------------------ */
using System.Collections.Generic;
using System.Text;
namespace hc {

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// https://github.com/neuecc/MarkdownGenerator
/// </summary>
public class MarkdownBuilder
{
  // -------------------------------------------------------------------------
  public static string markdownCodeQuote(string code)
  {
    return "`" + code + "`";
  }

  // -------------------------------------------------------------------------
  public StringBuilder stringBuilder = new StringBuilder();

  // -------------------------------------------------------------------------
  public void append(string text)
  {
    stringBuilder.Append(text);
  }

  // -------------------------------------------------------------------------
  public void appendLine()
  {
    stringBuilder.AppendLine();
  }

  // -------------------------------------------------------------------------
  public void appendLine(string text)
  {
    stringBuilder.AppendLine(text);
  }

  // -------------------------------------------------------------------------
  public void header(int level, string text)
  {
    for (int i = 0; i < level; i++) {
        stringBuilder.Append("#");
    }
    stringBuilder.Append(" ");
    stringBuilder.AppendLine(text);
  }

  // -------------------------------------------------------------------------
  public void headerWithCode(int level, string code)
  {
    for (int i = 0; i < level; i++) {
        stringBuilder.Append("#");
    }
    stringBuilder.Append(" ");
    codeQuote(code);
    stringBuilder.AppendLine();
  }

  // -------------------------------------------------------------------------
  public void headerWithLink(int level, string text, string url)
  {
    for (int i = 0; i < level; i++) {
        stringBuilder.Append("#");
    }
    stringBuilder.Append(" ");
    link(text, url);
    stringBuilder.AppendLine();
  }

  // -------------------------------------------------------------------------
  public void link(string text, string url)
  {
    stringBuilder.Append("[");
    stringBuilder.Append(text); 
    stringBuilder.Append("]");
    stringBuilder.Append("(");
    stringBuilder.Append(url);
    stringBuilder.Append(")");
  }

  // -------------------------------------------------------------------------
  public void image(string altText, string imageUrl)
  {
    stringBuilder.Append("!");
    link(altText, imageUrl);
  }

  // -------------------------------------------------------------------------
  public void code(string language, string code)
  {
    stringBuilder.Append("```");
    stringBuilder.AppendLine(language);
    stringBuilder.AppendLine(code);
    stringBuilder.AppendLine("```");
  }

  // -------------------------------------------------------------------------
  public void codeQuote(string code)
  {
    stringBuilder.Append("`");
    stringBuilder.Append(code);
    stringBuilder.Append("`");
  }

  // -------------------------------------------------------------------------
  public void table(string[] headers, IEnumerable<string[]> items)
  {
    stringBuilder.Append("| ");
    foreach (var item in headers) {
        stringBuilder.Append(item);
        stringBuilder.Append(" | ");
    }
    stringBuilder.AppendLine();
    stringBuilder.Append("| ");
    foreach (var item in headers) {
        stringBuilder.Append("---");
        stringBuilder.Append(" | ");
    }
    stringBuilder.AppendLine();
    foreach (var item in items) {
        stringBuilder.Append("| ");
        foreach (var item2 in item) {
            stringBuilder.Append(item2);
            stringBuilder.Append(" | ");
        }
        stringBuilder.AppendLine();
    }
    stringBuilder.AppendLine();
  }

  // -------------------------------------------------------------------------
  public void list(string text) // nest zero
  {
    stringBuilder.Append("- ");
    stringBuilder.AppendLine(text);
  }

  // -------------------------------------------------------------------------
  public void listLink(string text, string url) // nest zero
  {
    stringBuilder.Append("- ");
    link(text, url);
    stringBuilder.AppendLine();
  }

  // -------------------------------------------------------------------------
  public void quote(string text){
    stringBuilder.AppendLine(">" + text); 
  }

  // -------------------------------------------------------------------------
  public override string ToString()
  {
    return stringBuilder.ToString();
  }

}
} // End of namespace hc