#Name:Toaster
->Main

===Main===
Empty Line
\*You pick up the toaster take a look at the it.*
\*The toaster looks fine, but you hear soft rattling noises coming from inside of it when picked up.*
\*What is your next step?*
    + \*Shake the toaster around.*
    ->Shake
    + \*Look inside of the toaster.*
    ->Look
    + \*Put the toaster back down*
    ->Nothing

===Shake===
\*You shake the toaster around and it makes rattling noises. Like there is a lot of stuff stuck inside of the toaster.
->Main2

===Main2===
\*What is your next step?*
    + \*Look inside of the toaster.*
    ->Look
    + \*Hold the toaster upside down and shake it around.*
    ->turnshake
    
===turnshake===
\*You hold the toaster upside down and shake it around. A lot of dirt that was stuck inside of the toaster falls out of it and the toaster should be functional again*
->END

===Look===
\*You look inside the toaser. There is a lot of dirt stuck inside of it.*
->Main3

===Main3===
\*What is your next step?*
    + \*Shake the toaster around.*
    ->Shake
    + *Hold the toaster upside down and shake it around.*
    ->turnshake

===Nothing===
\*You put the toaster back down on the ground.*
->END