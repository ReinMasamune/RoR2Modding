This mod rebuilds the prototype version of sniper present in the files from the ground up. 
All four skills are rewritten from scratch and work significantly differently from how they did initially.  

If you have any issues contact me on the modding discord. (@Rein#7551)

### Important note:
In order for this mod to work in multiplayer, both the host and the client must have it installed. If both do not have it there will be bugs.

Due to the nature of these changes, it is not possible to avoid this without circumventing major portions of the game code.

### Base Stats
- 16 damage (highest of any survivor. Engie has 15, Mul-T has 11, everyone else has 12) Everyone gains 20% base per level
- 100 hp (Second lowest, all gain +30% per level)
- All other stats are identical to other ranged survivors (commando, artificer, huntress)

### Headshot (passive)
- Deal 50% extra damage when hitting some enemies in the head (multiplies with other damage boosts)
- Currently, this only works on Lemurians, Beetles, and Beetle Guards
- The kit was designed without this being taken into account at all, so think of it as a little side bonus
- Hopefully in the future I can get headshot hitboxes in place for other enemies

### Snipe
- Does 250% base damage
- Perfect reload = 2x damage
- Good reload = 1.45x damage
- Bad reload = 0.75x damage
- Has recoil that very slightly pushes you back. This can be significant with attack speed, allowing you to stay in the air longer.
- At base attack speed between 1 and 1.3 seconds between shots
- Bar speed scales with attack speed, but has a diminishing return formula to keep it from becoming impossible. Note that the bar is only half the time between shots, and the rest of this attack scales as normal with attack speed

### Steady Aim
- Aim down the scope of the rifle
- While aiming all shots pierce
- Can use the mousewheel to zoom in and out
- You move slower while aiming
- While aiming damage builds up over time
- There are three sections to the charge
- Black section: No change in damage, 30% slow, charge resets
- Blue section: 1.06x-1.32x damage, 50% slow, charge resets
- Red section: 9.5x-10x damage, 2.0 proc coef, 80% slow, has a 30 second cooldown, charge does not reset
- All increases multiply with the reload bonuses, meaning up to 20x damage, all of which can crit and influence the damage of procs
- Scope and blue charge can both be used when skill is on cooldown
- While on cooldown red charge functions as blue charge
- Takes 10 seconds to fully charge.
- 2.5 seconds is the start of blue
- 7.5 seconds is the start of red
- The charge speed increases with attack speed
- No capping, so with lots of attack speed all shots will be fully charged, takes a bit over 100 syringes
- Because red charge does not consume the charge, backup mags allow you to use multiple full charge shots in quick succession

### Military Training
- Dash forward in the direction of your character's velocity
- While dashing and for a very short duration after gain invisibility
- 8 second cooldown
- Invisibility causes enemies to lose focus, if you break line of sight
- The direction of this skill is only affected by the direction your character is moving
- Aim direction and the movement keys have no impact
- This makes it not functional for canceling out movement
- At the same time, this makes it very flexible, activate right after jumping to get a higher jump

### Snare Trap
- Places a trap mine on the ground or on an enemy
- 2.5 seconds to arm
- Has a wide trigger distance
- 30 second cooldown
- When activated creates a cripple zone that reduces armor and movespeed for all enemies inside
- Pulls nearby enemies towards the middle with a weak vortex effect
- In addition, will periodically send hooks out to the most distant enemies and drag them closer to the middle
- Lasts for 10 seconds after triggering
- Does no damage, and cannot apply items
- Primarily used to bunch groups of enemies together for piercing shots from steady aim
- Also has uses to CC enemies and escape, or to boost your damage output against a boss

## Future plans (Subject to change)
### Real soon
- Better sound and visual effects

### Less soon
- Additional features for the scope crosshair
- Config file stuff

### Later
- Getting animations to look better/work in general

### Eventually
- Custom texture for the model

### Soon (tm)
- A new model

## Changelog
### 1.0.01
- First release
