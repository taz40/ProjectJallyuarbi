VAR players_name = "unknown"
VAR npc_name = "unknown"
VAR meet_player = false

{ not meet_player:
    -> intro
}
-> help

=== intro ===
~ meet_player = true
Hello traveler! What's your name?
+ [{players_name}]
    Well, nice to meet you {players_name}. I'm {npc_name}, the local blacksmith.
    Weapons, Shields, Armor I got it all.
    -> help

=== help ===
What can I do for you, {players_name}?
+ [buy weapon] -> buy_weapon
+ [buy armor] -> buy_armor
+ [done]
    Come back soon!
    #end
    -> END
    
=== buy_weapon ===
See anything you like? #buy weapon
+ [back] -> help

=== buy_armor ===
This is what I got right now. #buy armor
+ [back] -> help