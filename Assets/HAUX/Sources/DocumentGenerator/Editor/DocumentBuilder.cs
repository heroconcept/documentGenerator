/* ----------------------------------------------------------------------- *

    * DocumentBuilder.cs.cs

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
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace hc {

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
public interface IDocumentBuilder
{
  // -------------------------------------------------------------------------  
  DocumentStructureGenerator.Structure getStructure();

  // -------------------------------------------------------------------------
  void setStructure(DocumentStructureGenerator.Structure structure);

  // -------------------------------------------------------------------------
  String getDocumentContext();

  // -------------------------------------------------------------------------
  string getFileExtention();

  // -------------------------------------------------------------------------
  void buildStructureNameAndDescriptions();

  // -------------------------------------------------------------------------
  void buildMethods();

  // -------------------------------------------------------------------------
  void buildEvents();

  // -------------------------------------------------------------------------
  void buildFields();

  // -------------------------------------------------------------------------
  void buildProperties();

  // -------------------------------------------------------------------------
  void buildInnerNestedTypes();

}

} // End of namespace hc