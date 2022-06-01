using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LuachProject
{
    [ComVisible(true)]
    public class ScriptingObject
    {
        public void showSefirah(int dayOfOmer, bool hebrew)
        {
            string lang = hebrew ? "heb" : "eng";
            Process a = new();
            a.StartInfo.FileName = System.IO.Path.Combine(Application.StartupPath, @"OmerReminder.exe");
            a.StartInfo.Arguments = $"-remind -omerDay {dayOfOmer} -lang {lang} -nusach {Properties.Settings.Default.Nusach}";
            a.Start();

        }
    }
}
