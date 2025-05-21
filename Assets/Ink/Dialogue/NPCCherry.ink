-> main

=== main ===
Empty line
1. Hey, it's nice to see a new face on this planet.
2. Do you think you could help me?
    + Yeah, sure!
    -> yes("Yeah, sure!")
    + Not right now.
    -> no ("Not right now.")
    
    === yes (understanding) ===
    1. Could you look at my toaster? It doesn't seem to work anymore.
    2. It's right over there.
    -> END
    
    === no (understanding) ===
    1. Oh, ok then.
    -> END
    
-> END