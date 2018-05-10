using System.IO;
using GTA;
using GT_MP_vehicleInfo.Data;
using Newtonsoft.Json;

namespace GT_MP_vehicleInfo.Processors
{
    public class OutputProcessor
    {

        public static JsonSerializerSettings defaultSettings = NewSettings;

        public static JsonSerializerSettings NewSettings => new JsonSerializerSettings
        {
            ContractResolver = new GeneralResolver()
        };
        
        public static void OutputVehicleInfo()
        {
            JsonSerializerSettings settings = defaultSettings;

            settings.Formatting = Formatting.Indented;
            // Large, Indented (For research purposes)
            OutputToJson(settings, ".ind");
                
            settings.Formatting = Formatting.None;
                
            // Normal, with translation
            OutputToJson(settings, "-" + CarInfoGen.languageCode + ".full");
                
            // Smaller, without translation
            settings.ContractResolver = new NoLocalizationResolver();
            OutputToJson(settings, ".noloc");
                
            // Smallest, without lists
            settings.ContractResolver = new NoListsResolver();
            OutputToJson(settings, ".nolist");

            defaultSettings = NewSettings;
            
            OutputSeperateVehicles();
        }

        private static void OutputSeperateVehicles()
        {
            foreach (var entry in CarInfoGen.Storage.vehicleStorage)
            {
                Process(@"output/vehicleInfo-" + CarInfoGen.languageCode + "/" + entry.Key + ".json", entry.Value);
            }
            
            // COMPRESS FILES
            var zipfile = CarInfoGen.GetPath("output/vehicleInfo-" + CarInfoGen.languageCode + ".zip");
            if(File.Exists(zipfile)) File.Delete(zipfile);
            System.IO.Compression.ZipFile.CreateFromDirectory(CarInfoGen.GetPath("output/vehicleInfo-" + CarInfoGen.languageCode + "/"), zipfile);
        }
        
        private static void OutputToJson(JsonSerializerSettings settings, string extension)
        {
            Process(@"output/vehicleInfo" + extension + ".json", CarInfoGen.Storage.vehicleStorage, settings);
        }
        
        public static void Process(string path, object data, JsonSerializerSettings settings = null)
        {
            if (settings == null) settings = defaultSettings;
            string payload = JsonConvert.SerializeObject(data, settings);
            
            try
            {
                File.WriteAllText(CarInfoGen.GetPath(path, true), payload);
            }
            catch (IOException e)
            {
                CarInfoGen.LogIt("AN ERROR OCCURED: " + e.Message);
            }
            
        }
        
        
    }
}