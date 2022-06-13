VAR g_players_name = "unknown"
VAR g_meet_player = false
VAR npc_name = "Smith"
VAR g_speeker = "???"
VAR g_meet_smith = false

=== start ===
{ not g_meet_player:
    -> intro
}
~ g_speeker = "Smith"
-> help

=== intro ===
~ g_meet_player = true
Hello traveler! What's your name?
~ g_speeker = g_players_name
My name is {g_players_name}.
~ g_speeker = "Smith"
~ g_meet_smith = true
Well, nice to meet you {g_players_name}. I'm Smith, the local blacksmith.
Weapons, Shields, Armor I got it all.
-> help

=== help ===
What can I do for you, {g_players_name}?
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