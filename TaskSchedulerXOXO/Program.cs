using System;
using System.IO;
using Microsoft.Win32.TaskScheduler;

class Program
{
    static void Main(string[] args)
    {
        // Path to the executable file you want to move
        string sourceFilePath = @"C:\path\to\your\executable.exe";
        // Destination folder
        string destinationFolder = @"C:\desired\folder";

        // Move the file
        string destinationFilePath = Path.Combine(destinationFolder, Path.GetFileName(sourceFilePath));
        File.Move(sourceFilePath, destinationFilePath);

        // Create a task scheduler
        using (TaskService ts = new TaskService())
        {
            TaskDefinition td = ts.NewTask();
            td.RegistrationInfo.Description = "Run my executable at startup";

            // Set trigger to start at logon
            td.Triggers.Add(new LogonTrigger());

            // Set the action to start the executable
            td.Actions.Add(new ExecAction(destinationFilePath, null, null));

            // Register the task in the root folder
            ts.RootFolder.RegisterTaskDefinition("MyExecutableAtStartup", td);
        }

        Console.WriteLine("Executable moved and scheduled task created.");
    }
}
