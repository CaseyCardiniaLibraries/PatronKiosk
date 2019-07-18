using System.IO;
using System.Xml.Serialization;

namespace CCLKiosk
{
    public class Configuration
    {
        #region Default Data
        private const string def_themeColour = "White";
        private const string def_resourceFolder = "~ResourceFiles";
        private const string def_mainBGImage = "None";
        private const int def_numOfButtons = 1;
        private const int def_buttonWidth = 2000;
        private const int def_buttonHeight = 2000;
        private const int def_mainRows = 1;
        private const int def_mainPaddingTop = 10;
        private const int def_mainPaddingRight = 10;
        private const int def_mainPaddingBottom = 10;
        private const int def_mainPaddingLeft = 10;
        private const int def_timeoutTime = 10;
        private const int def_idleTimeout = 30;
        private const string def_timeoutText = "Due to inactivity, you will be returned to the Home Screen in:";
        private const int def_timeoutWidth = 600;
        private const int def_timeoutHeight = 300;
        private const int def_timeoutFontSize = 14;
        private static readonly ButtonConfig[] def_appButtonsConfig = new ButtonConfig[3] { new ButtonConfig("None", " ", 20, "Black"), new ButtonConfig("None", " ", 20, "Black"), new ButtonConfig("None", " ", 20, "Black") };
        private static readonly HomeConfig def_homeButtonConfig = new HomeConfig("Default.png", "", 14, "Black", 64, 64, 10, 10, "TOPRIGHT");
        #endregion

        #region File Control
        // name of the .xml file
        public static string CONFIG_FILENAME = Path.Combine("ResourceFiles", "config.xml");

        public static ConfigurationData GetConfigData()
        {
            if (!File.Exists(CONFIG_FILENAME)) // create config file with default values
            {
                using (FileStream fs = new FileStream(CONFIG_FILENAME, FileMode.Create))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ConfigurationData));
                    ConfigurationData sxml = new ConfigurationData();
                    xs.Serialize(fs, sxml);
                    return sxml;
                }
            }
            else // read configuration from file
            {
                using (FileStream fs = new FileStream(CONFIG_FILENAME, FileMode.Open))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(ConfigurationData));
                    ConfigurationData sc = (ConfigurationData)xs.Deserialize(fs);
                    return sc;
                }
            }
        }
        #endregion

        #region Data Classes
        // this class holds configuration data
        public class ConfigurationData
        {
            public string themeColour;
            public string resourceFolder;
            public string mainBGImage;
            public int numOfButtons;
            public int buttonWidth;
            public int buttonHeight;
            public int mainRows;
            public int mainPaddingTop;
            public int mainPaddingRight;
            public int mainPaddingBottom;
            public int mainPaddingLeft;
            public int idleTimeout;
            public int timeoutTime;
            public string timeoutText;
            public int timeoutWidth;
            public int timeoutHeight;
            public int timeoutFontSize;
            public ButtonConfig[] appButtonsConfig;
            public HomeConfig homeButtonConfig;

            public ConfigurationData()
            {
                themeColour = def_themeColour;
                resourceFolder = def_resourceFolder;
                numOfButtons = def_numOfButtons;
                buttonWidth = def_buttonWidth;
                buttonHeight = def_buttonHeight;
                mainRows = def_mainRows;
                mainPaddingTop = def_mainPaddingTop;
                mainPaddingRight = def_mainPaddingRight;
                mainPaddingBottom = def_mainPaddingBottom;
                mainPaddingLeft = def_mainPaddingLeft;
                idleTimeout = def_idleTimeout;
                timeoutTime = def_timeoutTime;
                timeoutText = def_timeoutText;
                timeoutWidth = def_timeoutWidth;
                timeoutHeight = def_timeoutHeight;
                timeoutFontSize = def_timeoutFontSize;
                appButtonsConfig = def_appButtonsConfig;
                homeButtonConfig = def_homeButtonConfig;
            }
        }

        public class ButtonConfig
        {
            public string backgroundImageName;
            public string buttonText;
            public int fontSize;
            public string textColor;
            public ProcArg option;
            public ProcArg[] subOptions;

            public ButtonConfig() { }

            public ButtonConfig(string setImageName, string setText, int setSize, string setColor)
            {
                backgroundImageName = setImageName;
                buttonText = setText;
                fontSize = setSize;
                textColor = setColor;
            }

            public ButtonConfig(string setImageName, string setText, int setSize, string setColor, ProcArg setOption)
            {
                backgroundImageName = setImageName;
                buttonText = setText;
                fontSize = setSize;
                textColor = setColor;
                option = setOption;
            }

            public ButtonConfig(string setImageName, string setText, int setSize, string setColor, ProcArg setOption, ProcArg[] setSubOptions)
            {
                backgroundImageName = setImageName;
                buttonText = setText;
                fontSize = setSize;
                textColor = setColor;
                option = setOption;
                subOptions = setSubOptions;
            }
        }

        public class ProcArg
        {
            public string appProcName;
            public string arguments;
            public string text;

            public ProcArg() { }

            public ProcArg(string setName)
            {
                appProcName = setName;
            }

            public ProcArg(string setName, string setArgs)
            {
                appProcName = setName;
                arguments = setArgs;
            }

            public ProcArg(string setName, string setArgs, string setText)
            {
                appProcName = setName;
                arguments = setArgs;
                text = setText;
            }
        }

        public class HomeConfig : ButtonConfig
        {
            public int buttonWidth;
            public int buttonHeight;
            public int buttonPaddingVertical;
            public int buttonPaddingHorizontal;
            public string buttonPosition;

            public HomeConfig() { }

            public HomeConfig(string setImageName, string setText, int setSize, string setColor, int setWidth, int setHeight, int setPaddingVertical, int setPaddingHorizontal, string setPosition) : base(setImageName, setText, setSize, setColor)
            {
                buttonWidth = setWidth;
                buttonHeight = setHeight;
                buttonPaddingVertical = setPaddingVertical;
                buttonPaddingHorizontal = setPaddingHorizontal;
                buttonPosition = setPosition;
            }
        }
        #endregion
    }
}