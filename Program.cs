/*
 * Program.cs
 * 
 * Program Main
 * 
 * Reference:
 * Translated into Google Translator.
 * 
 * Copyright (c) jungwook(hjaas5397@naver.com). All rights reserved.
 */

using System;
using System.IO;
using System.Security.Principal;
using System.Text;

namespace RansomMineDecrypter
{
    class Program
    {
        static void Main(string[] args) // Program Start Point
        {
            Console.Title = "RansomMineDecrypter";

            // Whether administrator privileges are enabled
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);

            if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
            {
                Console.WriteLine("It must be run as an administrator.");
                Console.ReadKey();
                return;
            }

            String DecryptPath = "";

            // commandLine
            if (args.Length != 0)
            {
                DecryptPath = args[0];

                if (!Directory.Exists(DecryptPath)) // Folder Presence
                {
                    Console.WriteLine(DecryptPath + "\nSome of the paths can not be found or their paths do not exist.");
                    Console.ReadKey();
                    return;
                }
            } else
            {
                DecryptPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            Console.WriteLine("=======================================");
            Console.WriteLine("■        RansomMineDecrypter        ■");
            Console.WriteLine("Copyright (c) jungwook(hjaas5397@naver.com). All rights reserved.");
            Console.WriteLine("=======================================");
            Console.WriteLine();
            Console.WriteLine("Reference: Translated into Google Translator.");
            Console.WriteLine();
            Console.WriteLine("Warning: If you use AppCheck, please turn off realtime inspection. There may be problems in the process of Decrypting.");
            Console.WriteLine();
            Console.WriteLine("Set Path: " + DecryptPath + " (See the command line .txt for change.)");
            Console.WriteLine();
            home:
            Console.Write("Do you want to start decrypting?(N/Y): ");
            String cos = Console.ReadLine();

            switch (cos)
            {
                case "N":
                case "n":
                case "NO":
                case "no":
                    return;

                case "Y":
                case "y":
                case "YES":
                case "yes":
                    break;

                default:
                    goto home;
            }

            Console.WriteLine();
            Console.WriteLine("Decryption Key: " + ProgramVar.ProgramKey);
            Console.WriteLine();

            FileLog log = new FileLog("RansomMineDecrypter_Log.txt");
            log.WriteLine("**************************************************");

            StartDecrypter(DecryptPath);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("File Decrypted!");
            log.WriteLine("File Decrypted!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

        static void StartDecrypter(string aPath) // Decryption function
        {
            FileLog log = new FileLog("RansomMineDecrypter_Log.txt");

            try
            {
                foreach (String Fil in Directory.GetFiles(aPath, "*.RansomMine"))
                {
                    try
                    {
                        byte[] FileData = File.ReadAllBytes(Fil);
                        byte[] DecrypterData = AES256.AES_Decrypt(FileData, ProgramVar.ProgramKey);
                        File.WriteAllBytes(Fil, DecrypterData);

                        StringBuilder ssss = new StringBuilder(Fil);
                        ssss.Replace(".RansomMine", "");

                        File.Move(Fil, ssss.ToString());
                        Console.WriteLine(ssss.ToString() + " - OK");
                        log.WriteLine(ssss.ToString() + " - OK");
                    } catch (Exception e)
                    {
                        Console.WriteLine(Fil + " - Failed: " + e.Message);
                        log.WriteLine(Fil + " - Failed: " + e.Message);
                    }
                }
            }catch(Exception e)
            {
                log.WriteLine("File Search error: " + e.Message);
            }

            try
            {
                foreach (String Dirs in Directory.GetDirectories(aPath))
                {
                    log.WriteLine("Folder to decrypt: " + Dirs);
                    StartDecrypter(Dirs);
                }
            }
            catch (Exception e)
            {
                log.WriteLine("Folder Search error: " + e.Message);
            }
        }
    }
}
