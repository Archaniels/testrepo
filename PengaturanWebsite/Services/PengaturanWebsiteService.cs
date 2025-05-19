using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUBES_KPL.PengaturanWebsite.Config;
using static TUBES_KPL.PengaturanWebsite.Model.PengaturanWebsiteModel;

namespace TUBES_KPL.PengaturanWebsite.Services
{
    public class PengaturanWebsiteService
    {
        private readonly PengaturanWebsiteAutomata _automata;
        private readonly PengaturanWebsiteConfig _config;

        public PengaturanWebsiteService()
        {
            _automata = new PengaturanWebsiteAutomata();
            _config = PengaturanWebsiteConfig.Instance;
        }

        public void RunPengaturanWebsite()
        {
            bool status = true;

            while (status)
            {
                Console.Clear();
                DisplayHeader();

                switch (_automata.CurrentState)
                {
                    case PengaturanWebsiteState.MainMenu:
                        status = HandleMainMenu();
                        break;
                    case PengaturanWebsiteState.GeneralSettings:
                        HandleGeneralSettings();
                        break;
                    case PengaturanWebsiteState.ContentSettings:
                        HandleContentSettings();
                        break;
                    case PengaturanWebsiteState.Saving:
                        SaveSettings();
                        _automata.MoveNext(PengaturanWebsiteEvent.Back);
                        break;
                    case PengaturanWebsiteState.Exit:
                        status = false;
                        break;
                }

                if (_automata.CurrentState != PengaturanWebsiteState.Exit && _automata.CurrentState != PengaturanWebsiteState.MainMenu)
                {
                    Console.WriteLine("\nTekan Enter untuk melanjutkan...");
                    Console.ReadLine();
                }
            }
        }

        private void DisplayHeader()
        {
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║           Pengaturan Website               ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        private void DisplayMainMenu()
        {
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║              MENU PENGATURAN               ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine("1. Pengaturan Umum");
            Console.WriteLine("2. Pengaturan Tampilan");
            Console.WriteLine("3. Pengaturan Konten");
            Console.WriteLine("4. Pengaturan Performa");
            Console.WriteLine("0. Kembali");
            Console.WriteLine();
        }

        private bool HandleMainMenu()
        {
            DisplayMainMenu();
            Console.Write("Pilihan Anda: ");
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1":
                    _automata.MoveNext(PengaturanWebsiteEvent.SelectGeneral);
                    return true;
                case "2":
                    _automata.MoveNext(PengaturanWebsiteEvent.SelectContent);
                    return true;
                case "0":
                    _automata.MoveNext(PengaturanWebsiteEvent.Quit);
                    return false;
                default:
                    Console.WriteLine("Pilihan tidak valid! Tekan Enter untuk melanjutkan...");
                    Console.ReadLine();
                    return true;
            }
        }

        private void HandleGeneralSettings()
        {
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║           PENGATURAN UMUM                  ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine($"1. Nama Website: {_config.WebsiteName}");
            Console.WriteLine($"2. Mode Maintenance: {(_config.MaintenanceMode ? "Aktif" : "Tidak Aktif")}");
            Console.WriteLine("3. Simpan Perubahan");
            Console.WriteLine("0. Kembali");

            Console.Write("\nPilihan Anda: ");
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1":
                    Console.Write("Masukkan nama website baru: ");
                    string newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        _config.WebsiteName = newName;
                        Console.WriteLine("Nama website berhasil diubah!");
                    }
                    break;
                case "2":
                    _config.MaintenanceMode = !_config.MaintenanceMode;
                    Console.WriteLine($"Mode Maintenance: {(_config.MaintenanceMode ? "Aktif" : "Tidak Aktif")}");
                    break;
                case "3":
                    _automata.MoveNext(PengaturanWebsiteEvent.Save);
                    break;
                case "0":
                    _automata.MoveNext(PengaturanWebsiteEvent.Back);
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    break;
            }
        }

        private void HandleContentSettings()
        {
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║           PENGATURAN KONTEN                ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine($"1. Deskripsi Website: {_config.WebsiteDescription}");
            Console.WriteLine("2. Simpan Perubahan");
            Console.WriteLine("0. Kembali");

            Console.Write("\nPilihan Anda: ");
            string pilihan = Console.ReadLine();

            switch (pilihan)
            {
                case "1":
                    Console.Write("Masukkan deskripsi website baru: ");
                    string newDesc = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newDesc))
                    {
                        _config.WebsiteDescription = newDesc;
                        Console.WriteLine("Deskripsi website berhasil diubah!");
                    }
                    break;
                case "2":
                    _automata.MoveNext(PengaturanWebsiteEvent.Save);
                    break;
                case "0":
                    _automata.MoveNext(PengaturanWebsiteEvent.Back);
                    break;
                default:
                    Console.WriteLine("Pilihan tidak valid!");
                    break;
            }
        }

        private void SaveSettings()
        {
            Console.WriteLine("Menyimpan pengaturan...");
            PengaturanWebsiteConfig.SaveConfiguration(_config);
            Console.WriteLine("Pengaturan berhasil disimpan!");
        }
    }
}
