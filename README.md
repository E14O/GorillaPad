# GorillaPad!
GorillaPad is a GorillaTag Mod Made By E14O & H4RNS, past developers include Wryser, NotABird, Striker67, Ty, Cyn Who Helped Develop The Older Versions Of GorillaPad.

## Q&A
How Do I Install GorillaPad?
- Currently GorillaPad Is Tester Only Till Release, Feel Free To Build The Source If You Know How To.

How Do I Make Custom Apps?
- Follow The Guide Below To Successfully Create A Custom App

How Do I Install Custom Apps?
- Leaderboard, Music, Camera Will All Be In The Download When You First Install The Mod. Additinal Custom Apps Made By The Community Are Found In [GorillaPad Discord](<https://discord.gg/ntnGzFTMB6>)

## For Developers:

You Will Need To Go To E14O/GorillaPadCustomAppProject And Follow The Guide There To Get Your .app First.

This Is The Code Needed To Create Custom Apps:
```csharp
using GorillaPad.Interfaces;

namespace GorillaPad
{
    // make sure the class name is the same as your app name that you exported in the unity project.
    internal class YourApp : AppModule 
    {
        public override string AppName => "YourApp";  // This is what your app will be called on the homepage.
        public override string AppVersion => "0.0.1"; // Enter your app version here (This will be displayed in the bottom left hand corner of your app as defualt).

        // OnAppOpened is called when the user opens the app
        public override void OnAppOpen()
        {
            base.OnAppOpen();
            // Code here will be run before AppContent. So do start up code here and find gameobj inside AppContent to avoid null refrences.
        }

        public override void AppContent()
        {
            base.AppContent();
            // Main app content here (find gamobjs, add button scripts e.c).
        }

        public override void Tick()
        {
            base.Tick();
            // This acts like update. While your app is open, code here will run every frame.
        }

        // Note: DO NOT REMOVE (base.AppContent();) & (base.OnAppOpen();) & (base.Tick();), Your App will automatically have a button script that runs OnAppOpen, this will also automatically open your app screen you made. 
    }
}
```
If You Want To Create A Button, Use Our Built In System:
```csharp

public override void AppContent()
{
    base.AppContent();

    GameObject parent = Main.instance.AppInterfaces.transform.Find($"{AppName}App").gameObject;
    PadButton.Create(parent, "Obj", SelectedAudio.ButtonAudio, ButtonFunction);
}

void ButtonFunction()
{
    PadLogging.LogMessage("This Gets Ran When The Button Is Clicked");
}

```
If You Need Help Creating A Custom App Join The [Discord](<https://discord.gg/ntnGzFTMB6>)

## Disclaimer
This product is not affiliated with Another Axiom Inc. or its videogames Gorilla Tag and Orion Drift and is not endorsed or otherwise sponsored by Another Axiom. Portions of the materials contained herein are property of Another Axiom. ©2021 Another Axiom Inc.
