using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Rhino.DocObjects
{
  public class Font
  {
    private readonly int m_index;
    private readonly RhinoDoc m_doc;
    private Font() { }
    internal Font(int index, RhinoDoc doc)
    {
      m_index = index;
      m_doc = doc;
    }

    public string FaceName
    {
      get
      {
        IntPtr pName = UnsafeNativeMethods.CRhinoFont_FaceName(m_doc.m_docId, m_index);
        if (IntPtr.Zero == pName)
          return null;
        return Marshal.PtrToStringUni(pName);
      }
    }

    const int idxBold = 0;
    const int idxItalic = 1;

    public bool Bold
    {
      get
      {
        return UnsafeNativeMethods.CRhinoFont_GetBool(m_doc.m_docId, m_index, idxBold);
      }
    }

    public bool Italic
    {
      get
      {
        return UnsafeNativeMethods.CRhinoFont_GetBool(m_doc.m_docId, m_index, idxItalic);
      }
    }
  }
}

namespace Rhino.DocObjects.Tables
{
  /// <summary>
  /// Font tables store the list of fonts in a Rhino document.
  /// </summary>
  public sealed class FontTable : IEnumerable<Font>, IDocObjectTable<Font>
  {
    private readonly RhinoDoc m_doc;
    private FontTable() { }
    internal FontTable(RhinoDoc doc)
    {
      m_doc = doc;
    }

    /// <summary>Document that owns this table</summary>
    public RhinoDoc Document
    {
      get { return m_doc; }
    }

    /// <summary>Number of fonts in the table</summary>
    public int Count
    {
      get
      {
        return UnsafeNativeMethods.CRhinoFontTable_FontCount(m_doc.m_docId);
      }
    }

    public Rhino.DocObjects.Font this[int index]
    {
      get
      {
        if (index < 0 || index >= Count)
          return null;
        return new Rhino.DocObjects.Font(index, m_doc);
      }
    }

    public int FindOrCreate(string face, bool bold, bool italic)
    {
      return UnsafeNativeMethods.CRhinoFontTable_FindOrCreate(m_doc.m_docId, face, bold, italic);
    }

    #region enumerator

    // for IEnumerable<Layer>
    public IEnumerator<Font> GetEnumerator()
    {
      return new TableEnumerator<FontTable, Font>(this);
    }

    // for IEnumerable
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return new TableEnumerator<FontTable, Font>(this);
    }

    #endregion

  }
}
