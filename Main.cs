using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIExportToImage
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            ViewPlan viewPlan = new FilteredElementCollector(doc)
                        .OfClass(typeof(ViewPlan))
                        .Cast<ViewPlan>()
                        .FirstOrDefault(view => view.ViewType == ViewType.FloorPlan && view.Name.Equals("Level 1"));

            IList<ElementId> viewsAndSheets = new List<ElementId>();
            viewsAndSheets.Add(viewPlan.Id);

            var imageOption = new ImageExportOptions();
            imageOption.FilePath = @"G:\Рабочий стол\2035\BIM\Revit API\Задание 8.3\экспорт.png";
            imageOption.PixelSize = 1980;
            imageOption.HLRandWFViewsFileType = ImageFileType.PNG;
            imageOption.ImageResolution = ImageResolution.DPI_150;
            imageOption.ExportRange = ExportRange.SetOfViews;

            imageOption.SetViewsAndSheets(viewsAndSheets);

            using (var ts = new Transaction(doc, "export image"))
            {
                ts.Start();

                doc.ExportImage(imageOption);

                ts.Commit();
            }
            return Result.Succeeded;
        }
    }
}
