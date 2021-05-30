VAR g_players_name = ""
VAR npc_name = "Bill"
VAR g_speeker = "Bill"
VAR g_meet_smith = false

=== start ===
{ not g_meet_smith:
    -> meet_smith
}
-> intro

=== meet_smith ===
~ g_speeker = "???"
Talk to smith first.
#end
-> END

=== intro ===
~ g_speeker = "???"
I see you've meet smith.
~ g_speeker = "Bill"
My name is Bill.
Goodbye.
#end
-> END