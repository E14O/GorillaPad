using System;
using System.Collections.Generic;
using System.Text;

namespace GorillaPad.Interfaces
{
    internal class AppCreation
    {
        // foreach app, it needs to check if the name matches a defualt value if it does it needs to enable that app and run the functions
        // if it is not a defualt value it needs to get the set custom icon and app name and create a new app and run its functions. 

        public void Start()
        {
            var DefaultApps = new List<string> { "Credits", "Scoreboard", "Music" };
        }


        public static void ApplicationCreation(string appName)
        {
            AppSystem appInstance;
        }
    }

}
