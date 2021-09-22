using NoitaTool.Helpers;
using NoitaTool.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NoitaTool.Outputs
{
    public static class Menus
    {
        #region Main Menu
        public static Menu MainMenu()
        {
            Menu mainMenu = new Menu();
            mainMenu.Title = "Main Menu";

            mainMenu.Options.Add("Backup a Save", () =>
            {
                BackupMenu().ShowMenu();
            });

            mainMenu.Options.Add("Restore a Save", () =>
            {
                RestoreMenu().ShowMenu();
            });

            mainMenu.Options.Add("Delete a Save", () =>
            {
                DeleteMenu().ShowMenu();
            });

            mainMenu.Options.Add("Archive a Save", () =>
            {
                ArchiveMenu().ShowMenu();
            });

            mainMenu.Options.Add("Open Noita Folder", () =>
            {
                Process.Start("explorer.exe", AdditionalInfo.GamePath);
            });

            return mainMenu;
        }
        #endregion

        #region Backup Menu
        public static Menu BackupMenu()
        {
            Menu backupMenu = new Menu();
            backupMenu.Title = "Backup Menu";
            backupMenu.Options.Add("Backup to Main Save", () =>
            {
                if (OutputHelper.GetCenteredConfirmation("Backup to Main Save?"))
                {
                    string backupPath = $"{AdditionalInfo.GamePath}\\Main Save";
                    NoitaBackupHelper.BackupSave(backupPath);
                }

                BackupMenu().ShowMenu();
            });

            backupMenu.Options.Add("Backup with Custom Name", () =>
            {
                OutputHelper.ClearBuffer();
                Menu customBackupsMenu = new Menu();
                customBackupsMenu.Title = "Backup Custom Backup";
                
                List<CustomBackup> customBackups = NoitaBackupHelper.GetCustomBackups();

                
                for (int x = 0; x < NoitaBackupHelper.CustomSaveLimit; x++)
                {
                    CustomBackup cb = customBackups.Where(y => y.Index == x).FirstOrDefault();

                    if(cb != null)
                    {
                        if (cb.IsEmpty)
                        {
                            customBackupsMenu.Options.Add(cb.Name, () =>
                            {
                                NoitaBackupHelper.BackupNewCustomSave(cb.Index);
                                BackupMenu().ShowMenu();
                            });
                        }
                        else
                        {
                            customBackupsMenu.Options.Add(cb.Name, () =>
                            {
                                if (OutputHelper.GetCenteredConfirmation($"Overwrite backup '{cb.Name}'?"))
                                {
                                    NoitaBackupHelper.BackupSave(cb.FullPath);
                                }

                                BackupMenu().ShowMenu();
                            });
                        }
                    }
                }

                customBackupsMenu.ShowMenu();
            });

            return backupMenu;
        }
#endregion

        #region Restore Menu
        public static Menu RestoreMenu()
        {
            Menu restoreMenu = new Menu();
            restoreMenu.Title = "Restore Menu";

            restoreMenu.Options.Add("Restore Main Save", () =>
            {
                if (OutputHelper.GetCenteredConfirmation("Restore Main Save?"))
                {
                    string backupPath = $"{AdditionalInfo.GamePath}\\Main Save";
                    NoitaBackupHelper.RestoreSave(backupPath);
                }

                RestoreMenu().ShowMenu();
            });

            restoreMenu.Options.Add("Restore Custom Save", () =>
            {
                OutputHelper.ClearBuffer();
                Menu customBackupsMenu = new Menu();
                customBackupsMenu.Title = "Restore Custom Backup";

                List<CustomBackup> customBackups = NoitaBackupHelper.GetCustomBackups();


                for (int x = 0; x < NoitaBackupHelper.CustomSaveLimit; x++)
                {
                    CustomBackup cb = customBackups.Where(y => y.Index == x).FirstOrDefault();

                    if (cb != null)
                    {
                        if (cb.IsEmpty)
                        {
                            customBackupsMenu.Options.Add(cb.Name, () => 
                            {
                                RestoreMenu().ShowMenu();
                            });
                        }
                        else
                        {
                            customBackupsMenu.Options.Add(cb.Name, () =>
                            {
                                if (OutputHelper.GetCenteredConfirmation($"Restore save '{cb.Name}'?"))
                                {
                                    NoitaBackupHelper.RestoreSave(cb.FullPath);
                                }

                                RestoreMenu().ShowMenu();
                            });
                        }
                    }
                }

                customBackupsMenu.ShowMenu();
            });

            return restoreMenu;
        }
        #endregion

        #region Delete Menu
        public static Menu DeleteMenu()
        {
            Menu deleteMenu = new Menu();
            deleteMenu.Title = "Delete Menu";

            deleteMenu.Options.Add("Delete Main Save", () =>
            {
                if (OutputHelper.GetCenteredConfirmation("Delete Main Save?"))
                {
                    string backupPath = $"{AdditionalInfo.GamePath}\\Main Save";
                    NoitaBackupHelper.DeleteSave(backupPath);
                }

                DeleteMenu().ShowMenu();
            });

            deleteMenu.Options.Add("Delete Custom Save", () =>
            {
                OutputHelper.ClearBuffer();
                Menu customBackupsMenu = new Menu();
                customBackupsMenu.Title = "Delete Custom Backup";

                List<CustomBackup> customBackups = NoitaBackupHelper.GetCustomBackups();


                for (int x = 0; x < NoitaBackupHelper.CustomSaveLimit; x++)
                {
                    CustomBackup cb = customBackups.Where(y => y.Index == x).FirstOrDefault();

                    if (cb != null)
                    {
                        if (cb.IsEmpty)
                        {
                            customBackupsMenu.Options.Add(cb.Name, () => 
                            {
                                DeleteMenu().ShowMenu();
                            });
                        }
                        else
                        {
                            customBackupsMenu.Options.Add(cb.Name, () =>
                            {
                                if (OutputHelper.GetCenteredConfirmation($"Delete Save '{cb.Name}'?"))
                                {
                                    NoitaBackupHelper.DeleteSave(cb.FullPath);
                                }

                                DeleteMenu().ShowMenu();
                            });
                        }
                    }
                }

                customBackupsMenu.ShowMenu();
            });


            return deleteMenu;
        }
        #endregion

        #region Archive Menu
        public static Menu ArchiveMenu()
        {
            Menu archiveMenu = new Menu();
            archiveMenu.Title = "Archive Menu";

            archiveMenu.Options.Add("Archive Main Save", () =>
            {
                string backupPath = $"{AdditionalInfo.GamePath}\\Main Save";
                if (OutputHelper.GetCenteredConfirmation("Archive Main Save?"))
                {
                    NoitaBackupHelper.ArchiveBackup(backupPath);
                }
            });

            archiveMenu.Options.Add("Archive Custom Backup", () =>
            {
                OutputHelper.ClearBuffer();
                Menu customBackupsMenu = new Menu();
                customBackupsMenu.Title = "Archive Custom Backup";

                List<CustomBackup> customBackups = NoitaBackupHelper.GetCustomBackups();


                for (int x = 0; x < NoitaBackupHelper.CustomSaveLimit; x++)
                {
                    CustomBackup cb = customBackups.Where(y => y.Index == x).FirstOrDefault();

                    if (cb != null)
                    {
                        if (cb.IsEmpty)
                        {
                            customBackupsMenu.Options.Add(cb.Name, () =>
                            {
                                ArchiveMenu().ShowMenu();
                            });
                        }
                        else
                        {
                            customBackupsMenu.Options.Add(cb.Name, () =>
                            {
                                if (OutputHelper.GetCenteredConfirmation($"Archive Backup {cb.Name}?"))
                                {
                                    NoitaBackupHelper.ArchiveBackup(cb.FullPath);
                                }

                                ArchiveMenu().ShowMenu();
                            });
                        }
                    }
                }

                customBackupsMenu.ShowMenu();
            });

            return archiveMenu;
        }
        #endregion
    }
}
