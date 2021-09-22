using NoitaTool.Extensions;
using NoitaTool.Model;
using NoitaTool.Outputs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace NoitaTool.Helpers
{
    public static class NoitaBackupHelper
    {
        //this limit is so it doesn't use number 6 and up. Feel free to change the Menu.ConsistentResponses to other none number characters if you want to make this a higher number.
        //Also note I'm not account for multiple inputs, so if you want to go above 9 you'll have to do some more work.
        public static int CustomSaveLimit = 9;

        private static string customBackupsPath = $"{AdditionalInfo.GamePath}\\Custom Saves";
        private static string archiveBackupsPath = $"{AdditionalInfo.GamePath}\\Archive";

        /// <summary>
        /// Backup the Noita Save folder to another folder. Creates the target folder if it doesn't exist.
        /// </summary>
        /// <param name="SaveToFolderPath"></param>
        public static void BackupSave(string SaveToFolderPath)
        {
            OutputHelper.ClearBuffer();

            Console.WriteLine("-> Starting Backup ...");

            if (!Directory.Exists(SaveToFolderPath))
            {
                Directory.CreateDirectory(SaveToFolderPath);
            }

            DirectoryInfo MainSaveDir = new DirectoryInfo(AdditionalInfo.MainSavePath);
            DirectoryInfo saveToDir = new DirectoryInfo(SaveToFolderPath);

            if (MainSaveDir.Exists)
            {
                Console.WriteLine($"-> Saving to '{saveToDir.Name}'");
                MainSaveDir.Copy(SaveToFolderPath, true);
            }

            Console.WriteLine("-> Done");

            Thread.Sleep(2000);
        }

        public static void DeleteSave(string SaveFolderPath, bool Sleep = true, bool ClearBuffer = true)
        {
            if (ClearBuffer)
            {
                OutputHelper.ClearBuffer();
            }

            Console.WriteLine("-> Starting Delete ...");

            if (!Directory.Exists(SaveFolderPath))
            {
                Console.WriteLine("Save doesn't exist");
                Thread.Sleep(2000);
                return;
            }
            
            Directory.Delete(SaveFolderPath, true);

            if (Directory.Exists(SaveFolderPath))
            {
                Console.WriteLine("-> Save appears to exist still");
            }
            else
            {
                Console.WriteLine("-> Save Removed");
            }

            if (Sleep)
            {
                Thread.Sleep(2000);
            }
        }

        public static void BackupNewCustomSave(int index)
        {
            OutputHelper.ClearBuffer();
            Console.Write($"Enter a name for backup {index+1}: ");
            string newName = Console.ReadLine();

            if(!String.IsNullOrEmpty(newName))
            {
                string backupPath = $"{customBackupsPath}\\{index}_{newName}";
                BackupSave(backupPath);
            }
        }

        public static List<CustomBackup> GetCustomBackups()
        {
            List<CustomBackup> customBackups = new List<CustomBackup>();

            if (Directory.Exists(customBackupsPath))
            {
                //Get custom backup folders
                var dirFolders = new DirectoryInfo(customBackupsPath).GetDirectories();

                if (dirFolders != null)
                {
                    foreach (DirectoryInfo dir in dirFolders)
                    {
                        int i;
                        if (int.TryParse(dir.Name[0].ToString(), out i) && dir.Name[1].ToString() == "_")
                        {
                            if (i < CustomSaveLimit)
                            {
                                customBackups.Add(new CustomBackup(dir.Name.Substring(2), i, dir.FullName));
                            }
                        }
                    }
                }
            }

            for(int x = 0; x < NoitaBackupHelper.CustomSaveLimit; x++)
            {
                if(customBackups.Where(y => y.Index == x).Count() == 0)
                {
                    //If the index for this slot doesn't exist, add an empty custom backup.
                    customBackups.Add(new CustomBackup($"<Slot {x+1}>", x, "", true));
                }
            }

            return customBackups;
        }

        public static void RestoreSave(string RestoreFromFolderPath)
        {
            OutputHelper.ClearBuffer();

            Console.WriteLine("-> Starting Restore ...");

            if (!Directory.Exists(RestoreFromFolderPath))
            {
                Console.WriteLine("Save doesn't exist");
                Thread.Sleep(2000);
                return;
            }

            if(!Directory.Exists(AdditionalInfo.MainSavePath))
            {
                Directory.CreateDirectory(AdditionalInfo.MainSavePath);
            }

            DirectoryInfo MainSaveDir = new DirectoryInfo(AdditionalInfo.MainSavePath);
            DirectoryInfo restoreFromDir = new DirectoryInfo(RestoreFromFolderPath);

            if (MainSaveDir.Exists && restoreFromDir.Exists)
            {
                restoreFromDir.Copy(MainSaveDir.FullName, true);
            }

            Console.WriteLine("-> Done");

            Thread.Sleep(2000);
        }

        public static void ArchiveBackup(string BackupToArchive)
        {
            OutputHelper.ClearBuffer();
            Console.WriteLine("Starting Archive to:");
            string timestamp = DateTime.Now.ToString("M-dd-yyyy_HH-mm-ss");

            if(!Directory.Exists(archiveBackupsPath))
            {
                Directory.CreateDirectory(archiveBackupsPath);
            }

            if(Directory.Exists(BackupToArchive))
            {
                DirectoryInfo backupDir = new DirectoryInfo(BackupToArchive);

                string archivePath = $"{archiveBackupsPath}\\{backupDir.Name}_{timestamp}";
                Console.WriteLine(archivePath);
                backupDir.Copy(archivePath, true);

                if(Directory.Exists(archivePath))
                {
                    DeleteSave(BackupToArchive, false, false);
                }
                else
                {
                    Console.WriteLine("Archiving Failed");
                }
            }

            Console.WriteLine("-> Done");
            Thread.Sleep(2000);
        }
    }
}
