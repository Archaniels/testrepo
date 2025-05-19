using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUBES_KPL.PengaturanWebsite.Model
{
    public class PengaturanWebsiteModel
    {
        public enum PengaturanWebsiteState
        {
            MainMenu,
            GeneralSettings,
            ContentSettings,
            Saving,
            Exit
        }

        // enum untuk mendefinisikan event pada automata
        public enum PengaturanWebsiteEvent
        {
            SelectGeneral,
            SelectContent,
            Save,
            Back,
            Quit
        }

        // kelas yang merepresentasikan Automata untuk navigasi pengaturan website
        public class PengaturanWebsiteAutomata
        {
            // state awal dan sekarang
            public PengaturanWebsiteState CurrentState { get; private set; } = PengaturanWebsiteState.MainMenu;

            // dictionary untuk menyimpan transisi state
            private readonly Dictionary<(PengaturanWebsiteState, PengaturanWebsiteEvent), PengaturanWebsiteState> _transitions;

            public PengaturanWebsiteAutomata()
            {
                // inisialisasi transisi state
                _transitions = new Dictionary<(PengaturanWebsiteState, PengaturanWebsiteEvent), PengaturanWebsiteState>
                {
                    // dari MainMenu
                    { (PengaturanWebsiteState.MainMenu, PengaturanWebsiteEvent.SelectGeneral), PengaturanWebsiteState.GeneralSettings },
                    { (PengaturanWebsiteState.MainMenu, PengaturanWebsiteEvent.SelectContent), PengaturanWebsiteState.ContentSettings },
                    { (PengaturanWebsiteState.MainMenu, PengaturanWebsiteEvent.Quit), PengaturanWebsiteState.Exit },
                    
                    // dari GeneralSettings
                    { (PengaturanWebsiteState.GeneralSettings, PengaturanWebsiteEvent.Save), PengaturanWebsiteState.Saving },
                    { (PengaturanWebsiteState.GeneralSettings, PengaturanWebsiteEvent.Back), PengaturanWebsiteState.MainMenu },
                    
                    // dari ContentSettings
                    { (PengaturanWebsiteState.ContentSettings, PengaturanWebsiteEvent.Save), PengaturanWebsiteState.Saving },
                    { (PengaturanWebsiteState.ContentSettings, PengaturanWebsiteEvent.Back), PengaturanWebsiteState.MainMenu },
                    
                    // dari Saving
                    { (PengaturanWebsiteState.Saving, PengaturanWebsiteEvent.Back), PengaturanWebsiteState.MainMenu },
                };
            }

            // method untuk melakukan transisi
            public bool MoveNext(PengaturanWebsiteEvent evt)
            {
                if (_transitions.TryGetValue((CurrentState, evt), out PengaturanWebsiteState nextState))
                {
                    CurrentState = nextState;
                    return true;
                }

                return false;
            }

            // reset state machine ke state awal
            public void Reset()
            {
                CurrentState = PengaturanWebsiteState.MainMenu;
            }
        }
    }
}
