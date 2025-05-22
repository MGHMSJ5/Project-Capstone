-> main

=== main ===
Empty Line
You take a look at the toaster.
It looks like a normal toaster. Nothing wrong with it.
What do you want to do?
    + Shake the toaster.
    -> shake ("Shake the toaster.")
    + Look inside the toaster.
    -> look ("Look inside the toaster.")
    + Do nothing.
    -> nothing ("Do nothing.")

=== shake(understanding) ===
You shake the toaster. It sounds like there is something inside.
-> main2

=== main2 ===
What do you want to do?
    + Look inside the toaster.
    -> look ("Look inside the toaster.")
    + Turn the toaster around and shake.
    -> turnshake ("Turn the toaster around and shake.")
    
=== turnshake(understanding) ===
You turn the toaster around and shake. There is a lot of dirt coming out of the toaster. It looks clean again and should work now.
-> END

=== look(understanding) ===
You look inside the toaser. It looks really dirty.
-> main3

=== main3 ===
What do you want to do?
    + Shake the toaster.
    -> shake ("Shake the toaster.")
    + Turn the toaster around and shake.
    -> turnshake ("Turn the toaster around and shake.")

=== nothing(understanding) ===
You chose to do nothing!
-> END