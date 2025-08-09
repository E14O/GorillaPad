using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Text;
using UnityEngine;

namespace GorillaPad.Interfaces
{
    public abstract class AppSystem
    {
        public abstract string AppName { get; }
        public abstract string AppVersion { get; }

        public virtual void OnAppOpen()
        {

        }
        public virtual void OnAppClose()
        {

        }

        public virtual void AppContent()
        {

        }
    }
}
