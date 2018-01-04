using FlaUI.Core;
using FlaUI.Core.Conditions;
using FlaUI.UIA3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {

            Process[] proc = Process.GetProcessesByName("FlexBARMS");

            

            var app = Application.Attach(proc[0].Id);
            using (var automation = new UIA3Automation())
            {
                var MainWindow = app.GetMainWindow(automation);
                Console.WriteLine(MainWindow.Title);


                int i = 0;
                var elementArray= MainWindow.FindAllDescendants(new PropertyCondition(automation.PropertyLibrary.Element.FrameworkId, "WPF"));

                MainWindow.Focus();

                foreach(FlaUI.Core.AutomationElements.Infrastructure.AutomationElement element in elementArray)
                {
                    
                    Console.WriteLine(i + " - " + element);


                    

                    try
                    {
                        element.CaptureToFile(element.ClassName + " - " +  + i + ".png");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                    if (i == 90)
                    {
                        
                        element.DrawHighlight();
                        try
                        {
                            MainWindow.CaptureToFile("hooker.png");

                        } catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    i++;
                }


            }
        }
    }
}
