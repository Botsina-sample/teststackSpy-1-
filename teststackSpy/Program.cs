using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace teststackSpy
{
    static class TestMethod
    {
        public static List<AutomationElement> GetAllDescendants(this AutomationElement element, int depth = 0, int maxDepth = 4)
        {
            var allChildren = new List<AutomationElement>();

            if (depth > maxDepth)
            {
                return allChildren;
            }

            AutomationElement sibling = TreeWalker.RawViewWalker.GetFirstChild(element);

            while (sibling != null)
            {
                allChildren.Add(sibling);
                allChildren.AddRange(sibling.GetAllDescendants(depth + 1, maxDepth));
                sibling = TreeWalker.RawViewWalker.GetNextSibling(sibling);
            }

            return allChildren;
        }
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }

    class Program
    {
      
        static void Main(string[] args)
        {
          
            AutomationElement target=null;
            AutomationElementCollection automationCollection = AutomationElement.RootElement.FindAll(TreeScope.Children, Condition.TrueCondition);
            foreach(AutomationElement automation in automationCollection)
            {
                if (automation.Current.Name == "Hệ Thống Quản Lý Bán Lẻ")// sửa lại thành cửa sổ đang cần spy
                {
                    target = automation;
                    break;
                }
           
            }
            Process []flexproc = Process.GetProcessesByName("FlexBARMS");
            TestMethod.SetForegroundWindow(flexproc[0].MainWindowHandle);
            var automationlist=TestMethod.GetAllDescendants(target);
            foreach(AutomationElement a in automationlist)
            {
                if (a.Current.AutomationId != "")
                {
                    Console.WriteLine(a.Current.AutomationId);  
                }
                  
            }
        }
    }
}
