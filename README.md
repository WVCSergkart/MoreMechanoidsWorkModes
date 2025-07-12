# More Mechanoids Work Modes
**Adds 14 new work mods for mechanoids:**

**_Find and destroy_** - Mechanoids, regardless of their type, will look for enemies and try to destroy them.

**_Work and recharge_** - Work autonomously according to mechanoid type. If the mechanoid cannot work or it has done all the work it shuts down. (Does the same thing as [this mod](https://steamcommunity.com/sharedfiles/filedetails/?id=2885792215), but for all mechanoids.)

**_Escort and recharge_** - The mechanoid, regardless of its type, will accompany the overseer and, if necessary, go to charge.

**_Work, recharge and escort_** - The mechanoid will accompany the overseer if it does not have a job, and will also be charged if necessary.

**_Wait enemy_** - The mechanoid will shutdown and wait for the enemy, and when it appears, it will wake up and head towards the target.

**_Work and wait_** - Mechanoids, depending on the type, will work, and when an enemy appears, they will drop everything and go to destroy him. If they don't have a job, they'll just shutdown. (The shutdown lasts longer than in "wait enemy" mode)

**_Ambush_** - The mechanoid shutdown and waits for the enemy to approach it, then attacks.

**_Defend yourself_** - Mechanoids, regardless of type, will attack enemies that get too close. Or if damaged, they will try to retreat to the nearest safe zone.

**_Escort if enemy_** - Mechanoids, regardless of type, will accompany the overseer, but only if there is an enemy on the map. When there is no enemy, mechanoids shutdown.

**_Work if safe_** - Mechanoids will work, but only as long as there are no enemies on the map. In case of danger, they will try to get to the safe spot.

## Bonus modes
_**Escort if enemy, work and recharge**_ - Mechanoids, regardless of their type, will accompany the overseer when an enemy is in their zone of operation (within area). Otherwise, they will work.

_**Escort if drafted or downed**_ - Mechanoids, regardless of type, will accompany an overseer if it is drafted or downed.

_**Hive mind research**_ - Mechanoids, regardless of type, will generate research points relative to their bandwidth cost. The speed and efficiency of research directly depends on the number of mechanoids involved in it. **This mode requires the "Mech research mode" technology.**

_**Scavenging**_ - Mechanoids, regardless of type, will search for useful resources in special "Scavenge" zones. **This mode requires the "Scavenging mode" technology.**

_**Bonus modes require manual activation in settings!**_

## Unique behaviors
**_Checking for enemies on the map_** - if the enemy is reachable and is not in the area in which the mechanoid operates, then False is returned and the mechanoid will continue to work normally.

_What does it mean:_
_If_ you enable the "Work if safe" mode and limit the working area, the mechanoid will ignore the intrusion until the enemy is in the mechanoid's working area.
_If_ you have enabled the "Find and destroy" mode and locked the mechanoids in a small but open area, then the mechanoid will not pursue enemies, but will try to fight with the enemy that is in the field of view of the mechanoid. This can lead to too smart AI kiting your mechs.

**_Smart charge_** - In some modes, mechanoids will try to maintain close to the maximum charge. This is calculated according to the formula (Maximum charge level - 5), that is, if you set the charge to 100%, the mechanoids will be charged up to 95%, and so on. However, unlike vanilla charging, mechanoids will be interrupted if necessary.

**_Shutdown mode_** - Some work modes automatically shutdown mechanoids if they have finished their work. For this purpose, special shutdown zones are used. Mechanoids can also shutdown even if these zones do not exist, but then they become an easy target for any threats.
In work (with work) modes, the cooldown is much longer than in combat (without work). (For work 1500 ticks (1 game hour). For combat 650 ticks.)

_Note:_ The shutdown state from the mod uses the vanilla shutdown job. That is, all triggers and checks will count mechanoids as if they were in vanilla "Shutdown mode". 
**For example, the game will not count shutdown paramedics as medics.** Distribute mechanoids into groups wisely!

**_Mechanoid shutdown zone_** - Mechanoids that have finished all their business and are going to sleep will go to a such zone, if it is in an accessible area for them.

**_Smart escort_** - Mechanoids can be assigned an escort purpose. This way the mechanoids will be able to protect and accompany different pawns. This feature only works for the home map.

**Version 1.5:**

**_Dormant mode_** - Mechanoids consume less bandwidth when they are in a shutdown state. Requires manual activation in settings.

**_Auto-repair On_** - New mechanoids **spawn** with auto-repair enabled. Enabled by default.[b]Version 1.6:[/b]

**Version 1.6:**

**_Mechanoid hangar_** - any room that has a mechanoid charger will be considered a hangar. If possible and in the absence of zones, mechanoids will be sent to a random hangar for shutdown. Enabled by default.