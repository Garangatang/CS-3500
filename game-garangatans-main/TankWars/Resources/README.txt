README PS9
Special Things:
	-Implemented different gamemode which can be activated by typing EXTRA into the Mode parameter within the settings file. Spelling is also case insensitive.
	-If you type BASIC into the gamemode parameter, then TankWars will be played with the baseline parameters, also case insensitive.
	-If the gamemode is set to EXTRA, then there are three different power ups which can be gained instead of only the beam attack.
	-First powerup is increased speed which will last for 300 frames.
	-Second powerup is increased firing rate which will last for 300 frames.
	-Lastly is invincibility, where no damage can be taken, for 300 frames.
	-When a powerup is collected, one of these three power ups will be randomly given to the collector.

Methods:
	To begin with this project, we used the help session recording from Ella in order to have a baseline of starting code for: 
accepting new clients, maintaining connections, sending startup information, updating the world as needed, receiving control commands, 
and implementing control commands. Help session code gave us a very good starting point, but we also made sure to not just purely copy 
code from it. We spent a fair amount of time watching the recording, stopping to understand the written code, then implementing and testing 
it for ourselves. Understanding this code gave us a leg up which was very much appreciated and got us well past sprint 1.
	Sprint 2 was the next phase as outlined by the PS9 document. For this phase we took the baseline code previously developed 
and started to implement receiving and sending information for projectiles. The continuous stream of information from each tank included 
a turret direction which would be used for assessing which direction a projectile needed to be fired each time a control command for firing 
was sent. Upon successful completion of this, we added in projectile collisions for walls and tanks. If the projectile collided with a wall, 
it would be set to dead and removed on the next frame. If a projectile hit the tank, then the hit tank’s HP was decreased and the firing tank’s 
points would be increased if the projectile collision resulted in a death. 
	Tanks could now be killed, so this means we could tackle the problem of respawning a tank at a random location on the map where it did 
not collide with anything. A method was used to find a random location within the world for the tank to respawn, then this new random location 
was checked against the location of all the walls to see if there would be any collisions present. If there were collisions present then a new 
random location was found until it had no collisions. Respawn rate was also controlled for through a counter, which would count the number of 
frames before a tank could be spawned in 
	Successfully adding in projectiles then allowed us to move on to implementing the beam attack. The beam attack was linked to the 
collection of a powerup, where when a tank collided with a powerup, the powerup died and was removed next frame then the tank had a bool 
set to true for having a powerup. Beams were sent for only one frame upon a ctrl command for firing being sent with a value of “alt”. Tank 
collisions with the beam were handled by the class given in the PS9 instructions. If a beam and tank collided then the tank which collided 
with the beam immediately had its health set to zero and was respaned at a random location next frame.
For testing purposes many values had been hard coded previously. As outlined by the instructions though, we needed to be able to read in 
game settings from a .xml file. We used previous code from the spreadsheet class containing functions for reading a .xml file in order to pull 
the wanted information from the file as necessary. A basic settings file given to us during a lab session was used to read in the basic information 
of walls, and tank location. 
	Lastly we implemented a new game mode which can be played through the settings outlined above. Having the game mode set to 
extra activated a boolean telling the rest of the classes to operate with different power ups. Since our game mode dealt with powerups, the tank 
class, and world class were the classes which needed to be updated. If a new powerup was collected, then an integer ranging from 0 to 2 would be 
selected randomly to give one of three buffs. If a speed boost was gained, the tank’s engine power would be increased for 300 frames. If a power 
up for increased firing rate was gained, then the number of frames before a frame could fire again would be decreased for 300 frames. Lastly, if 
invincibility was gained, checking for collisions between projectiles and tanks had an additional if statement where if invincibility was true for
the collided tank, no damage would be taken.

