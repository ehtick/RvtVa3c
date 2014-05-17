#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
#endregion

namespace RvtVa3c
{

  [Transaction( TransactionMode.Manual )]
  public class Command : IExternalCommand
  {
    void ExportView3D( View3D view3d )
    {
      Document doc = view3d.Document;

      Va3cExportContext context
        = new Va3cExportContext( doc );

      CustomExporter exporter = new CustomExporter(
        doc, context );

      // Note: Excluding faces just suppresses the 
      // OnFaceBegin calls, not the actual processing 
      // of face tessellation. Meshes of the faces 
      // will still be received by the context.

      exporter.IncludeFaces = false;

      exporter.ShouldStopOnError = false;

      exporter.Export( view3d );
    }

    public Result Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      UIDocument uidoc = uiapp.ActiveUIDocument;
      Application app = uiapp.Application;
      Document doc = uidoc.Document;

      if( doc.ActiveView is View3D )
      {
        ExportView3D( doc.ActiveView as View3D );
      }
      else
      {
        TaskDialog.Show( "va3c", 
          "You must be in 3D view to export." );
      }
      return Result.Succeeded;
    }
  }
}