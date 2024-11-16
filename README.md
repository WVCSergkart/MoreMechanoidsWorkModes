# More Mechanoids Work Modes or WVC - Work Modes
[i]Expanded mechanoids work modes[/i] or [i]Better mechanoids work modes[/i]
[b]Adds 14 new work mods for mechanoids:[/b]

[b][i]Find and destroy[/i][/b] - Mechanoids, regardless of their type, will look for enemies and try to destroy them.

[b][i]Work and recharge[/i][/b] - Work autonomously according to mechanoid type. If the mechanoid cannot work or it has done all the work it shuts down. (Does the same thing as [url=https://steamcommunity.com/sharedfiles/filedetails/?id=2885792215]this mod[/url], but for all mechanoids.)

[b][i]Escort and recharge[/i][/b] - The mechanoid, regardless of its type, will accompany the overseer and, if necessary, go to charge.

[b][i]Work, recharge and escort[/i][/b] - The mechanoid will accompany the overseer if it does not have a job, and will also be charged if necessary.

[b][i]Wait enemy[/i][/b] - The mechanoid will shutdown and wait for the enemy, and when it appears, it will wake up and head towards the target.

[b][i]Work and wait[/i][/b] - Mechanoids, depending on the type, will work, and when an enemy appears, they will drop everything and go to destroy him. If they don't have a job, they'll just shutdown. (The shutdown lasts longer than in "wait enemy" mode)

[b][i]Ambush[/i][/b] - The mechanoid shutdown and waits for the enemy to approach it, then attacks.

[b][i]Defend yourself[/i][/b] - Mechanoids, regardless of type, will attack enemies that get too close. Or if damaged, they will try to retreat to the nearest safe zone.

[b][i]Escort if enemy[/i][/b] - Mechanoids, regardless of type, will accompany the overseer, but only if there is an enemy on the map. When there is no enemy, mechanoids shutdown.

[b][i]Work if safe[/i][/b] - Mechanoids will work, but only as long as there are no enemies on the map. In case of danger, they will try to get to the safe spot.

[h1]Bonus modes[/h1]
[i][b]Escort if enemy, work and recharge[/b][/i] - Mechanoids, regardless of their type, will accompany the overseer when an enemy is in their zone of operation (within area). Otherwise, they will work.

[i][b]Escort if drafted or downed[/b][/i] - Mechanoids, regardless of type, will accompany an overseer if it is drafted or downed.

[i][b]Hive mind research[/b][/i] - Mechanoids, regardless of type, will generate research points relative to their bandwidth cost. The speed and efficiency of research directly depends on the number of mechanoids involved in it. [b]This mode requires the "Mech research mode" technology.[/b]

[i][b]Scavenging[/b][/i] - Mechanoids, regardless of type, will search for useful resources in special "Scavenge" zones. [b]This mode requires the "Scavenging mode" technology.[/b]

[i][b]Bonus modes require manual activation in settings![/b][/i]

[h1]Unique behaviors[/h1]
[b][i]Checking for enemies on the map[/i][/b] - if the enemy is reachable and is not in the area in which the mechanoid operates, then False is returned and the mechanoid will continue to work normally.

[i]What does it mean:[/i]
[i]If[/i] you enable the "Work if safe" mode and limit the working area, the mechanoid will ignore the intrusion until the enemy is in the mechanoid's working area.
[i]If[/i] you have enabled the "Find and destroy" mode and locked the mechanoids in a small but open area, then the mechanoid will not pursue enemies, but will try to fight with the enemy that is in the field of view of the mechanoid. This can lead to too smart AI kiting your mechs.

[b][i]Smart charge[/i][/b] - In some modes, mechanoids will try to maintain close to the maximum charge. This is calculated according to the formula (Maximum charge level - 5), that is, if you set the charge to 100%, the mechanoids will be charged up to 95%, and so on. However, unlike vanilla charging, mechanoids will be interrupted if necessary.

[b][i]Shutdown mode[/i][/b] - Some work modes automatically shutdown mechanoids if they have finished their work. For this purpose, special shutdown zones are used. Mechanoids can also shutdown even if these zones do not exist, but then they become an easy target for any threats.
In work (with work) modes, the cooldown is much longer than in combat (without work). (For work 1500 ticks (1 game hour). For combat 650 ticks.)

[i]Note:[/i] The shutdown state from the mod uses the vanilla shutdown job. That is, all triggers and checks will count mechanoids as if they were in vanilla "Shutdown mode". 
[b]For example, the game will not count shutdown paramedics as medics.[/b] Distribute mechanoids into groups wisely!

[b][i]Mechanoid shutdown zone[/i][/b] - Mechanoids that have finished all their business and are going to sleep will go to a such zone, if it is in an accessible area for them.

[b][i]Smart escort[/i][/b] - Mechanoids can be assigned an escort purpose. This way the mechanoids will be able to protect and accompany different pawns. This feature only works for the home map.

[b]Version 1.5:[/b]

[b][i]Dormant mode[/i][/b] - Mechanoids consume less bandwidth when they are in a shutdown state. Requires manual activation in settings.

[b][i]Auto-repair On[/i][/b] - New mechanoids [b]spawn[/b] with auto-repair enabled. Enabled by default.