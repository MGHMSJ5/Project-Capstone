#Name:Chobo
//Lea: Thomas, there are a few comments in this file that need your attention!
//Lea: You can also edit some stuff of mine if you don't like it!
-> main

=== main ===
Empty line
Oh, a stranger? I knew someone would come eventually! Welcome to our planet, Kettler 22-T! 
You have no idea how happy I am to see you. We've been sending out distress signals for ages!
I would love to offer you some tea, but well.. Our Kettle is a little broken.
    + Distress signals?
    -> Distress 
    + I've come to help!
    -> HelpingHand
    + I didn't hear anything. //Lea: This needs changing for sure, but I don't know into what direction
    -> What
    
=== Distress ===
Yes! We've been beaming out messages non-stop! You must've heard at least one of them, right?
Well, even if you are just a traveler, maybe you could still be of help to us.. Our planet is in big trouble.
The whole place is freezing, and that's not supposed to happen.
The world is normally warmer, especially around our spa. Our Kettle, which is supposed to keep everything warm, broke down.
We don't know what to do anymore. Can you help us fix the Kettle, friend? //Lea: Are options like this fine? With just one answer? Otherwise, you can add something that doesn't feel forced.
    + Yes
    -> Yes
    
=== HelpingHand ===
Oh, so our messages have reached a hero! You are a lifesaver, friend!
We are in a bit of a situation, as you may notice. The planet is freezing, while it is normally a lot warmer than this.
Our big spa Kettle is busted, and we have no clue how to repair it.
Can you help us out? You look like someone who knows the difference between a hammer and a wrench.
    + Yes
    -> Yes

=== What ===
//Lea: Since I don't know what direction this goes in, you can do something creative here
Really? We've been beaming out messages non-stop! Are you sure you heard nothing?
Well, even if you are just a traveler, maybe you could still give us a hand.
Our planet is normally a lot hotter than this. The whole place

Are you sure? Why else would you be here?
Anyway, we have a big problem.
As you can see, the entire planet is kind off cold.
We are usually a warm planet, but our big spa kettle broke down.
Do you think you could help us?
    + Yes
    -> Yes

    
=== Yes ===
Oh, thank you friend! You are a hero!
-> End

=== End ===
//Lea: dude runs to the village and I have no clue why. Please make the connection, cause I have no ideas for it. 

-> END