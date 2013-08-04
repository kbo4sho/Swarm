using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace XNASwarms
{
    public static class ImportExportHelper
    {
#if !WINDOWS_PHONE
        public static async Task<SaveAllSpecies> Export()
        {

            FileSavePicker exportPicker = new FileSavePicker();

            exportPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            exportPicker.FileTypeChoices.Add("XML", new List<string>() { ".xml" });
            exportPicker.DefaultFileExtension = ".xml";
            exportPicker.SuggestedFileName = "SwarmsSaves";
            exportPicker.CommitButtonText = "Export";
            StorageFile file = await exportPicker.PickSaveFileAsync();
            if (null != file)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (Stream outputStream = stream.AsStreamForWrite())
                    {
                        XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                        xmlWriterSettings.Indent = true;
                        xmlWriterSettings.Encoding = new UTF8Encoding(false);
                        XmlSerializer serializer = new XmlSerializer(typeof(SaveAllSpecies));
                        var game = await SaveHelper.LoadGameFile("AllSaved");
                        using (XmlWriter xmlWriter = XmlWriter.Create(outputStream, xmlWriterSettings))
                        {
                            serializer.Serialize(xmlWriter, game);
                        }
                        await outputStream.FlushAsync();
                        return game;
                    }
                }
            }
            return null;
        }
#endif

        public static async Task<SaveAllSpecies> Import()
        {
            FileOpenPicker importPicker = new FileOpenPicker();
            importPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            importPicker.FileTypeFilter.Add(".xml");
            importPicker.CommitButtonText = "Import";
            StorageFile file = await importPicker.PickSingleFileAsync();
            if (null != file)
            {
                using (var stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    using (Stream inputStream = stream.AsStreamForRead())
                    {
                        SaveAllSpecies data;
                        XmlSerializer serializer = new XmlSerializer(typeof(SaveAllSpecies));
                        using (XmlReader xmlReader = XmlReader.Create(inputStream))
                        {
                            data = (SaveAllSpecies)serializer.Deserialize(xmlReader);
                        }
                        await inputStream.FlushAsync();
                        return data;
                    }                    
                }
            }
            else
            {
                //nothing
            }

            return null;
        }
    }
}
