using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using FlaUI.Core;
using FlaUI.UIA3;
using FlaUI.Core.Identifiers;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using FlaUI.Core.Conditions;
using System.Threading;
namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public static class TestMethod
    {

        public static void ActionSelectComboBoxItem(this AutomationElement comboBoxElement, int indexToSelect)
        {
            if (comboBoxElement == null)
                throw new Exception("Combo Box not found");

            //Get the all the list items in the ComboBox


            //Expand the combobox
            ExpandCollapsePattern expandPattern = (ExpandCollapsePattern)comboBoxElement.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            expandPattern.Expand();
            AutomationElementCollection comboboxItem = comboBoxElement.FindAll(TreeScope.Children, new System.Windows.Automation.PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));

            //Index to set in combo box
            AutomationElement itemToSelect = comboboxItem[indexToSelect];

            //Finding the pattern which need to select
            SelectionItemPattern selectPattern = (SelectionItemPattern)itemToSelect.GetCurrentPattern(SelectionItemPattern.Pattern);
            selectPattern.Select();
        }
        #region 1st Way to select combobox item
        //public static void SetSelectedComboBoxItem(this AutomationElement comboBox, string item)
        //{
        //    AutomationPattern automationPatternFromElement = GetSpecifiedPattern(comboBox, "ExpandCollapsePatternIdentifiers.Pattern");

        //    ExpandCollapsePattern expandCollapsePattern = comboBox.GetCurrentPattern(automationPatternFromElement) as ExpandCollapsePattern;

        //    expandCollapsePattern.Expand();
        //    expandCollapsePattern.Collapse();

        //    AutomationElement listItem = comboBox.FindFirst(TreeScope.Subtree, new System.Windows.Automation.PropertyCondition(AutomationElement.NameProperty, item));

        //    automationPatternFromElement = GetSpecifiedPattern(listItem, "SelectionItemPatternIdentifiers.Pattern");

        //    SelectionItemPattern selectionItemPattern = listItem.GetCurrentPattern(automationPatternFromElement) as SelectionItemPattern;

        //    selectionItemPattern.Select();
        //}
        #endregion
        #region 2nd Way to select combobox item
        public static void SetSelectedComboBoxItem(this AutomationElement comboBoxElement, string item)
        {
            if (comboBoxElement == null)
                throw new Exception("Combo Box not found");

            //Get the all the list items in the ComboBox


            //Expand the combobox
            ExpandCollapsePattern expandPattern = (ExpandCollapsePattern)comboBoxElement.GetCurrentPattern(ExpandCollapsePattern.Pattern);
            expandPattern.Expand();
            expandPattern.Collapse();
            AutomationElementCollection comboboxItem = comboBoxElement.FindAll(TreeScope.Subtree, new System.Windows.Automation.PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
            int i = 0;
            //try to get patterns
            //foreach(AutomationElement cbcItem in comboboxItem)
            //{
            //    foreach (AutomationPattern ap in cbcItem.GetSupportedPatterns())
            //    {
            //        MessageBox.Show(ap.ProgrammaticName);

            //    }
            //}
            foreach (AutomationElement cbxItem in comboboxItem)
            {
                if (cbxItem.FindFirst(TreeScope.Children, System.Windows.Automation.Condition.TrueCondition).Current.Name == item)
                {
                    break;
                }
                i++;
            }
            //Index to set in combo box
            AutomationElement itemToSelect = comboboxItem[i];

            //Finding the pattern which need to select
            SelectionItemPattern selectPattern = (SelectionItemPattern)itemToSelect.GetCurrentPattern(SelectionItemPattern.Pattern);
            selectPattern.Select();
        }
        #endregion
        public static AutomationPattern GetSpecifiedPattern(this AutomationElement element, string patternName)
        {
            AutomationPattern[] supportedPattern = element.GetSupportedPatterns();

            foreach (AutomationPattern pattern in supportedPattern)
            {
                if (pattern.ProgrammaticName == patternName)
                    return pattern;
            }

            return null;
        }

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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            #region UIAutomation
            //AutomationElement target = null;
            //AutomationElementCollection automationCollection = AutomationElement.RootElement.FindAll(TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
            //foreach (AutomationElement automation in automationCollection)
            //{
            //    if (automation.Current.Name == "TestForm")// sửa lại thành cửa sổ đang cần spy
            //    {
            //        target = automation;
            //        break;
            //    }

            //}
            //Process[] flexproc = Process.GetProcessesByName("WpfApp1");// Sửa lại tên app
            //TestMethod.SetForegroundWindow(flexproc[0].MainWindowHandle);
            //Thread.Sleep(1000);
            //var automationlist = TestMethod.GetAllDescendants(target);
            //int i = 0;
            //foreach (AutomationElement a in automationlist)
            //{
            //    listBox.Items.Add(i+a.Current.AutomationId+"_"+a.Current.Name+"_"+a.Current.ControlType.LocalizedControlType);
            //    if (a.Current.AutomationId == "testCmb")// sửa lại thành PersonalCountryCmb
            //    //if (i == 63)
            //    {


            //        try
            //        {
            //            //a.ActionSelectComboBoxItem(2);
            //            ExpandCollapsePattern expandCollapsePattern = a.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            //            expandCollapsePattern.Expand();
            //            var comboBoxEditItemCondition = new System.Windows.Automation.PropertyCondition(AutomationElement.ClassNameProperty, "ComboBoxEditItem");
            //            var listItems = a.FindAll(TreeScope.Subtree, comboBoxEditItemCondition);//It's can only get one item in the list (the first one).
            //            var testItem = listItems[4];
            //            (testItem.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Select();
            //            expandCollapsePattern.Collapse();

            //        }
            //        catch (Exception error)
            //        {
            //            MessageBox.Show(error.Message);
            //        }
            //    }
            //    i++;
            //}
            #endregion
            //var items = a.FindAll(TreeScope.Descendants, new System.Windows.Automation.PropertyCondition(AutomationElement.AutomationIdProperty, "PART_Item"));
            //var items = a.FindAll(TreeScope.Descendants, System.Windows.Automation.Condition.TrueCondition);
            //foreach (AutomationElement item in items)
            //{

            //}
            //TestMethod.SelectValueInComboBox(a,"Russia");
            #region FlaUI
            //Process[] proc = Process.GetProcessesByName("FlexBARMS");



            //var app = FlaUI.Core.Application.Attach(proc[0].Id);
            //using (var automation = new UIA3Automation())
            //{
            //    var MainWindow = app.GetMainWindow(automation);
            //    Console.WriteLine(MainWindow.Title);



            //    var elementArray = MainWindow.FindAllDescendants(new FlaUI.Core.Conditions.PropertyCondition(automation.PropertyLibrary.Element.FrameworkId, "WPF"));

            //    MainWindow.Focus();
            //    var testCbx = MainWindow.FindFirstDescendant(new FlaUI.Core.Conditions.PropertyCondition(automation.PropertyLibrary.Element.AutomationId, "PersonalCountryCmb"));


            //    int i = 0;
            //    foreach (FlaUI.Core.AutomationElements.Infrastructure.AutomationElement element in elementArray)
            //    {



            //        if (element.ControlType.ToString() != "ScrollBar")
            //        {
            //            if (element.AutomationId.ToString() != "")
            //                listBox.Items.Add(element.AutomationId.ToString() + "_" + element.ControlType.ToString().Replace(" ", "_"));
            //            else if (element.AutomationId.ToString() == "")
            //            {
            //                if (element.Name.ToString() != "")
            //                    listBox.Items.Add(element.Name.ToString().Replace(" ", "_") + "_" + element.ControlType.ToString().Replace(" ", "_"));
            //                else
            //                    try
            //                    {
            //                        listBox.Items.Add(elementArray[i - 1].Name.ToString().Replace(" ", "_") + "_" + element.ControlType.ToString().Replace(" ", "_"));
            //                    }
            //                    catch (Exception ex)
            //                    {
            //                        listBox.Items.Add(ex.Message);
            //                    }

            //            }

            //            //element.DrawHighlight();
            //        }
            //        i++;
            //    }
            //}
            #endregion

            #region GuUI
            //Process[] proc = Process.GetProcessesByName("WpfApp1");
            //var app = Gu.Wpf.UiAutomation.Application.Attach(proc[0].Id);
            //var mainWindow = app.MainWindow;
            //var elements=mainWindow.FindAll(TreeScope.Descendants, System.Windows.Automation.Condition.TrueCondition);
            //foreach (Gu.Wpf.UiAutomation.UiElement a in elements)
            //{
  
            //    if (a.AutomationId == "testCmb")// sửa lại thành PersonalCountryCmb
            //    //if (i == 63)
            //    {


            //        try
            //        {
            //            MessageBox.Show(a.AutomationElement.Current.IsEnabled.ToString());
            //            //a.ActionSelectComboBoxItem(2);
            //            ExpandCollapsePattern expandCollapsePattern = a.AutomationElement.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
            //            expandCollapsePattern.Expand();
                        
            //            var comboBoxEditItemCondition = new System.Windows.Automation.PropertyCondition(AutomationElement.ClassNameProperty, "ComboBoxEditItem");
            //            var listItems = a.FindAll(TreeScope.Subtree, comboBoxEditItemCondition);//It's can only get one item in the list (the first one).
            //            var testItem = listItems[4];
            //            (testItem.AutomationElement.GetCurrentPattern(SelectionItemPattern.Pattern) as SelectionItemPattern).Select();
            //            expandCollapsePattern.Collapse();

            //        }
            //        catch (Exception error)
            //        {
            //            MessageBox.Show(error.Message);
            //        }
            //    }
            //}
            #endregion
        }
    }
}


