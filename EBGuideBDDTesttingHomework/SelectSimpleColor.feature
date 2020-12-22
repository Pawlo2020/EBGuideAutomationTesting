Feature: Check if the color picker correctly selects colors
	I wanted to know if color picker correctly changes colors
	for the elements

@SelectSimpleColor
Scenario: The select simple color test
	Given I opened EGBuide Studio and created new project named Project1
	And I clicked color picker Button with automationId PART_ColorPickerToggleButton
	And I selected dark orange color in ListBox with number 34
	And I clicked the color properties in Advanced options
	Then Check if hex code #FFFF8C00
	And Check if RGBA values are correct R - 255 G - 140 B - 0 A - 255
	And I close the application to end this test case

@ChangeColorProperties
Scenario: The change color values test
    Given I opened EGBuide Studio and created new project named Project1
	And I clicked color picker Button with automationId PART_ColorPickerToggleButton
	And I clicked first time the color properties in Advanced options
	And I entered hex color value #ABABFAAF
	And I double clicked the color properties in Standard options
	Then Check if RGBA values are correct R - 171 G - 250 B - 175 A - 171
	And  Change RValue - 241 GValue - 196 BValue - 15 AValue - 255
	Then Check if hex code #FFF1C40F
	And I close the application to end this test case
