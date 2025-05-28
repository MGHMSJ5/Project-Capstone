#Name:Chobo
-> main

=== main ===
Empty line
Oh, a stranger? I knew someone would come eventually! Welcome to our planet, Kettler 22-T! I am Chobo, head of communications around here.
You have no idea how pleased I am to see you. We've been sending out distress signals for a while now!
I would love to offer you some tea, or even a hot bath, but, well... Our Kettle is a little broken.
    + Distress signals?
    -> Distress 
    + I am here to help!
    -> HelpingHand
    + What a shame about the tea!
    -> What
    
=== Distress ===
Yes! We've been sending out messages to outer space for a while now! You must've heard at least one of them, right?
Well, even if you are just a traveler, maybe you could still be of help to us... Our planet is in big trouble.
The whole place is freezing over, and that's quite the contrast to what it's usually like here.
The world is normally warmer, especially around our spa. Our Kettle, which is supposed to keep everything warm, broke down.
We don't have a clue on how to repair it. Can you help us fix the Kettle, friend? 
    + I think I can!
    -> Yes
    
=== HelpingHand ===
Oh, so our messages were not sent in vain! Our hero has arrived in the shape of a metal lunchbox on legs!
We are in a bit of a situation, as you may have noticed. The planet is currently freezing, while it is supposed to be nice and cozy outside.
It is all because our big spa Kettle is busted, and we have no clue how to repair it.
Can you help us out? You look like someone who knows the difference between a hammer and a wrench.
    + Of course!
    -> Yes

=== What ===
It really is a shame. Normally, I would have offered you some tea or a hot bath as a traditional warm welcome to our beautiful home planet. 
But you also do not look like the kind to go swimming or... Can you even drink tea? 
Anyways, the Kettle that is supposed to provide the planet with hot water is kaput, and well, we do not have the skills or knowledge to repair it.
Would you be able to help us out by trying to fix it, biscuit tin?
    + Yes...
    -> Yes

    
=== Yes ===
Ah, perfect! You truly have a big heart for a robot of your size!
Alright, so before I send you off to the Kettle, follow me to the village for more information.
-> End

=== End ===

-> END