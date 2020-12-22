//BDD EB Guide Automate Testing
//Color picker
//Pawel Szelag
//Student Development Program - Quality & Assurance

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.Core.AutomationElements.Infrastructure;
using FlaUI.Core.Conditions;
using FlaUI.Core.Definitions;
using FlaUI.Core.Input;
using FlaUI.UIA3;

using NUnit.Framework;

using TechTalk.SpecFlow;

namespace EBGuideBDDTesttingHomework
{
    //FeatureFilesSteps class responsible for Test steps tracking and handling
    [Binding]
    public class FeatureFilesSteps
    {
        //Fields
        private const string APP_TITLE = "EB GUIDE Studio";
        private static Application _guideApplication;
        private static Window _guideWindow;

        private static AutomationBase _automation;

        //Start method - starting EBGuide Studio
        [Before("SelectSimpleColor")]
        [Before("ChangeColorProperties")]
        public static void Start()
        {
            //UIA3 automation handler
            _automation = new UIA3Automation();

            //Starting process for EBGuide
            //SPECIFY CORRECT EBGUIDE STUDIO PATH
            StartApplication(@"C:\Program Files (x86)\Elektrobit\EB GUIDE 6.7.3\studio\Studio.exe");
            //
            //
        }
        
        //Step - create project
        [Given(@"I opened EGBuide Studio and created new project named (.*)")]
        public void GivenIOpenedEGBuideStudioAndCreatedNewProjectNamedProject(string name)
        {
            //Get window handler
            var window = GetGuidePlainWindow();

            //Find and click new project button
            var newProject = GetControlByClassNameAndAutomationIdForElement(window, "RadioButton", "NewProject");
            Click(newProject);

            //Get window handler
            window = GetGuidePlainWindow();
            
            //Find textBox and enter project name
            var projectNameControl = GetControlByClassNameAndAutomationIdForElement(window, "TextBox", "ProjectNameTextBox");
            EnterText(projectNameControl, name);

            //Get window handler
            _guideWindow = GetGuidePlainWindow();

            //Find and click create button
            var createButton = GetControlByClassNameAndNameForElement(window, "Button", "CREATE");
            createButton.Click();

            //Wait three seconds
            Thread.Sleep(TimeSpan.FromSeconds(3));

            //Find popUpWindow
            var popupWindow = GetPopupWindowWithTitle("Name already exists");
            if (popupWindow != null)
            {
                //Press overwrite if popUp appears
                var overwriteButton = GetControlByClassNameAndNameForElement(popupWindow, "Button", "Overwrite");

                overwriteButton.Click();

                //Wait some time for ready GUI
                GiveEnoughTimeForStartup();

                //Check if window appears
                window = GetApplicationWindowWithTitle("Project1 - EB GUIDE Studio");
                Assert.That(window, Is.Not.Null); 
            }

        }

        //Step - click the color picker button
        [Given(@"I clicked color picker (.*) with automationId (.*)")]
        public void GivenIClickedEventsTabItemNamedXceed_Wpf_AvalonDock_Layout_LayoutAnchorable(string type, string auID)
        {
            //Click color picker method
            ClickColorPicker(_guideWindow, type, auID);
        }

        //Step - selected specyfied color from simple palette
        [Given(@"I selected dark orange color in (.*) with number (.*)")]
        public void GivenISelectedDarkOrangeColorinListBoxWithNumber34(string containerType, string colorNumber)
        {
            //For visual purposes only
            Thread.Sleep(TimeSpan.FromSeconds(2));

            //Find palette of colors and click specyfied index
            var colorItem = GetControlByClassNameAndAutomationId(containerType, "PART_AvailableColors");
            colorItem.FindAllChildren()[Convert.ToInt32(colorNumber)].Click();
        }

        //Step - click Advanced properties in color picker
        [Given(@"I clicked the color properties in (.*) options")]
        public void IClickedTheColorPropertiesInAdvancedOptions(string modeButton)
        {
            //Click color picker again
            ClickColorPicker(_guideWindow, "Button", "PART_ColorPickerToggleButton");

            //Go to the advanced properties
            var modeButt = GetControlByClassNameAndNameForElement(_guideWindow, "Button", modeButton);
            modeButt.Click();

        }

        //Check correctness of hex code
        [Then(@"Check if hex code (.*)")]
        public void CheckIfHexCodeForDarkOrangeIs(string hexCode)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            //Find textBox containing hexCode
            var textBox = GetControlByClassNameAndAutomationId("TextBox", "PART_HexadecimalTextBox");
            
            //Check if hexCode is correct
            Assert.That(textBox.AsTextBox().Text == hexCode ? true : false);

        }

