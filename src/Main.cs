using System.IO;
using System.Windows.Forms;
using GTA;
using GTA.UI;
using GT_MP_vehicleInfo.Processors;
using NLog;

namespace GT_MP_vehicleInfo
{
    public class CarInfoGen : Script
    {
        public static Logger Logger = LogManager.GetCurrentClassLogger();
        public static readonly string BasePath = @"N:\gt-mp\bin\scripts\vehicleinfo";
        public static readonly Storage Storage = new Storage();
        public static string languageCode = "de";
        public static CarInfoGen Instance;

        public CarInfoGen()
        {
            LogIt("~y~Loading...");
            LogIt(Path.Combine(CarInfoGen.BasePath, "vehiclemeta/"));
            KeyUp += OnKeyUp;
            Tick += CarInfoGen_Tick;
            Instance = this;
        }

        private void CarInfoGen_Tick(object sender, System.EventArgs e)
        {
            LogIt("TICK");
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.NumPad4)
            {
                foreach (var weapon in Enum.GetValues(typeof(WeaponHash)))
                {
                    Game.Player.Character.Weapons.Give((WeaponHash) weapon, 100, true, true);
                }
                
            }
            if (e.KeyCode == Keys.NumPad2)
            {
                Vehicle veh = World.CreateVehicle((VehicleHash) Game.GenerateHash(Game.GetUserInput()),
                    Game.Player.Character.Position + Game.Player.Character.ForwardVector * 3.0f,
                    Game.Player.Character.Heading);
                veh.PlaceOnGround();
            }*/
            LogIt("~y~Keyup ... "+e.KeyCode.ToString());
            if (e.KeyCode == Keys.NumPad1)
            {
                if (string.IsNullOrEmpty(languageCode)) languageCode = Game.GetUserInput("de");
                
                LogIt("~y~Starting...");
                
                VehicleLoader.LoadVehicles();
                ModAssignProcessor.Process();
                LocalizationProcessor.Process();
                CleanupProcessor.Process();
                

                OutputProcessor.OutputVehicleInfo();
               
                
                LogIt("~g~Finished!");
            }
        }
        public static void LogIt(string msg)
        {
            Logger.Info(msg);
        }
        
        public static string GetPath(string path, bool create = false)
        {
            string resultPath = Path.Combine(BasePath, path);
            if (create)Directory.CreateDirectory(Path.GetDirectoryName(resultPath));
            return resultPath;
        }
    }
}
