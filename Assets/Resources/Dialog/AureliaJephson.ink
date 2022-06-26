VAR npc_name = "Aurelia Jephson"
VAR g_speeker = "Aurelia"
VAR meet_player = false

=== start ===
~ g_speeker = "Aurelia"
{ not meet_player:
    -> intro
}
-> talk

=== intro ===
Hi stranger, My name is Aurelia
If you need something talk to my husband.
~ meet_player = true
#end
-> END

=== talk ===
I'm busy
#end
-> END
