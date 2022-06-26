VAR g_players_name = "unknown"
VAR meet_player = false
VAR npc_name = "Jack Jephson"
VAR g_speeker = "???"
VAR g_meet_jack = false

=== start ===
{ not meet_player:
    ~ g_speeker = "???"
    -> intro
}
~ g_speeker = "Jack"
-> talk

=== intro ===
~ meet_player = true
Excuse me sir or madam, but are you a floating sword?
+ [Yes I am!]
Very Interesting!
-> name

=== name ===
Do you have a name Mr. floating sword?
+ [Yes]
Nice to meet you {g_players_name}, my name is Jack Jephson
~ g_speeker = "Jack"
~ g_meet_jack = true
Though you can just call me Jack
-> talk

=== talk ===
What can I do for you {g_players_name}?
+ [Tell me about yourself]
    Well, My family moved out here a few years back to get away from the city.
    I live here with my wife, Aurelia, and daughter, Adeline.
    My wife is always cooking and Adeline is usually out playing in the pond.
    ->talk
+ [Wooden Golem?]
    A wooden Golem just outside of HogsFeet? That's a scary thought.
    The old man up north is always rambling about a monster in the forest Though
    ->talk
+ [Nothing]
    Alrighty then, See you around!
    #end
    -> END
