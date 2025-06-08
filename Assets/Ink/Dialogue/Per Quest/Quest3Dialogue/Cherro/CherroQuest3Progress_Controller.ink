//After the player starts the 4th quest, they can return back to Cherro.
//Explain the controls for pulse again, if they forgot - Controller controls.

#Name:Cherro
->Main

===Main===
So, have you forgotten how the pulse works? No worries.
+I have!
->Forgot
+I remember!
->Remember

===Forgot===
Alright, when you press the left bumper on your controller a pulse happens around the player that can trigger panels and doors.
+Got it!
->Remember

===Remember===
Now you can enter the factory and help out my colleagues with the plug!
->END