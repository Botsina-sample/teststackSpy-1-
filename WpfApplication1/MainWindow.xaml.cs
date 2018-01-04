﻿using System;
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
        public static void SetSelectedComboBoxItem(this AutomationElement comboBox, string item)
        {
            AutomationPattern automationPatternFromElement = GetSpecifiedPattern(comboBox, "ExpandCollapsePatternIdentifiers.Pattern");

            ExpandCollapsePattern expandCollapsePattern = comboBox.GetCurrentPattern(automationPatternFromElement) as ExpandCollapsePattern;

            expandCollapsePattern.Expand();
            expandCollapsePattern.Collapse();

            AutomationElement listItem = comboBox.FindFirst(TreeScope.Subtree, new System.Windows.Automation.PropertyCondition(AutomationElement.NameProperty, item));

            automationPatternFromElement = GetSpecifiedPattern(listItem, "SelectionItemPatternIdentifiers.Pattern");

            SelectionItemPattern selectionItemPattern = listItem.GetCurrentPattern(automationPatternFromElement) as SelectionItemPattern;

            selectionItemPattern.Select();
        }

        public static AutomationPattern GetSpecifiedPattern( this AutomationElement element, string patternName)
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

            #region oldcode
            AutomationElement target = null;
            AutomationElementCollection automationCollection = AutomationElement.RootElement.FindAll(TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
            foreach (AutomationElement automation in automationCollection)
            {
                if (automation.Current.Name == "TestForm")// sửa lại thành cửa sổ đang cần spy
                {
                    target = automation;
                    break;
                }

            }
            Process[] flexproc = Process.GetProcessesByName("WpfApplication1");// Sửa lại tên app
            TestMethod.SetForegroundWindow(flexproc[0].MainWindowHandle);
            Thread.Sleep(1000);
            var automationlist = TestMethod.GetAllDescendants(target);

            foreach (AutomationElement a in automationlist)
            {
                listBox.Items.Add(a.Current.AutomationId);
                if (a.Current.AutomationId == "comboBox")// sửa lại thành PersonalCountryCmb
                {


                    try
                    {
                        a.ActionSelectComboBoxItem(2);
                        Thread.Sleep(1000);
                        a.SetSelectedComboBoxItem("B");

                    }
                    catch (Exception error)
                    {
                        MessageBox.Show(error.Message);
                    }
                }
            }
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
            //                 listBox.Items.Add(element.AutomationId.ToString() + "_" + element.ControlType.ToString().Replace(" ", "_"));
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
            //Process[] proc = Process.GetProcessesByName("FlexBARMS");
            //var app = Gu.Wpf.UiAutomation.Application.Attach(proc[0].Id);

            #endregion
        }
    }
}
