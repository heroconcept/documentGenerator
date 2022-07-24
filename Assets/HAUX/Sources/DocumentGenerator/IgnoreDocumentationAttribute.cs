/* ----------------------------------------------------------------------- *

    * IgnoreDocumentationAttribute.cs.cs

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
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace hc {

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/// <summary>
/// This Attributes ensure that document generator mark as not suppose to be generate.
/// </summary>
public class IgnoreDocumentationAttribute: Attribute
{

}

} // End of namespace hc