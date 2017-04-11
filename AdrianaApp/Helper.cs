using AdrianaApp.Models.Data;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using OpenXmlPowerTools;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace AdrianaApp
{
   public class Helper
    {
        public static string GetPlainText(OpenXmlElement element)
        {
            StringBuilder PlainTextInWord = new StringBuilder();
            foreach (OpenXmlElement section in element.Elements())
            {
                switch (section.LocalName)
                {
                    // Text 
                    case "t":
                        PlainTextInWord.Append(section.InnerText);
                        break;


                    case "cr":                          // Carriage return 
                    case "br":                          // Page break 
                        PlainTextInWord.Append(Environment.NewLine);
                        break;


                    // Tab 
                    case "tab":
                        PlainTextInWord.Append("\t");
                        break;


                    // Paragraph 
                    case "p":
                        PlainTextInWord.Append(GetPlainText(section));
                        PlainTextInWord.AppendLine(Environment.NewLine);
                        break;


                    default:
                        PlainTextInWord.Append(GetPlainText(section));
                        break;
                }
            }


            return PlainTextInWord.ToString();
        }

        internal static StringBuilder GetHtml(string uploadPath, string FileName)
        {
            StringBuilder sb = new StringBuilder();
            using (WordprocessingDocument pac = WordprocessingDocument.Open(uploadPath + "/" + FileName, true))
            {
                HtmlConverterSettings settings = GetHtmlConverterSettings(FileName);
                XElement html = HtmlConverter.ConvertToHtml(pac, settings);

                // Note: the xhtml returned by ConvertToHtmlTransform contains objects of type
                // XEntity.  PtOpenXmlUtil.cs define the XEntity class.  See
                // http://blogs.msdn.com/ericwhite/archive/2010/01/21/writing-entity-references-using-linq-to-xml.aspx
                // for detailed explanation.
                //
                // If you further transform the XML tree returned by ConvertToHtmlTransform, you
                // must do it correctly, or entities will not be serialized properly.

              return  sb.Append(html.ToString(SaveOptions.DisableFormatting));
            }
        }
       

        internal static HtmlConverterSettings GetHtmlConverterSettings(string FileName)
        {
            string uploadPath = HttpContext.Current.Server.MapPath("~/uploads");
            var imageDirectoryName = (uploadPath + "\\" + FileName).Substring(0, (uploadPath + "\\" + FileName).Length - 5) + "_files";
            int imageCounter = 0;
            HtmlConverterSettings settings = new HtmlConverterSettings()
            {
                FabricateCssClasses = true,
                CssClassPrefix = "pt-",
                RestrictToSupportedLanguages = false,
                RestrictToSupportedNumberingFormats = false,
                ImageHandler = imageInfo =>
                {
                    DirectoryInfo localDirInfo = new DirectoryInfo(imageDirectoryName);
                    if (!localDirInfo.Exists)
                        localDirInfo.Create();
                    ++imageCounter;
                    string extension = imageInfo.ContentType.Split('/')[1].ToLower();
                    ImageFormat imageFormat = null;
                    if (extension == "png")
                    {
                        // Convert png to jpeg.
                        extension = "gif";
                        imageFormat = ImageFormat.Gif;
                    }
                    else if (extension == "gif")
                        imageFormat = ImageFormat.Gif;
                    else if (extension == "bmp")
                        imageFormat = ImageFormat.Bmp;
                    else if (extension == "jpeg")
                        imageFormat = ImageFormat.Jpeg;
                    else if (extension == "tiff")
                    {
                        // Convert tiff to gif.
                        extension = "gif";
                        imageFormat = ImageFormat.Gif;
                    }
                    else if (extension == "x-wmf")
                    {
                        extension = "wmf";
                        imageFormat = ImageFormat.Wmf;
                    }

                    // If the image format isn't one that we expect, ignore it,
                    // and don't return markup for the link.
                    if (imageFormat == null)
                        return null;

                    string imageFileName = imageDirectoryName + "\\image" +
                        imageCounter.ToString() + "." + extension;
                    try
                    {
                        imageInfo.Bitmap.Save(imageFileName, imageFormat);
                    }
                    catch (System.Runtime.InteropServices.ExternalException)
                    {
                        return null;
                    }
                    var imageFileName1 = "..\\uploads" + imageFileName.Substring(uploadPath.Length);
                    XElement img = new XElement(Xhtml.img,
                        new XAttribute(NoNamespace.src, imageFileName1),
                        imageInfo.ImgStyleAttribute,
                        imageInfo.AltText != null ?
                            new XAttribute(NoNamespace.alt, imageInfo.AltText) : null);
                    return img;
                }
            };

            return settings;
        }
    }
}
