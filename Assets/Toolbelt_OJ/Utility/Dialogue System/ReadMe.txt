Dialogue System Components Overview & Set-up instructions:

- Add Dialogue System Prefab to canvas
- Add start dialogue component to an object in the scene
- Start dialogue with either the 'EnterDialogue' or 'EnterDialogueWithRandomNPC' methods

- To create an NPC:
	- Right Click in the Project Window > Create > DialogueSystem > NPCInfo
	- Set NPC Name
	- Set personality values
		- Happy, Angry & Surprised Min Threshold must be over 0
			- it will take longer to change to these moods the higher these values are (respectively)
		- Sad, Nervous & Scared Min Threshold must be below 0
			- it will take longer to change to these moods the lower these values are (respectively)

	- Emotion (These values can be left as default)
		- NPC Mood is determined by a combination of the emotions that are above their min threshold

		- if happiness > happy min threshold the npc will have a happy disposition
		- if happiness < sad min threshold the npc will have a sad disposition

		- if stress > angry min threshold the npc will have an angry disposition
		- if stress < nervous min threshold the npc will have a nervous disposition

		- if shock > surprised min threshold the npc will have a surprised disposition
		- if shock < scared min threshold the npc will have a scared disposition

	- Dialogue
		- Add potential greeting npc dialogue to greeting dialogue list
		- Add potential goodbye npc dialogue to goodbye dialogue list
		- Add potential changing topic npc dialogue to change topic dialogue list

		- Dialogue Connections:
			- This changes the NPC's available responses depending on their mood
			- There are 20 moods in total, which means each player dialogue option can have up to 20 responses
				- If there is no dialogue option corresponding to the npc's current mood, it will find the best available response
					- which is the emotion that is furthest past its minimum threshold and is an available response
				- The only required mood is calm
			- Set up:
				- Create a new element for every player dialogue option that can be said to this npc
				- Assign each potential player dialogue option to the Player Dialogue Input variable
				- Assign each potential npc dialogue response to the response variable in the NPC Responses list
				- Assign the required mood for each potential npc dialogue response in the NPC Mood variable of the NPC Responses list


- To create player dialogue:
	- Right Click in the Project Window > Create > DialogueSystem > PlayerDialogueOption

	- Write the dialogue text in the Dialogue text box

	- Set happiness effect to a positive value to make the npc happier, a negative value to make the npc sadder, or 0 to have no effect when dialogue is selected
	- Set stress effect to a positive value to make the npc angrier, a negative value to make the npc more nervous, or 0 to have no effect when dialogue is selected
	- Set shock effect to a positive value to make the npc more surprised, a negative value to make the npc more scared, or 0 to have no effect when dialogue is selected


- To create NPC dialogue:
	- Right Click in the Project Window > Create > DialogueSystem > NPCDialogue

	- Write the dialogue text in the Dialogue text box
	- Tick 'Requires Response' to enable the player dialogue selection UI
	- Untick for multiple npc dialogue in a row
		- if unticked: Add the next npc dialogue to 'Continued Dialogue'
	- Add all potential player responses dialogue to 'Player Responses'
		- If left empty it will be autofilled by the player's available questions for the npc (in 'PlayerDialogue' script on the Dialogue System prefab
	- Tick limited time to force to player to respond in a certain time
		- if ticked set time limit
	- Tick 'Can Change Topic' to allow the player to change the topic or leave the conversation instead of selecting a dialogue option


- Dialogue System Script variables
	- In Dialogue: whether the player is currently in dialogue with an npc
	- Player Is Speaking: whether the player can respond
	- Timer Auto Start: whether to start the timer automatically or wait for a player input when time is limited

	- NPC: who the player is talking to
	- NPC Dialogue: the current npc dialogue option
	- selected dialogue option: the player's currently selected dialogue option

	- The rest is to control UI

- Player Dialogue Script:
	- Add potential greeting player dialogue to greeting dialogue list
	- Add potential goodbye player dialogue to goodbye dialogue list
	- Add potential changing topic player dialogue to change topic dialogue list

	- Player Questions:
		- Add every NPC to a new element in the list
		- Add all the dialogue options the player is able to ask them to the Questions For NPC list



	

	