        //Step - Check corectness RGBA values
        [Then(@"Check if RGBA values are correct R - (.*) G - (.*) B - (.*) A - (.*)")]
        public void CheckRGBAValues(string RValue, string GValue, string BValue, string AValue)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            //Find popUp
            var popUp = GetControlByClassNameAndNameForElement(_guideWindow, "Popup", "");

            //Find all RGBA TextBoxes
            var edits = popUp.FindAllChildren();

            //Check RGBA
            CheckRGB(edits, RValue, GValue, BValue, AValue);

        }

        //Step - change properties mode in color picker
        [Given(@"I clicked first time the color properties in (.*) options")]
        public void IClickedFirstTimeTheColorPropertiesInAdvancedOptions(string modeButton)
        {
            //Find and click properties mode
            var modeButt = GetControlByClassNameAndNameForElement(_guideWindow, "Button", modeButton);
            modeButt.Click();

        }

        //Step - enter hexCode into color picker
        [Given(@"I entered hex color value (.*)")]
        public void IEnteredHexColorValue(string hexCode)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            //Find hexCode textBox
            var textBox = GetControlByClassNameAndAutomationId("TextBox", "PART_HexadecimalTextBox");

            EnterText(textBox, hexCode);

            //Check if correctly assigned
            Assert.That(textBox.AsTextBox().Text == hexCode ? true : false);

        }

        //Step - double click the color properties to update the view
        [Given(@"I double clicked the color properties in (.*) options")]
        public void IClickedOnTheToUpdateView(string modeButton)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            //Find properties selection mode button in color picker and click
            var modeButt = GetControlByClassNameAndNameForElement(_guideWindow, "Button", modeButton);
            modeButt.DoubleClick();

        }

        //Step - Change RGBA values
        [Then(@"Change RValue - (.*) GValue - (.*) BValue - (.*) AValue - (.*)")]
        public void ChangeRGBAValues(string RValue, string GValue, string BValue, string AValue)
        {
            var popUp = GetControlByClassNameAndNameForElement(_guideWindow, "Popup", "");
            var edits = popUp.FindAllChildren();

            //Enter texts to the all RGBA textboxes
            EnterText(edits[6], RValue);
            EnterText(edits[8], GValue);
            EnterText(edits[10], BValue);
            EnterText(edits[12], AValue);
            
            //Update view
            var modeButt = GetControlByClassNameAndNameForElement(_guideWindow, "Button", "Standard");
            modeButt.DoubleClick();

            //Check correctness RGBA values
            CheckRGB(edits, RValue, GValue, BValue, AValue);
        }

        //Method responsible for finding the colorPicker instance
        private void ClickColorPicker(AutomationElement window, string type, string auID)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            //Find and click colorPicker
            var pickerButton = GetControlByClassNameAndAutomationIdForElement(_guideWindow, type, auID);
            pickerButton.Click();
        }

        //Method responsible for finding NewProject RadioButton 
        public void GivenIPressTheRadioButtonNewProject(string buttonType, string buttonName)
        {
            var window = GetGuidePlainWindow();
            Assert.That(window, Is.Not.Null);

            //Find newProject button by Class name and AutomationID and click
            var newProject = GetControlByClassNameAndAutomationIdForElement(window, buttonType, buttonName);
            Click(newProject);
        }

        //Click element method
        private void Click(AutomationElement typeToPress)
        {
            Wait.UntilResponsive(typeToPress);
            typeToPress.Click(false);
            Wait.UntilInputIsProcessed();
        }

        //Get application window instance
        private static Window GetGuidePlainWindow()
        {
            return GetApplicationWindowWithTitle(APP_TITLE);
        }

        ////Get application window instance with title
        private static Window GetApplicationWindowWithTitle(string windowTitle)
        {
            var topLevelWindows = _guideApplication.GetAllTopLevelWindows(_automation);
            var window = topLevelWindows.FirstOrDefault(win => win.Title.Equals(windowTitle));
            return window;
        }

        //Get popUp instance with title
        private static Window GetPopupWindowWithTitle(string windowTitle)
        {
            var topLevelWindows = _guideApplication.GetMainWindow(_automation);
            var window = topLevelWindows.ModalWindows.FirstOrDefault(win => win.Title.Equals(windowTitle));
            return window;
        }

        //Check RGBA values with assertions method
        private void CheckRGB(AutomationElement[] edits, string RValue, string GValue, string BValue, string AValue)
        {
            //Check R value
            Assert.That(edits[6].AsTextBox().Text == RValue ? true : false);

            //Check G value
            Assert.That(edits[8].AsTextBox().Text == GValue ? true : false);

            //Check B value
            Assert.That(edits[10].AsTextBox().Text == BValue ? true : false);

            //Check A value
            Assert.That(edits[12].AsTextBox().Text == AValue ? true : false);
        }
        
        //Type project name method
        public void GivenIEnterProjectInTheTextBoxProjectNameTextBox(string projectName)
        {
            var window = GetGuidePlainWindow();
            var projectNameControl = GetControlByClassNameAndAutomationIdForElement(window, "TextBox", "ProjectNameTextBox");
            EnterText(projectNameControl, projectName);
        }
        
        //Click overwrite button when appears method
        public void GivenIPressButtonOverwriteInTheDialogTitledNameAlreadyExistsWhenItPopsUp(string popupName)
        {
            var popupWindow = GetPopupWindowWithTitle(popupName);
            if (popupWindow != null)
            {
                //Find and click overwrite button
                var overwriteButton = GetControlByClassNameAndNameForElement(popupWindow, "Button", "Overwrite");
                overwriteButton.Click();
            }

        }

        //Check if window visible and correctly named
        public void ThenICanSeeAWindowTitledProject_EBGUIDEStudio(string windowName)
        {
            GiveEnoughTimeForStartup();
            var window = GetApplicationWindowWithTitle(windowName);

            //Check if not null
            Assert.That(window, Is.Not.Null);
        }

        //Step - Close the app
        [Then(@"I close the application to end this test case")]
        public void ThenICloseWindow()
        {
            //Find close button and click
            var closeButton = GetControlByClassNameAndAutomationIdForElement(_guideWindow, "Button", "PART_CloseButton");
            Click(closeButton);

            //Find safety popUp
            var popupWindow = GetPopupWindowWithTitle("Closing 'EB GUIDE Studio'");
            if (popupWindow != null)
            {
                var closingButton = GetControlByClassNameAndNameForElement(popupWindow, "Button", "No");
                closingButton.Click();
            }
        }

        //Start Application method
        private static void StartApplication(string executableName)
        {
            //Wait between test cases
            Thread.Sleep(TimeSpan.FromSeconds(5));

            //Create base domain and execute app
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            var parentDir = Directory.GetParent(baseDirectory).FullName;
            var pathToGuide = Path.Combine(parentDir, executableName);

            //Starting process
            var processStartInfo = new ProcessStartInfo(pathToGuide);
            _guideApplication = Application.AttachOrLaunch(processStartInfo);
            _guideApplication.WaitWhileMainHandleIsMissing(TimeSpan.MaxValue);
            GiveEnoughTimeForStartup();
        }

        //Get control by class name and automationID
        private AutomationElement GetControlByClassNameAndAutomationId(string className, string automationId)
        {
            return GetControlByClassNameAndAutomationIdForElement(_guideWindow, className, automationId);
        }

        //Get control by class name and automationID for element
        private static AutomationElement GetControlByClassNameAndAutomationIdForElement(AutomationElement window, string className, string automationId)
        {
            var id = automationId.Replace(" ", string.Empty);

            return GetControlBySearchCriteria(window, cf => cf.ByAutomationId(id).And(cf.ByClassName(className)));
        }
        
        //Get colors method (for debug purposes)
        private static AutomationElement GetColors(AutomationElement window, String xpath)
        {
            var c = window.FindFirstByXPath(xpath);
            return c;
        }

        //Get control by class name and name for element
        private static AutomationElement GetControlByClassNameAndNameForElement(AutomationElement window, string typeName, string name)
        {
            return GetControlBySearchCriteria(window, cf => cf.ByClassName(typeName).And(cf.ByName(name)));
        }

        //Get control by class name and text for element
        private static AutomationElement GetControlByClassNameAndTextForElement(AutomationElement window, string typeName, string title)
        {
            return GetControlBySearchCriteria(window, cf => cf.ByClassName(typeName).And(cf.ByText(title)));
        }

        //Get automation element by specified search criteria
        private static AutomationElement GetControlBySearchCriteria(AutomationElement window, Func<ConditionFactory, ConditionBase> conditionFunc)
        {
            var c = window.FindFirstDescendant(conditionFunc);
            //Assert.That(c, Is.Not.Null);
            if (c != null)
            {
                Wait.UntilResponsive(c);
            }

            return c;
        }

        //Wait 10 second in currect thread
        private static void GiveEnoughTimeForStartup()
        {
            Thread.Sleep(TimeSpan.FromSeconds(10));
        }

        //Enter text to the textboxes method
        private static void EnterText(AutomationElement element, string project)
        {
            Wait.UntilResponsive(element);
            element.AsTextBox()?.Enter(project);
            Wait.UntilInputIsProcessed();
        }
    }
}